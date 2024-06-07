namespace AIMTechToolkit.Services
{
	internal static class MicrosoftLoginService
	{
		private static AuthenticationResult _authenticationResult;
		public static bool IsUserLoggedIn => _authenticationResult != null;
		public static AuthenticationResult GetAuthenticatedUser() => _authenticationResult;
		public static void SetAuthenticatedUser(AuthenticationResult authResult) => _authenticationResult = authResult;
		public static string GetAccessToken() => _authenticationResult.AccessToken;
		private static MicrosoftAccountProfileInfo _loggedInUserAccountInfo;
		public static void SetLoggedInUserAccountInfo(MicrosoftAccountProfileInfo msaProfile) => _loggedInUserAccountInfo = msaProfile;
		public static MicrosoftAccountProfileInfo GetLoggedInUserAccountInfo() => _loggedInUserAccountInfo;

		private static IPublicClientApplication MSALClientApp { get; set; } = null;

		public static IPublicClientApplication GetMSALPublicClientApp()
		{
			if (MSALClientApp is null)
			{
				// Initialize MSAL Public Client Application
				// Configure your public client application
				MSALClientApp =
				PublicClientApplicationBuilder.CreateWithApplicationOptions(new PublicClientApplicationOptions
				{
					// 'Tenant ID' of your Microsoft Entra instance - this value is a GUID
					TenantId = "031a550a-f1f3-4b62-9c64-3ef02c7798a5",

					// 'Application (client) ID' of app registration in Azure portal - this value is a GUID
					ClientId = "bad777e8-f3e5-4b86-b813-9217f1254faf"
				}).WithRedirectUri("http://localhost").Build();
			}

			return MSALClientApp;
		}

		public static async Task<bool> SignInAsync()
		{
			try
			{
				var msalPublicClientApp = GetMSALPublicClientApp();
				AuthenticationResult msalAuthenticationResult = null;

				// Acquire a cached access token for Microsoft Graph from a prior execution of this process.
				var accounts = await msalPublicClientApp.GetAccountsAsync();
				if (accounts.Any())
				{
					try
					{
						// Will return a cached access token if available, refreshing if necessary.
						msalAuthenticationResult = await msalPublicClientApp.AcquireTokenSilent(["https://graph.microsoft.com/User.Read"], accounts.First()).ExecuteAsync();

						Log.Info("Acquired token silently.");
					}

					catch (MsalUiRequiredException ex)
					{
						// Nothing in cache for this account + scope, and interactive experience required.
						Log.Warning($"MsalUiRequiredException: {ex.Message}");
					}
				}

				if (msalAuthenticationResult == null)
				{
					/* This is likely the first authentication request in the application, so,
					 * Calling this will launch the user's default browser and send them through a login flow.
					 * After the flow is complete, the rest of this method will continue to execute.
					 */

					msalAuthenticationResult = await msalPublicClientApp.AcquireTokenInteractive(["https://graph.microsoft.com/User.Read"]).ExecuteAsync();
				}

				var tokenWasFromCache = TokenSource.Cache == msalAuthenticationResult.AuthenticationResultMetadata.TokenSource;
				var tokenAcquisitionResponse = $"Access Token: {(tokenWasFromCache ? "Cached" : "Newly Acquired")} (Expires: {msalAuthenticationResult.ExpiresOn:R})";
				Log.Info(tokenAcquisitionResponse);

				_authenticationResult = msalAuthenticationResult;
				return true;
			}

			catch (Exception ex)
			{
				var errorMsg = $"ERROR: {ex.Message}" +
					Environment.NewLine + Environment.NewLine +
					$"Exception Stacktrace: {ex.StackTrace}" +
					Environment.NewLine + Environment.NewLine +
					"See the log for more details.";
				Log.Error(errorMsg);
				Log.Error(ex.ToString());

				return false;
			}
		}
	}
}
