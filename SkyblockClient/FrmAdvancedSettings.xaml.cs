using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
