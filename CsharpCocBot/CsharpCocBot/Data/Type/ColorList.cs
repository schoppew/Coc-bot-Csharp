using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// The namespace is just CoC.Bot.Data in order to simplify usage of them
namespace CoC.Bot.Data

{
	public class ColorSet : List<Color>
	{
		public ColorSet()
		{

		}

		public ColorSet(int[] from)
		{
			foreach (int i in from)
				Add(Color.FromArgb(i));
		}

		void Add(int color)
		{
			Add(Color.FromArgb(color));
		}
	}
}
