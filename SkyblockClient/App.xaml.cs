using Newtonsoft.Json;
using SkyblockClient.Options;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SkyblockClient
{
	/// <summary>
	/// Interaktionslogik für "App.xaml"
	/// </summary>
	public partial class App : Application
	{
		public static string ResourceDirectory = $"pack://application:,,,/{Utils.AssemblyName};component/";

		protected override async void OnStartup(StartupEventArgs e)
		{
			base.OnStartup(e);
			await Initialize();
		}

		public async Task Initialize()
		{
			if (!Globals.isDebugEnabled)
			{
				Utils.Info("Thank you for using SkyClient", "This is the output Console and will display information important to the developer!");
			}

			var errorAt = "try";
			try
			{
				errorAt = "Utils.LoadSettings();";
				Utils.LoadSettings();
			}
			catch (Exception ex)
			{
				Utils.Error("UNKOWN FATAL ERROR INITIALIZING INSTALLER");
				Utils.Log(ex, "errorAt: " + errorAt, "unkown fatal error initizialing installer");
			}

			try
			{
				await Utils.ExecuteAsyncronous(
					DownloadModsFile(),
					DownloadResourceFile(),
					DownloadPreviewHtmlBase()
				);
			}
			catch (Exception e)
			{
				Utils.Error("ERROR CONNECTING TO GITHUB", "\tAre you using a proxy?", "\tYou might also be using an outdated version! Update!");
				Utils.Log(e, "error connecting to github");
				if (!Globals.isDebugEnabled)
				{
					Utils.OpenLinkInBrowser(Globals.GITHUB_RELEASES);
				}
			}

			Globals.InvokeContentLoaded(this);
			// doing this just to be sure 
			Globals.MainWindow?.Globals_OnContentLoaded(this, new EventArgs());
		}

		private async Task DownloadResourceFile()
		{
			string response = await Globals.DownloadFileStringAsync("packs.json");
			Globals.packOptions = JsonConvert.DeserializeObject<List<PackOption>>(response, Utils.JsonSerializerSettings);
		}

		private async Task DownloadModsFile()
		{
			string response = await Globals.DownloadFileStringAsync("mods.json");
			Globals.modOptions = JsonConvert.DeserializeObject<List<ModOption>>(response, Utils.JsonSerializerSettings);
		}

		private async Task DownloadPreviewHtmlBase()
		{
			string response = await Globals.DownloadFileStringAsync("PreviewBase.html");
			Globals.PreviewHtmlBase = response;
		}
	}
}
