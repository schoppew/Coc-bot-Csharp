using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Properties;
using CoC.Bot.Plugin;
using PluginContracts;
using System.Drawing;
using CoC.Bot.Tools;
using CoC.Bot.BotEngine;
using CoC.Bot.ViewModels;

namespace CoC.Bot.PluginMngr
{
	public class PluginLoader
	{
		public ICollection<ICocPlugin> Plugins { get; private set; }

		public PluginLoader(MainViewModel vm)
		{
			string path = Settings.Default.PluginsPath;
			string[] dllFileNames = null; 
			if(!Directory.Exists(path)) 
				Directory.CreateDirectory(path);
			if(Directory.Exists(path)) 
			{ 
				dllFileNames = Directory.GetFiles(path, "*.dll"); 
			}
			ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
			foreach (string dllFile in dllFileNames)
			{
				vm.WriteToOutput(string.Format("Loading {0}", dllFile));
				AssemblyName an = System.Reflection.AssemblyName.GetAssemblyName(dllFile);
				Assembly assembly = Assembly.Load(an);
				assemblies.Add(assembly);
			}

			// Now we have loaded all assemblies from our predefined location, we can search for all types that implement our Interface IPlugin.
			Type pluginType = typeof(CoC.Bot.Plugin.ICocPlugin);
			ICollection<Type> pluginTypes = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly != null)
				{
					Type[] types = assembly.GetTypes();
					bool found = false;
					foreach (Type type in types)
					{
						if (type.IsInterface || type.IsAbstract)
						{
							continue;
						}
						else
						{
							if (type.GetInterface(pluginType.FullName) != null)
							{
								pluginTypes.Add(type);
								found = true;
							}
						}
					}
					if (!found)
						vm.WriteToOutput(string.Format("Assembly {0} doesn't implement the proper interface", assembly.GetName()), GlobalVariables.OutputStates.Information);				

				}
			}

			Plugins = new List<ICocPlugin>(pluginTypes.Count);
			foreach (Type type in pluginTypes)
			{
				ICocPlugin plugin = (ICocPlugin)Activator.CreateInstance(type);
				if (plugin.Init(new SimpleServicesProvider(this, vm)))
				{
					vm.WriteToOutput(string.Format("{0} successfuly initialized", plugin.GetType().Name), GlobalVariables.OutputStates.Information);
					Plugins.Add(plugin);
					vm.WriteToOutput(string.Format("Written by {0}, {1}.\r\nVersion {2}.", plugin.Author, plugin.Copyright, plugin.Version), GlobalVariables.OutputStates.Information);
					vm.WriteToOutput(string.Format("{0}", plugin.Description), GlobalVariables.OutputStates.Information);					
					
				}
				else
				{
					Main.Bot.WriteToOutput(string.Format("{0} failed to intialize, so it will be ignored.", plugin.GetType().Name), GlobalVariables.OutputStates.Information);
					
				}
			}
			if (Plugins.Count == 0)
				vm.WriteToOutput(string.Format("No plugin active."), GlobalVariables.OutputStates.Information);
			else
				vm.WriteToOutput(string.Format("{0} plugin(s) active.", Plugins.Count), GlobalVariables.OutputStates.Information);

			//return Plugins.Count() > 0;
		}
		
	}
}
