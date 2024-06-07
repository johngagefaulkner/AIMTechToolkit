using System.Collections.ObjectModel;

namespace AIMTechToolkit.Pages
{
	public sealed partial class DevicesPage : Page
	{
		public const string PageHeader = "Devices";
		private ObservableCollection<Person> BasicListViewItems;

		public DevicesPage()
		{
			this.InitializeComponent();

			List<Person> _list = new();
			_list.Add(new Person("John Doe", "NCR"));
			_list.Add(new Person("Gage Faulkner", "City of Atlanta"));
			_list.Add(new Person("Lateef Ashekun", "Fulton County"));
			_list.Add(new Person("DESKTOP-38473KF", "Laptop // Windows 11"));
			BasicListViewItems = new(_list);
			listView1.ItemsSource = BasicListViewItems;
			listView1.Visibility = Visibility.Visible;
		}
	}
}
