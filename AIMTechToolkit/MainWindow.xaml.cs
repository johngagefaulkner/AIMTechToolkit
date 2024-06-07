using System.Linq.Expressions;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.System;

namespace AIMTechToolkit
{
	public sealed partial class MainWindow : Window
	{
		internal static MainWindow InstanceMainWindow { get; private set; }

		public MainWindow()
		{
			this.InitializeComponent();
			this.Title = "AIM Tech Toolkit";
			this.ExtendsContentIntoTitleBar = true;
			this.SetTitleBar(AppTitleBar1);
			this.AppWindow.SetIcon("ms-appx:///Assets/AIMTechToolkit.ico");
			this.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
			this.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

			InstanceMainWindow = this;
			Log.Info("MainWindow activated.");
			NavigationService.InstanceNavView = navigationView1;
			NavigationService.InstanceContentFrame = AppContentFrame1;

			// Set the initial content
			NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);

			// Handle User Login Button
			AppTitleBarLoginBtn1.Margin = new Thickness(0, 0, this.AppWindow.TitleBar.RightInset, 0);
		}

		private async void navigationView1_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			try
			{
				if (args.IsSettingsInvoked)
				{
					NavigationService.Navigate(typeof(SettingsPage), SettingsPage.PageHeader);
				}

				else
				{
					NavigationViewItem item = args.InvokedItemContainer as NavigationViewItem;
					string pageName = item.Tag.ToString();
					if (!string.IsNullOrEmpty(pageName))
					{
						switch (pageName)
						{
							case "Home":
								NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);
								break;
							case "Devices":
								NavigationService.Navigate(typeof(DevicesPage), DevicesPage.PageHeader);
								break;
							case "Login":
								//NavigationViewLoginButtonClicked();
								//InitiateLoginWithDeviceCode();
								//await HandleDeviceCodeFlow(); // WORKS!!
								NavigationService.Navigate(typeof(Controls.LoginDialogContentPage), "Login");
								break;
							default:
								break;
						}
					}
				}
			}

			catch (Exception ex)
			{
				var errorMessage = 
					ex.Message + 
					Environment.NewLine + Environment.NewLine + 
					ex.StackTrace +
					Environment.NewLine + Environment.NewLine +
					"See error log for more details.";

				await DialogService.ShowDialogAsync("Error", errorMessage);
				return;
			}
		}

		private async void AppTitleBarLoginBtn1_Click(object sender, RoutedEventArgs e)
		{
			if (AppTitleBarLoginBtn1.Content is string strContent)
			{
				if (strContent == "Login")
				{
					await DialogService.ShowDialogAsync("Login", "Please login to your Microsoft 365 account.");
				}
			}

			if (AppTitleBarLoginBtn1.Content is PersonPicture userProfilePicture)
			{
				await DialogService.ShowDialogAsync("User Profile", "Please see your account details!");
			}
		}

		internal void ShowNotification(string title, string message, InfoBarSeverity severity = InfoBarSeverity.Informational)
		{
			AppTitleBarInfoBar1.IsOpen = false;

			AppTitleBarInfoBar1.Severity = severity;
			AppTitleBarInfoBar1.Title = title;
			AppTitleBarInfoBar1.Message = message;

			AppTitleBarInfoBar1.IsOpen = true;
		}

		internal void SetTitleBarButtonContent(string content)
		{
			AppTitleBarLoginBtn1.Content = content;
		}

		internal async Task UpdateUserProfilePicture()
		{
			if (MicrosoftLoginService.IsUserLoggedIn)
			{
				try
				{
					var loggedInUserId = MicrosoftLoginService.GetAuthenticatedUser().UniqueId;
					accountNavItem1.Content = loggedInUserId;

					var stream = await MicrosoftGraphAPIService.GetUserPhotoAsStreamAsync(loggedInUserId);
					var bitmapImage = new BitmapImage();
					bitmapImage.SetSource(stream);

					accountNavItemIcon1.Source = bitmapImage;

					var userProfilePicture = new PersonPicture
					{
						Initials = MicrosoftLoginService.GetAuthenticatedUser().Account.Username,
						DisplayName = MicrosoftLoginService.GetAuthenticatedUser().Account.Username,
						ProfilePicture = bitmapImage
					};

					AppTitleBarLoginBtn1.Content = userProfilePicture;
				}

				catch (Exception ex)
				{
					Log.Error(ex, "Failed to update user's profile picture!");
					return;
				}
			}
		}
	}
}
