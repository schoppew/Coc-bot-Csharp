using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Plugin;

namespace PluginContracts
{
    public class PluginExemple : BasePlugin
    {
		public override string Author { get { return "FastFrench"; } }
		public override string Version { get { return "1.0"; } }
		public override string Description { get { return "This is a very simple plugin for the CocBot. Actually it does nothing at all. "; } }
		public override string Name { get { return "Plugin Demo"; } }
		public override string Copyright { get { return "(C) 2015 FastFrench"; } }

		/// <summary>
		/// Initialize the plugin. If it returns false, then the plugin is disabled. 
		/// </summary>
		/// <returns></returns>
		public override bool Init(ICocBot theBotServiceProvider)
		{
			base.Init(theBotServiceProvider);
			theBotServiceProvider.WriteToOutput("Hello from the plugin");
			return true;
		}

    }
}
