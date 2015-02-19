using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Plugin;

namespace PluginContracts
{
    public class PluginExemple : ICocPlugin
    {
		public string Author { get { return "FastFrench"; } }
		public string Version { get { return "1.0"; } }
		public string Description { get { return "This is a very simple plugin for the CocBot. Actually it does nothing at all. "; } }
		public string Copyright { get { return "(C) 2015 FastFrench"; } }

		/// <summary>
		/// Initialize the plugin. If it returns false, then the plugin is disabled. 
		/// </summary>
		/// <returns></returns>
		public bool Init(ICocBot theBotServiceProvider)
		{
			theBotServiceProvider.WriteToOutput("Hello from the plugin");
			return true;
		}

    }
}
