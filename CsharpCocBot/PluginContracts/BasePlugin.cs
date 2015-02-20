using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Plugin;

namespace PluginContracts
{

	/// <summary>
	/// Derive your own plugin to easily implement the mandatory interface
	/// </summary>
	public abstract class BasePlugin : ICocPlugin
	{
		protected ICocBot MainBot { get; set; }
		abstract public string Author { get; }
		virtual public string Version { get { return "1.0"; } }
		abstract public string Name { get; }
		virtual public string Description { get { return ""; } }
		virtual public string Copyright { get { return "(C) 2015 FastFrench"; } }

		/// <summary>
		/// Initialize the plugin. If it returns false, then the plugin is disabled. 
		/// </summary>
		/// <returns></returns>
		virtual public bool Init(ICocBot theBotServiceProvider)
		{
			MainBot = theBotServiceProvider;
			theBotServiceProvider.WriteToOutput("Hello from BasePlugin");
			return true;
		}

	}
}
