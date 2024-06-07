﻿namespace AIMTechToolkit.Models
{
	internal class MicrosoftAccountProfileInfo
	{
		public string odatacontext { get; set; }
		public string[] businessPhones { get; set; }
		public string displayName { get; set; }
		public string givenName { get; set; }
		public string jobTitle { get; set; }
		public string mail { get; set; }
		public string mobilePhone { get; set; }
		public string officeLocation { get; set; }
		public object preferredLanguage { get; set; }
		public string surname { get; set; }
		public string userPrincipalName { get; set; }
		public string id { get; set; }

		public static MicrosoftAccountProfileInfo FromJson(string json) => JsonSerializer.Deserialize<MicrosoftAccountProfileInfo>(json);
	}
}
