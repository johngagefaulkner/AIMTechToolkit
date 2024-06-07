namespace AIMTechToolkit.Controls
{
	public sealed partial class ContentDialogContent : Page
	{
		/// <summary>
		/// Displays a pop-up ContentDialog with a message.
		/// </summary>
		/// <param name="DialogMessage"><see langword="string"/>Message displayed to the user in the pop-up dialog.</param>
		public ContentDialogContent(string DialogMessage = "")
		{
			this.InitializeComponent();
			textBlock1.Text = DialogMessage;
		}
	}
}
