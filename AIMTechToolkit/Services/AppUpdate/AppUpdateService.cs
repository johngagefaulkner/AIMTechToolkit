using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Input;
using Microsoft.Windows.AppLifecycle;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using Windows.UI.Popups;

namespace AIMTechToolkit.Services.AppUpdate
{
	internal static class AppUpdateService
	{
		private const string UrlToHostedAppInstaller = "https://trial3.azurewebsites.net/HRApp/HRApp.appinstaller";
		private const string UrlToHostedMSIX = "https://trial3.azurewebsites.net/HRApp/HRApp.msix";
		private const string UrlToHostedVersionFile = "https://trial3.azurewebsites.net/HRApp/Version.txt";

		// Queue up the update and close the current app instance.
		private static async Task ApplyTheAppUpdate()
		{
			// Register the active instance of an application for restart in your Update method
			uint res = RelaunchHelper.RegisterApplicationRestart(null, RelaunchHelper.RestartFlags.NONE);

			PackageManager packagemanager = new PackageManager();
			await packagemanager.AddPackageAsync(new Uri(UrlToHostedMSIX), null, DeploymentOptions.ForceApplicationShutdown);
			Microsoft.Windows.AppLifecycle.AppInstance.Restart("update");
		}

		public static async Task<bool> CheckForAppInstallerUpdatesAndLaunchAsync(string targetPackageFullName, PackageVolume packageVolume)
		{
			// Get the current app's package for the current user.
			PackageManager pm = new PackageManager();
			Package package = pm.FindPackageForUser(string.Empty, targetPackageFullName);

			PackageUpdateAvailabilityResult result = await package.CheckUpdateAvailabilityAsync();
			switch (result.Availability)
			{
				case PackageUpdateAvailability.Available:
				case PackageUpdateAvailability.Required:
					Log.Info("Update available!");
					//Queue up the update and close the current instance
					await pm.AddPackageByAppInstallerFileAsync(
					new Uri(UrlToHostedAppInstaller),
					AddPackageByAppInstallerOptions.ForceTargetAppShutdown,
					packageVolume);
					break;
				case PackageUpdateAvailability.NoUpdates:
					// Close AppInstaller.
					//await ConsolidateAppInstallerView();
					Log.Info("No update available!");
					return false;
				case PackageUpdateAvailability.Unknown:
					Log.Info("Update availability unknown!");
					return false;
				default:
					// Log and ignore error.
					Log.Warning($"No update information associated with app {targetPackageFullName}");
					// Launch target app and close AppInstaller.
					//await ConsolidateAppInstallerView();
					return false;
			}

			return false;
		}

		//check for an update on my server
		private static async Task<bool> CheckForUpdatesWithoutAppInstallerAsync()
		{
			try
			{
				Log.Info("Checking for updates...");
				var _stream = await HttpService.InstanceHttpClient.GetStreamAsync(UrlToHostedVersionFile);
				var _reader = new StreamReader(_stream);
				var _latestVersion = new Version(await _reader.ReadToEndAsync());
				Log.Info($"Latest Version: {_latestVersion}");

				var _appPkg = Package.Current;
				PackageVersion _appPkgVersion = _appPkg.Id.Version;
				var _currentVersion = new Version(string.Format("{0}.{1}.{2}.{3}", _appPkgVersion.Major, _appPkgVersion.Minor, _appPkgVersion.Build, _appPkgVersion.Revision));
				Log.Info($"Current Version: {_currentVersion}");

				// Compare package versions
				if (_latestVersion.CompareTo(_currentVersion) > 0)
				{
					Log.Info("Update available!");
					return true;
				}
				else
				{
					Log.Info("Latest version is already installed.");
					return false;
				}
			}

			catch (Exception ex)
			{
				Log.Error(ex, "Failed to check for app updates!");
				return false;
			}
		}
	}
}
