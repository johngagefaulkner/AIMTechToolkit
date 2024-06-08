using System.Collections.Concurrent;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace AIMTechToolkit
{
	public partial class App : Application
	{
		public App()
		{
			this.InitializeComponent();
			Log.Initialize();
			Log.Info("App initialized.");
		}

		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			m_window = new MainWindow();
			m_window.Activate();
		}

		private static Window m_window;
		public static Window InstanceWindow => m_window;
		public static AppWindow InstanceAppWindow => m_window.AppWindow;
		public static MainWindow InstanceMainWindow => m_window as MainWindow;
	}
}
