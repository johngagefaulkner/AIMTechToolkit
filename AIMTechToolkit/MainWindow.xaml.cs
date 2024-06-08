using Microsoft.Graph.Models;
using Microsoft.UI.Xaml.Controls;

namespace AIMTechToolkit
{
	public sealed partial class MainWindow : Window
	{
		//public static MainWindow Current { get; private set; }
		public static new MainWindow Current { get; private set; }

		public MainWindow()
		{
			this.InitializeComponent();
			this.Title = "AIM Tech Toolkit";
			this.ExtendsContentIntoTitleBar = true;
			this.SetTitleBar(AppTitleBar1);
			this.AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
			this.AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
			Current = this;
			Log.Info("MainWindow activated.");

			// Set the initial content
			NavigationService.InstanceNavView = navigationView1;
			NavigationService.InstanceContentFrame = AppContentFrame1;
			NavigationService.Navigate(typeof(HomePage), HomePage.PageHeader);
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
								if (!MicrosoftLoginService.IsUserLoggedIn)
								{ NavigationService.Navigate(typeof(Controls.LoginDialogContentPage), "Login"); }
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

		public void NotifyUser(string strMessage, string strTitle = "Information", InfoBarSeverity severity = InfoBarSeverity.Informational)
		{
			// If called from the UI thread, then update immediately.
			// Otherwise, schedule a task on the UI thread to perform the update.
			if (DispatcherQueue.HasThreadAccess)
			{
				UpdateStatus(strMessage, strTitle, severity);
			}
			else
			{
				DispatcherQueue.TryEnqueue(() =>
				{
					UpdateStatus(strMessage, strTitle, severity);
				});
			}
		}

		private void UpdateStatus(string strMessage, string strTitle, InfoBarSeverity severity)
		{
			AppInfoBar1.IsOpen = false;
			AppInfoBar1.Title =	strTitle;
			AppInfoBar1.Message = strMessage;
			AppInfoBar1.Severity = severity;
			AppInfoBar1.IsOpen = true;
		}
	}
}
