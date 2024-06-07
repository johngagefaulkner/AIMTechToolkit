using Microsoft.UI.Xaml.Printing;

namespace AIMTechToolkit.Services
{
	internal static class NavigationService
	{
		internal static NavigationView InstanceNavView { get; set; }
		internal static Frame InstanceContentFrame { get; set; }
		internal static void Navigate(Type pageType, string pageName)
		{
			InstanceContentFrame.Navigate(pageType, null, new CommonNavigationTransitionInfo());
			InstanceNavView.Header = pageName;
			Log.Info($"Navigated to: {pageName}");
		}
	}
}
