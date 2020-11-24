using Microsoft.WindowsAPICodePack.Dialogs;
using System;
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

			txtCustomGameLocation.Text = Globals.minecraftRootLocation;
		}

		private void TxtGameDirectoryTextChanged(object sender, TextChangedEventArgs e)
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
			string path = string.Empty;

			if (Directory.Exists(Globals.skyblockModsLocation))
				path = Globals.skyblockModsLocation;
			else if (Directory.Exists(Globals.skyblockRootLocation))
				path = Globals.skyblockRootLocation;
			else
				path = Globals.minecraftRootLocation;

			path = path.Replace("/", "\\");

			Process explorer = new Process
			{
				StartInfo = Utils.CreateProcessStartInfo("explorer.exe", $"\"{path}\"")
			};
			explorer.Start();
			explorer.WaitForExit();
		}

		private void BtnSelectCustomGameLocation(object sender, RoutedEventArgs e)
		{
			CommonOpenFileDialog dialog = new CommonOpenFileDialog();

			string initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			if (Directory.Exists(Path.Combine(initialDirectory, ".minecraft")))
			{
				initialDirectory = Path.Combine(initialDirectory, ".minecraft");
			}

			dialog.InitialDirectory = initialDirectory;
			dialog.IsFolderPicker = true;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				bool valid = Utils.ValidateMinecraftDirectory(dialog.FileName);
				if (!valid)
				{
					string msg = $"\"{dialog.FileName}\" is not a valid minecraft directory.\nMake sure you run the minecraft launcher at least once.";
					MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
				}
				else
				{
					Globals.minecraftRootLocation = dialog.FileName;
				}
			}
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			mainWindow.frmAdvancedSettingsIsOpen = false;
		}
	}
}
