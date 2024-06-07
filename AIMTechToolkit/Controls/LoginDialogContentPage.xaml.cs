using Microsoft.UI.Dispatching;

namespace AIMTechToolkit.Controls
{
	public sealed partial class LoginDialogContentPage : Page
	{
		private DispatcherQueue dispatcherQueue;

		internal string LoginDeviceCode { get; set; } = "000000";
		internal string LoginMessage { get; set; } = "";
		internal string LoginButtonText { get; set; } = "Click Here to Login";
		internal Uri LoginButtonUrl { get; set; } = new Uri(Constants.DeviceCodeLoginUrlGeneric);

		internal static LoginDialogContentPage CurrentlyDisplayedLoginPage { get; set; }

		public LoginDialogContentPage()
		{
			this.InitializeComponent();
			dispatcherQueue = DispatcherQueue.GetForCurrentThread();

			App.InstanceLoginPage = this;
			CurrentlyDisplayedLoginPage = this;
		}

		private async void loginDialogContentPage1_Loaded(object sender, RoutedEventArgs e)
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

		internal void UpdateLoginData(string _code, string _message, string _url)
		{
			// Ensure UI updates are done on the UI thread
			dispatcherQueue.TryEnqueue(() =>
			{
				// Update UI elements here
				deviceCodeTextBlock1.Text = _code;
				deviceLoginMessageTextBlock1.Text = _message;
				hyperlinkButton1.NavigateUri = new Uri(_url);
			});
		}
	}
}
