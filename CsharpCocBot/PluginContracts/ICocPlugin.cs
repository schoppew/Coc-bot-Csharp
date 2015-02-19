using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginContracts;

namespace CoC.Bot.Plugin
{
// All plugins designed for this bot must support this Interface
	public interface ICocPlugin
	{
		string Author { get; }
		string Version { get; }
		string Description { get; }
		string Copyright { get; }

		/// <summary>
		/// Initialize the plugin. If it returns false, then the plugin is disabled. 
		/// </summary>
		/// <returns></returns>
		bool Init(ICocBot cocServicesProvider);

	}
}
