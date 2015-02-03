using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorOptimizer
{
	public class FileData
	{
		FileData(string fileName)
		{
			FileName = fileName;
			using (ExtBitmap bitmap = new ExtBitmap())
			{
			if (!bitmap.LoadFromFile(fileName))
				OK = false;
			else
				{			
					ownStats = bitmap.CountColors();
					OK = true;
				}
			}
		}
		public bool OK {get; private set;}
		public string FileName {get; set;}
		public Dictionary<int, int> ownStats;
	}
	public class DataPerCategory
	{
		public DataPerCategory()
		{
			FileNames = new List<string>();
			SelectedColorsSoFar = new Dictionary<int,int>();
		}
		public string Label { get; set; }
		public string Comment { get; set; }
		List<string> FileNames { get; set; }
		Dictionary<int, int> SelectedColorsSoFar;
		private bool ProcessFirstGoodFile(string fileName)
		{
			return false;
		}
		private bool ProcessNextGoodFile(string fileName)
		{
			return false;
		}
		public bool ProcessGoodFiles()
		{
			return false;
		}
		public bool ProcessBadFile(string fileName)
		{
			return false;
		}
	}

	public class Moulinette
	{		
	}
}
