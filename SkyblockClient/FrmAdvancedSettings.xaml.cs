using System.Windows;
using System.Windows.Controls;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für AdvancedSettings.xaml
	/// </summary>
	public partial class FrmAdvancedSettings : Window
	{
		MainWindow mainWindow;
		public FrmAdvancedSettings(MainWindow mainWindow)
		{
			this.mainWindow = mainWindow;
			InitializeComponent();
		}

		private void TxtGameDirecotryTextChanged(object sender, TextChangedEventArgs e)
		{
			var txt = (TextBox)sender;
			mainWindow.gameDirectory = txt.Text;
		}
	}
}
