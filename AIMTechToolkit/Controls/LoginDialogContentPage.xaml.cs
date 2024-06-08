using Microsoft.UI.Dispatching;

namespace AIMTechToolkit.Controls
{
	public sealed partial class LoginDialogContentPage : Page
	{
		private DispatcherQueue dispatcherQueue;
		public static LoginDialogContentPage Current;

		public LoginDialogContentPage()
		{
			this.InitializeComponent();
			dispatcherQueue = DispatcherQueue.GetForCurrentThread();
			Current = this;

			//App.InstanceLoginPage = this;
			//CurrentlyDisplayedLoginPage = this;
		}

		private async void loginDialogContentPage1_Loaded(object sender, RoutedEventArgs e)
		{
			var _authResult = await Services.Auth.DeviceCodeFlow.GetATokenForGraph();
			if (_authResult is not null)
			{
				Log.Info("Login was successful!");
				MainWindow.Current.NotifyUser("Success", "You successfully logged into your Microsoft account!");
				NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);
				MicrosoftLoginService.SetAuthenticatedUser(_authResult);
				// await App.InstanceMainWindow.UpdateUserProfilePicture();
			}
		}

		internal void PopulateDeviceCodeLoginData(string _code, string _message, string _url)
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
