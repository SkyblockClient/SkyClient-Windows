using SkyblockClient.Option;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkyblockClient.Config
{
	public class ConfigUtils
	{
		/*
		public static List<Type> ConfigWorkers
		{
			get
			{
				if (_configWorkers == null)
				{
					Dictionary<string, Type> dict = new Dictionary<string, Type>();
					var workerType = typeof(ModConfigWorkerBase);
					var workers = AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(s => s.GetTypes())
						.Where(p => workerType.IsAssignableFrom(p))
						.Where(p => p.Name != "ModConfigWorkerBase");

					_configWorkers = workers.ToList();
				}
				return _configWorkers;
			}
		}
		private static List<Type> _configWorkers = null;
		*/

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
						.Where(p => p.Name != "ModConfigWorkerBase" );


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
		}
		private static Dictionary<string, ModConfigWorkerCollection> _configWorkers = null;


		public static async Task CreateModConfigWorker(ModOption mod)
		{
			if (ConfigWorkers.ContainsKey(mod.id))
			{
				var workerCollection = ConfigWorkers[mod.id];
				var worker = Activator.CreateInstance(workerCollection.configWorker) as ModConfigWorkerBase;
				worker.id = workerCollection.workerAttribute.id;
				worker.mod = mod;
				worker.helper = new ModConfigHelper(mod);
				await worker.Work();
			}

			/*
			List<Task> tasks = new List<Task>();
			bool workerFound = false;

			foreach (var configWorker in ConfigWorkers)
			{
				var workers = configWorker.GetCustomAttributes(typeof(ModConfigWorkerAttribute), true);

				foreach (ModConfigWorkerAttribute workerAttribute in workers)
				{
					if (workerAttribute.id == mod.id)
					{
						var worker = Activator.CreateInstance(configWorker) as ModConfigWorkerBase;
						worker.id = workerAttribute.id;
						worker.mod = mod;
						worker.helper = new ModConfigHelper(mod);
						try
						{
							tasks.Add(worker.Work(mod));
						}
						catch (Exception e)
						{
							Utils.Error(e.Message, e.StackTrace);
						}
						workerFound = true;
						break;
					}
				}
				if (workerFound)
				{
					break;
				}
			}
			Task.WaitAll(tasks.ToArray());
			*/
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
	}


}
