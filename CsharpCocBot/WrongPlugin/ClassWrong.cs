using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Plugin;
using PluginContracts;

namespace WrongPlugin
{
	abstract class ClassWrong : ICocPlugin
	{
		abstract public string Author { get; }
		abstract public string Version { get; }
		abstract public string Description { get; }
		abstract public string Name { get; }
		abstract public string Copyright { get; }		
		abstract public bool Init(ICocBot theBotServiceProvider);		
	}
}
