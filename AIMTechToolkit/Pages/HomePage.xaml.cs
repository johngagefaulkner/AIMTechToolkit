namespace AIMTechToolkit.Pages
{
	public sealed partial class HomePage : Page
	{
		public const string PageHeader = "Home";

		public HomePage()
		{
			this.InitializeComponent();
		}

		private void testBtn1_Click(object sender, RoutedEventArgs e)
		{
			localAppDataTextBlock1.Text = Services.AppDataService.GetLocalAppDataPath();
			roamingAppDataTextBlock1.Text = Services.AppDataService.GetRoamingAppDataPath();
		}
	}
}
