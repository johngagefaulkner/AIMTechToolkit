using Azure.Identity;
using Microsoft.Graph;

namespace AIMTechToolkit.Services.MSGraph
{
	internal static class MSGraphClientService
	{
		public static GraphServiceClient CreateClientUsingDeviceCodeAuth()
		{
			try
			{
				var scopes = Common.Constants.Scopes;
				var tenantId = Common.Constants.TenantId;
				var clientId = Common.Constants.ClientId;

				// using Azure.Identity;
				var options = new DeviceCodeCredentialOptions
				{
					AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
					ClientId = clientId,
					TenantId = tenantId,
					// Callback function that receives the user prompt
					// Prompt contains the generated device code that user must
					// enter during the auth process in the browser
					DeviceCodeCallback = (code, cancellation) =>
					{
						StringBuilder sb = new();
						sb.AppendLine($"[Services.MSGraph.MSGraphClientService.CreateClientWithDeviceCodeAuthAsync] Please sign in with the following code: {code.UserCode}");
						sb.AppendLine($"[DeviceCode | DeviceCode] {code.DeviceCode}");
						sb.AppendLine($"[DeviceCode | Message] {code.Message}");
						sb.AppendLine($"[DeviceCode | Verification URL] {code.VerificationUri}");
						sb.AppendLine($"[DeviceCode | Expiration] {code.ExpiresOn}");
						Log.Info(sb.ToString());

						LoginDialogContentPage.Current.PopulateDeviceCodeLoginData(code.UserCode, code.Message, code.VerificationUri.ToString());

						return Task.FromResult(0);
					},
				};

				// https://learn.microsoft.com/dotnet/api/azure.identity.devicecodecredential
				var deviceCodeCredential = new DeviceCodeCredential(options);

				return new GraphServiceClient(deviceCodeCredential, scopes);
			}

			catch (Exception ex)
			{
				Log.Error(ex, "[Services.MSGraph.MSGraphClientService.CreateClientUsingDeviceCodeAuth] Exception occurred while creating GraphServiceClient using DeviceCodeCredential.");
				return null;
			}
		}
	}
}
