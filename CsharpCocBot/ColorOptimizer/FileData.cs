using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorOptimizer
{
	public class FileData
	{
		public FileData(string fileName)
		{
			FileName = fileName;
			using (ExtBitmap bitmap = new ExtBitmap())
			{
				if (!bitmap.LoadFromFile(fileName))
					OK = false;
				else
				{
					ownStats = bitmap.CountColors();
					OK = ownStats.Count>0;
				}
			}
		}
		public bool OK { get; private set; }
		public string FileName { get; set; }
		public Dictionary<int, int> ownStats;
	}
}
