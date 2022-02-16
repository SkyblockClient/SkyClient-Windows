using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using SkyblockClient.Options;
using SkyblockClient.Persistence;
using System.IO;
using System.Runtime.InteropServices;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

		[DllImport("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImport("user32.dll")]
		public static extern bool ReleaseCapture();

		IntPtr Handle
		{
			get
			{
				if (_handle == IntPtr.Zero)
				{
					_handle = new System.Windows.Interop.WindowInteropHelper(this).Handle;
				}
				return _handle;
			}
		}
		IntPtr _handle = IntPtr.Zero;

		public OptionPreview ModDocument
		{
			get => this.mdModDocument.Document;
			set
			{
				this.mdModDocument.Document = value;
				this.mdModDocument.Visibility = Utils.IsPropSet(value.Content) ? Visibility.Visible : Visibility.Collapsed;
			}
		}
		public OptionPreview PackDocument
		{
			get => this.mdPackDocument.Document;
			set
			{
				this.mdPackDocument.Document = value;
				this.mdPackDocument.Visibility = Utils.IsPropSet(value.Content) ? Visibility.Visible : Visibility.Collapsed;
			}
		}


		public MainWindow()
		{
			Globals.OnContentLoaded += Globals_OnContentLoaded;
			Globals.MainWindow = this;

			var errorAt = "try";
			try
			{
				errorAt = "InitializeComponent();";
				InitializeComponent();
			}
			catch (Exception e)
			{
				Utils.Error("UNKOWN FATAL ERROR INITIALIZING INSTALLER");
				Utils.Log(e, "errorAt: " + errorAt, "unkown fatal error initizialing installer");
			}
		}

		private bool _loaded = false;
		public void Globals_OnContentLoaded(object sender, EventArgs e)
		{
			if (!_loaded)
			{
				_loaded = true;
				RefreshContentList(Globals.packOptions, tabPacks);
				RefreshContentList(Globals.modOptions, tabMods);
			}
		}

		public void RefreshContentList<OptionType>(List<OptionType> list, TabControlOptions tabControl) where OptionType : Option
		{
			var newlist = new List<CheckBoxMod>();
			foreach (OptionType option in list)
			{
				if (!option.Hidden)
				{
					newlist.Add(option.CheckBox);
				}
			}

			tabControl.ItemSource = newlist;
		}

		public void RefreshPacks()
        {
			var list = new List<CheckBoxMod>();
			foreach (PackOption pack in Globals.packOptions)
			{
				if (!pack.Hidden)
				{
					list.Add(pack.CheckBox);
				}
			}

			tabPacks.ItemSource = list;
		}

		public void RefreshMods()
		{
			var list = new List<CheckBoxMod>();
			foreach (ModOption mod in Globals.modOptions)
			{
				if (!mod.Hidden)
				{
					list.Add(mod.CheckBox);
				}
			}
			tabMods.ItemSource = list;
		}

		private void ButtonsEnabled(bool enabled)
		{
			//TODO: add those buttons
			/*
			btnUpdate.IsEnabled = enabled;
			btnInstall.IsEnabled = enabled;
			*/
		}

		private async Task InitializeInstall()
		{
			await Utils.InitializeInstall();
		}

		private async Task InstallForge()
		{
			await Forge.ForgeInstaller.Work();
		}

		private void NotifyCompleted(string message)
		{
			Thread thread = new Thread(NotifyCompletedInternal);
			thread.Start(message);
		}

		private void NotifyCompletedInternal(object obj)
		{
			MessageBox.Show((string)obj, "Completed", MessageBoxButton.OK, MessageBoxImage.Information);
		}

		public bool frmAdvancedSettingsIsOpen = false;
		public FrmAdvancedSettings frmAdvancedSettings = null;
		private void OpenAdvancedSettings()
		{
			if (!frmAdvancedSettingsIsOpen)
			{
				frmAdvancedSettings = new FrmAdvancedSettings(this);
				frmAdvancedSettings.Show();
				frmAdvancedSettingsIsOpen = true;
			}
			else
			{
				if (frmAdvancedSettings != null)
				{
					frmAdvancedSettings.Show();
					frmAdvancedSettingsIsOpen = true;
				}
			}
		}

		public async Task Persist()
		{
			await PersistenceMain.Update();
		}

		private async Task Startupdate()
		{
			await UpdateMain.Update();
		}

		public async Task StartInstaller()
		{
			var crashedAt = string.Empty;
			try
			{
				if (File.Exists(Globals.skyblockPersistenceLocation))
				{
					var text = "SkyClient is already installed.\nInstalling it again will wipe your Mods and Packs folder, and reset Mod configs\n\nInstall anyway?";
					if (Globals.ShowInfo(text, "Warning", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) != MessageBoxResult.Yes)
						return;
				}

				crashedAt = "await InitializeInstall();";
				await InitializeInstall();
				crashedAt = "await Persist();";
				await Persist();

				crashedAt = "await Utils.ExecuteAsyncronous(";
				await Utils.ExecuteAsyncronous(
					InstallForge(),
					InstallMods(),
					InstallPacks()
				);
				crashedAt = "NotifyCompleted";
				NotifyCompleted("The installation is done!\nNow all that's left is to start the the minecraft launcher and pick the SkyClient profile");

			}
			catch (Exception e)
			{
				Utils.Error(e.Message, "An exception occured during the Installation, the program might not have installed correctly, please submit the log file to the author");
				Utils.Log(e, "crashAt:"+ crashedAt, "Exception occured at MainWindow.StartInstaller()");
			}
		}

		public async Task InstallPacks()
		{
			await PersistenceMain.InstallPacks(Globals.neededPacks);
		}

		public async Task InstallMods()
		{
			await PersistenceMain.InstallMods(Globals.neededMods);
		}

		public MessageBoxResult ShowInfo(string text, string title, MessageBoxButton messageBoxButton, MessageBoxImage messageBoxImage)
		{
			return MessageBox.Show(text, title, messageBoxButton, messageBoxImage);
		}
	}
}
