using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data;

// The namespace is just CoC.Bot.Data in order to simplify usage of them
namespace CoC.Bot.Data
{
	/// <summary>
	/// Base class for all building
	/// Contents: 
	///		Name
	///		ColorList to locate it in the village
	///		Position
	/// </summary>
	public class Building
	{
		public string Name { get; set; }
		public ClickablePoint Location { get; set; }
		public ColorSet Colors { get; set; }

	}
}
