using System.Diagnostics;
using System.IO;
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
			txtGameDirectory.Text = Globals.gameDirectory;
		}

		private void TxtGameDirecotryTextChanged(object sender, TextChangedEventArgs e)
		{
			var txt = (TextBox)sender;
			var text = txt.Text;
			if (!text.EndsWith("/"))
			{
				text += "/";
			}
			Globals.gameDirectory = text;
		}

		private void BtnOpenModsFolderClick(object sender, RoutedEventArgs e)
		{
			Process explorer = new Process
			{
				StartInfo = Utils.CreateProcessStartInfo("explorer.exe", $"{Globals.skyblockModsLocation}")
			};
			explorer.Start();
			explorer.WaitForExit();
		}

	}
}
