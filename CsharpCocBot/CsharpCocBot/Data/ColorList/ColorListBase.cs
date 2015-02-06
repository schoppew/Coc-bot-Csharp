using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data.ColorList
{
	abstract public class ColorListBase
	{
		abstract public string GenericName { get; }
		
		abstract public ColorListItem[] Items { get; }

		public int NbItems
		{
			get
			{
				if (Items == null) return 0;
				return Items.Length;
			}
		}

		public ColorListItem GetItemByLevel(int level)
		{
			return Items.FirstOrDefault(it => it.Level == level);
		}

		public ColorListItem GetItemByName(string name)
		{
			ColorListItem item = Items.FirstOrDefault(it => it.Name == name); // First try to find exact match
			if (item != null) return item;
			string upperName = name.ToUpperInvariant();
			item = Items.FirstOrDefault(it => it.Name.ToUpperInvariant() == upperName); // Then, if fail, ignore case
			if (item != null) return item;
			item = Items.FirstOrDefault(it => it.Name.ToUpperInvariant().Contains(upperName)); // If still fail, then check names that include the given string
			return item;
		}
	}
}
