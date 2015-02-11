using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.ViewModels;

namespace CoC.Bot.Data.Type
{
	public class DebugDataModel : ViewModelBase
	{
		string _description;
		public string Description
		{
			get { return _description; }
			set
			{
				if (_description != value)
				{
					_description = value;
					OnPropertyChanged();
				}
			}
		}

		private int _count;
		public int Count
		{
			get { return _count; }
			set
			{
				if (_count != value)
				{
					_count = value;
					OnPropertyChanged();
				}
			}
		}
	}
}
