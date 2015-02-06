using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data.ColorList
{
	public class ColorListItem
	{
		public ColorListItem()
		{

		}

		public ColorListItem(string name, int[] colors, int minPixelCount)
		{
			Name = name;
			Colors = colors;

			// Extract number from the name. If any, then set this as the level. Otherwise Level will be 0. 
			string number = string.Join("", name.ToCharArray().Where(Char.IsDigit));
			int lv = 0;
			if (!string.IsNullOrWhiteSpace(number))
				int.TryParse(number, out lv);
			Level = lv;

			if (Colors == null)
				ColorCount = 0;
			else
				ColorCount = Colors.Length;

			MinPixelCount = minPixelCount;
		}

		public string Name { get; private set; }
		public int[] Colors { get; private set; }
		public int Level { get; private set; }
		public int ColorCount { get; private set; }
		public int MinPixelCount {get; private set;}		
	}
}
