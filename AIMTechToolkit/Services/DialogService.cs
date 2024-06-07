using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMTechToolkit.Services
{
	internal static class DialogService
	{
		private static XamlRoot InstanceXamlRoot = NavigationService.InstanceContentFrame.XamlRoot;

		public static async Task ShowDialogAsync(string DialogTitle, string DialogContent)
		{
			ContentDialog dialog = new ContentDialog
			{
				Title = DialogTitle,
				Content = new Controls.ContentDialogContent(DialogContent),
				PrimaryButtonText = "OK",
				IsPrimaryButtonEnabled = true,
				Padding = new Thickness(20),
				XamlRoot = NavigationService.InstanceContentFrame.XamlRoot
			};

			await dialog.ShowAsync();
		}
	}
}
