using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace AIMTechToolkit.Services.Auth
{
	internal static class RunDeviceCodeTest
	{
		// User auth token credential
		//private static DeviceCodeCredential? _deviceCodeCredential;
		// Client configured with user authentication
		private static GraphServiceClient _userClient;

	}

	internal static class AuthWithDeviceCodeService
	{
		private const string ClientId = Common.Constants.ClientId;
		private const string Authority = Common.Constants.DeviceCodeLoginUrlWithTenantId;
		private static readonly string[] scopes = Common.Constants.Scopes;

		public static async Task<string> GetValidAccessTokenAsync()
		{
			IPublicClientApplication app = PublicClientApplicationBuilder.Create(Common.Constants.ClientId)
				.WithRedirectUri("http://localhost")
				.WithTenantId(Common.Constants.TenantId)
				.Build();

			IEnumerable<IAccount> accounts = await app.GetAccountsAsync().ConfigureAwait(false);
			AuthenticationResult result;
			try
			{
				result = await app.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync().ConfigureAwait(false);
			}
			catch (MsalUiRequiredException)
			{
				// A MsalUiRequiredException happened on AcquireTokenSilent.
				// This indicates you need to call AcquireTokenInteractive to acquire a token
				result = await app.AcquireTokenInteractive(scopes).ExecuteAsync().ConfigureAwait(false);
			}

			return result.AccessToken;
		}

		internal static async Task<AuthenticationResult> GetATokenForGraphAsync()
		{
			IPublicClientApplication pca = PublicClientApplicationBuilder
			   .Create(ClientId)
			   .WithAuthority(Authority)
			   .WithRedirectUri("http://localhost")
			   .Build();

			var accounts = await pca.GetAccountsAsync();

			// All AcquireToken* methods store the tokens in the cache, so check the cache first
			try
			{
				Log.Info("Attempting to silently acquire an Access Token from cache, please wait...");
				return await pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync();
			}

			catch (MsalUiRequiredException ex)
			{
				// No token found in the cache or Azure AD insists that a form interactive auth is required (e.g. the tenant admin turned on MFA)
				// If you want to provide a more complex user experience, check out ex.Classification 
				Log.Error(ex, "No token found in the cache or Azure AD insists that a form interactive auth is required.");
				return await AcquireByDeviceCodeAsync(pca);
			}

			catch (Exception ex)
			{
				// Generic catch-all for any other errors
				Log.Error(ex, "[Services.Auth.AuthWithDeviceCodeService.cs] [Task<AuthenticationResult> GetATokenForGraphAsync()] Failed to retrieve a token for Microsoft Graph. Generic error/failure.");
				return await AcquireByDeviceCodeAsync(pca);
			}
		}

		private static async Task<AuthenticationResult> AcquireByDeviceCodeAsync(IPublicClientApplication _instancePublicClientApplication)
		{
			try
			{
				var result = await _instancePublicClientApplication.AcquireTokenWithDeviceCode(scopes, 
					deviceCodeResult =>
					{
						// This will print the message on the console which tells the user where to go sign-in using 
						// a separate browser and the code to enter once they sign in.
						// The AcquireTokenWithDeviceCode() method will poll the server after firing this
						// device code callback to look for the successful login of the user via that browser.
						// This background polling (whose interval and timeout data is also provided as fields in the 
						// deviceCodeCallback class) will occur until:
						// * The user has successfully logged in via browser and entered the proper code
						// * The timeout specified by the server for the lifetime of this code (typically ~15 minutes) has been reached
						// * The developing application calls the Cancel() method on a CancellationToken sent into the method.
						//   If this occurs, an OperationCanceledException will be thrown (see catch below for more details).
						// Console.WriteLine(deviceCodeResult.Message);
						App.InstanceLoginPage.LoginDeviceCode = deviceCodeResult.UserCode;
						App.InstanceLoginPage.LoginMessage = deviceCodeResult.Message;
						App.InstanceLoginPage.LoginButtonUrl = new Uri(deviceCodeResult.VerificationUrl);
						Log.Info($"[Services.Auth.AuthWithDeviceCodeService.cs | AcquireByDeviceCodeAsync()] Device Code: {deviceCodeResult.UserCode}");
						return Task.FromResult(0);
					}).ExecuteAsync();

				var loginSuccessMessage = $"Successfully logged-in user: {result.Account.Username}";
				Log.Info(loginSuccessMessage);
				return result;
			}

			// TODO: handle or throw all these exceptions
			catch (MsalServiceException ex)
			{
				// Kind of errors you could have (in ex.Message)

				// AADSTS50059: No tenant-identifying information found in either the request or implied by any provided credentials.
				// Mitigation: as explained in the message from Azure AD, the authoriy needs to be tenanted. you have probably created
				// your public client application with the following authorities:
				// https://login.microsoftonline.com/common or https://login.microsoftonline.com/organizations

				// AADSTS90133: Device Code flow is not supported under /common or /consumers endpoint.
				// Mitigation: as explained in the message from Azure AD, the authority needs to be tenanted

				// AADSTS90002: Tenant <tenantId or domain you used in the authority> not found. This may happen if there are 
				// no active subscriptions for the tenant. Check with your subscription administrator.
				// Mitigation: if you have an active subscription for the tenant this might be that you have a typo in the 
				// tenantId (GUID) or tenant domain name.
				Log.Error(ex, "MsalServiceException");
				return null;
			}
			catch (OperationCanceledException ex)
			{
				// If you use a CancellationToken, and call the Cancel() method on it, then this *may* be triggered
				// to indicate that the operation was cancelled. 
				// See /dotnet/standard/threading/cancellation-in-managed-threads 
				// for more detailed information on how C# supports cancellation in managed threads.
				Log.Error(ex, "OperationCanceledException");
				return null;
			}
			catch (MsalClientException ex)
			{
				// Possible cause - verification code expired before contacting the server
				// This exception will occur if the user does not manage to sign-in before a time out (15 mins) and the
				// call to `AcquireTokenWithDeviceCode` is not cancelled in between
				Log.Error(ex, "MsalClientException");
				return null;
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Login with Device Code process failed.");
				return null;
			}
		}
	}
}

