using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CoC.Bot.Plugin;
using CoC.Bot.Properties;
using CoC.Bot.ViewModels;

namespace CoC.Bot.PluginMngr
{
	public class PluginLoader
	{
		public ICollection<ICocPlugin> Plugins { get; private set; }

		public PluginLoader(MainViewModel vm)
		{
			string path = Settings.Default.PluginsPath;

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string[] dllFileNames = null;
			if (Directory.Exists(path))
				dllFileNames = Directory.GetFiles(path, "*.dll");

			Plugins = new List<ICocPlugin>();
			foreach (string dllFile in dllFileNames)
			{
				vm.WriteToOutput(string.Format("Loading {0}", dllFile));
				AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
				Assembly assembly = Assembly.Load(an);
				if (assembly != null)
				{
					bool found = false;

					foreach (Type type in assembly.GetTypes().Where(type => !type.IsAbstract && !type.IsInterface))
					{
						ICocPlugin plugin = Activator.CreateInstance(type) as ICocPlugin;
						if (plugin != null)
						{
							found = true;
							if (plugin.Init(new SimpleServicesProvider(this, vm)))
							{
								vm.WriteToOutput(string.Format("{0} successfuly initialized", type.Name), GlobalVariables.OutputStates.Information);
								Plugins.Add(plugin);
								vm.WriteToOutput(string.Format("Written by {0}, {1}.\r\nVersion {2}.", plugin.Author, plugin.Copyright, plugin.Version), GlobalVariables.OutputStates.Information);
								vm.WriteToOutput(plugin.Description, GlobalVariables.OutputStates.Information);
							}
							else
								vm.WriteToOutput(string.Format("In assembly {0}, {1} failed to intialize, so it will be ignored.", assembly.GetAssemblyName(), type.Name), GlobalVariables.OutputStates.Warning);
						}
					}

					if (!found)
						vm.WriteToOutput(string.Format("Assembly {0} doesn't implement the proper interface", assembly.GetAssemblyName()), GlobalVariables.OutputStates.Warning);

				}
			}

			if (Plugins.Count == 0)
				vm.WriteToOutput("No plugin active.", GlobalVariables.OutputStates.Information);
			else
				vm.WriteToOutput(string.Format("{0} plugin(s) active.", Plugins.Count), GlobalVariables.OutputStates.Information);
		}
	}
}
