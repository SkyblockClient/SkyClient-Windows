using SkyblockClient.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Config
{
	public class ConfigUtils
	{
		public static Dictionary<string, ModConfigWorkerCollection> ConfigWorkers
		{
			get
			{
				if (_configWorkers == null)
				{
					var dict = new Dictionary<string, ModConfigWorkerCollection>();

					var workerType = typeof(ModConfigWorkerBase);
					var workers = AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(s => s.GetTypes())
						.Where(p => workerType.IsAssignableFrom(p))
						.Where(p => p.Name != "ModConfigWorkerBase");


					foreach (var configWorker in workers)
					{
						var workerAttributes = configWorker.GetCustomAttributes(typeof(ModConfigWorkerAttribute), true);

						foreach (ModConfigWorkerAttribute workerAttribute in workerAttributes)
						{
							var workerCollection = new ModConfigWorkerCollection(configWorker, workerAttribute);
							dict.Add(workerAttribute.id, workerCollection);
						}
					}
					_configWorkers = dict;
				}
				return _configWorkers;
			}
		} private static Dictionary<string, ModConfigWorkerCollection> _configWorkers = null;

		public static async Task CreateModConfigWorker(ModOption mod)
		{
			if (ConfigWorkers.ContainsKey(mod.Id))
			{
				var workerCollection = ConfigWorkers[mod.Id];
				var worker = Activator.CreateInstance(workerCollection.configWorker) as ModConfigWorkerBase;
				worker.id = workerCollection.workerAttribute.id;
				worker.mod = mod;
				worker.helper = new ModConfigHelper(mod);
				await worker.Work();
			}
		}

		public class ModConfigWorkerCollection
		{
			public Type configWorker;
			public ModConfigWorkerAttribute workerAttribute;

			public ModConfigWorkerCollection(Type configWorker, ModConfigWorkerAttribute workerAttribute)
			{
				this.configWorker = configWorker;
				this.workerAttribute = workerAttribute;
			}
		}

		public static async Task StartWorker(string id)
		{
			if (Utils.AvailableModOptions.ContainsKey(id))
			{
				var mod = Utils.AvailableModOptions[id];
				await CreateModConfigWorker(mod);
			}
		}
	}
}
