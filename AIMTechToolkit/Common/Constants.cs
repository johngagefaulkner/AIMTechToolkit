namespace AIMTechToolkit.Common
{
	internal static class Constants
	{
		// 'Tenant ID' of your Microsoft Entra instance - this value is a GUID
		public const string TenantId = "031a550a-f1f3-4b62-9c64-3ef02c7798a5";

		// 'Application (client) ID' of app registration in Azure portal - this value is a GUID
		public const string ClientId = "bad777e8-f3e5-4b86-b813-9217f1254faf";

		public const string Authority = "https://login.microsoftonline.com/031a550a-f1f3-4b62-9c64-3ef02c7798a5/";

		public const string DeviceCodeLoginUrlGeneric = "https://microsoft.com/devicelogin";

		public const string DeviceCodeLoginUrl = "https://login.microsoftonline.com/common/oauth2/deviceauth";

		public const string DeviceCodeLoginUrlWithTenantId = "https://login.microsoftonline.com/031a550a-f1f3-4b62-9c64-3ef02c7798a5/oauth2/deviceauth";

		public const string RedirectUrl = "https://login.microsoftonline.com/common/oauth2/nativeclient";

		public const string OAuth2AuthorizationEndpoint = "https://login.microsoftonline.com/031a550a-f1f3-4b62-9c64-3ef02c7798a5/oauth2/v2.0/authorize";

		public const string OAuth2TokenEndpoint = "https://login.microsoftonline.com/031a550a-f1f3-4b62-9c64-3ef02c7798a5/oauth2/v2.0/token";

		public static readonly string[] Scopes = [
			"user.read",
			"Application.Read.All",
			"Device.Read",
			"Device.Read.All",
			"DeviceManagementApps.Read.All",
			"DeviceManagementConfiguration.Read.All",
			"DeviceManagementManagedDevices.Read.All",
			"DeviceManagementServiceConfig.Read.All",
			"email",
			"offline_access",
			"openid",
			"Presence.Read",
			"Presence.Read.All",
			"PrinterShare.Read.All",
			"PrinterShare.ReadBasic.All",
			"profile"
		];
	}
}
