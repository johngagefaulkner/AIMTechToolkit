namespace AIMTechToolkit.Controls
{
	public sealed partial class LoginDialogContentPage : Page
	{
		internal string LoginDeviceCode { get; set; } = "000000";
		internal string LoginMessage { get; set; } = "";
		internal string LoginButtonText { get; set; } = "Click Here to Login";
		internal Uri LoginButtonUrl { get; set; } = new Uri(Constants.DeviceCodeLoginUrlGeneric);

		internal static LoginDialogContentPage CurrentlyDisplayedLoginPage { get; set; }

		public LoginDialogContentPage()
		{
			this.InitializeComponent();
			App.InstanceLoginPage = this;
			CurrentlyDisplayedLoginPage = this;
		}

		private async void loginDialogContentPage1_Loaded(object sender, RoutedEventArgs e)
		{
			if (MicrosoftLoginService.IsUserLoggedIn)
			{
				NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);
				App.InstanceMainWindow.ShowNotification("Information", "User is already logged-in!");
			}

			else
			{
				var _authResult = await Services.Auth.DeviceCodeFlow.GetATokenForGraph();
				if (_authResult is not null)
				{
					Log.Info("Login was successful!");
					App.InstanceMainWindow.ShowNotification("Success", "You successfully logged into your Microsoft account!");
					NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);
					MicrosoftLoginService.SetAuthenticatedUser(_authResult);
					await App.InstanceMainWindow.UpdateUserProfilePicture();
				}
			}
		}

		internal void UpdateLoginData(string _code, string _message, string _url)
		{
			deviceCodeTextBlock1.Text = _code;
			deviceLoginMessageTextBlock1.Text = _message;
			hyperlinkButton1.NavigateUri = new Uri(_url);
		}

		internal void UpdateLoginDataFromBag()
		{
			var dataArray = App.DeviceCodeLoginData.ToArray();
			deviceCodeTextBlock1.Text = dataArray[0];
			deviceLoginMessageTextBlock1.Text = dataArray[1];
			hyperlinkButton1.NavigateUri = new Uri(dataArray[2]);
		}
	}
}
