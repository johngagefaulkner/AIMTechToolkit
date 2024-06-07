using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMTechToolkit.Services
{
	internal static class UserSettingsService
	{
		private const string AppThemeKey = "AppTheme";
		private const string SystemBackdropKey = "SystemBackdrop";
		private const string PaneDisplayModeKey = "PaneDisplayMode";

		public static string UserProfile { get; set; } = Environment.ExpandEnvironmentVariables("%USERPROFILE%");
		public static ElementTheme AppTheme { get; set; } = ElementTheme.Default;
		public static SystemBackdrop AppSystemBackdrop { get; set; } = new MicaBackdrop();
		public static NavigationViewPaneDisplayMode AppPaneDisplayMode { get; set; } = NavigationViewPaneDisplayMode.Left;

		public static void SetAppThemeFromIndex(int themeIndex) => AppTheme = (ElementTheme)themeIndex;

		public static void SetAppPaneDisplayModeFromIndex(int paneDisplayModeIndex)
		{
			AppPaneDisplayMode = (NavigationViewPaneDisplayMode)paneDisplayModeIndex;
		}

		public static void SetAppSystemBackdropFromName(string systemBackdropName)
		{
			switch (systemBackdropName)
			{
				case "Mica":
					AppSystemBackdrop = new MicaBackdrop();
					break;
				case "Acrylic":
					AppSystemBackdrop = new DesktopAcrylicBackdrop();
					break;
				case "Mica (Alt)":
					AppSystemBackdrop = new MicaBackdrop() { Kind = Microsoft.UI.Composition.SystemBackdrops.MicaKind.BaseAlt };
					break;
				default:
					AppSystemBackdrop = new MicaBackdrop();
					break;
			}
		}

		private static void LoadAppTheme()
		{
			if (!SettingsManagerService.ContainsKey(AppThemeKey))
			{
				SettingsManagerService.Set(AppThemeKey, (int)ElementTheme.Default);
				SetAppThemeFromIndex((int)ElementTheme.Default);
			}

			else
			{
				SetAppThemeFromIndex(SettingsManagerService.Get<int>(AppThemeKey));
			}
		}

		private static void LoadSystemBackdrop()
		{
			if (!SettingsManagerService.ContainsKey(SystemBackdropKey))
			{
				SettingsManagerService.Set(SystemBackdropKey, "Mica");
				SetAppSystemBackdropFromName("Mica");
			}

			else
			{
				SetAppSystemBackdropFromName(SettingsManagerService.Get<string>(SystemBackdropKey));
			}
		}

		private static void LoadPaneDisplayMode()
		{
			if (!SettingsManagerService.ContainsKey(PaneDisplayModeKey))
			{
				SettingsManagerService.Set(PaneDisplayModeKey, (int)NavigationViewPaneDisplayMode.Left);
				SetAppPaneDisplayModeFromIndex((int)NavigationViewPaneDisplayMode.Left);
			}

			else
			{
				SetAppPaneDisplayModeFromIndex(SettingsManagerService.Get<int>(PaneDisplayModeKey));
			}
		}

		public static void Load()
		{
			LoadAppTheme();
			LoadSystemBackdrop();
			LoadPaneDisplayMode();
		}

		public static void UpdateSetting(string key, object value) => SettingsManagerService.Set(key, value);
	}
}
