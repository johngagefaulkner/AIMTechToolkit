namespace AIMTechToolkit.Pages
{
	public sealed partial class HomePage : Page
	{
		public const string PageHeader = "Home";

		public HomePage()
		{
			this.InitializeComponent();
		}

		private async void testBtn1_Click(object sender, RoutedEventArgs e)
		{
			if (MicrosoftLoginService.IsUserLoggedIn)
			{
				return;
			}

			else
			{
				if (await MicrosoftLoginService.SignInAsync())
				{
					await DialogService.ShowDialogAsync("Success", "You have successfully logged in!" + Environment.NewLine + Environment.NewLine + "Please press 'OK' to retrieve your account information.");

					var accountInfo = await MicrosoftGraphAPIService.GetProfileAsync();
					if (accountInfo is not null)
					{
						await DialogService.ShowDialogAsync("Account Information", accountInfo.userPrincipalName);
					}
				}

				else
				{
					await DialogService.ShowDialogAsync("Error", "There was an error logging in.");
				}
			}
		}
	}
}
