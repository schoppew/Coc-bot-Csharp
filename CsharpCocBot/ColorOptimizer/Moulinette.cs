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
			GoodSnaps = new List<FileData>();
			BadSnaps = new List<FileData>();
			SelectedColorsSoFar = new Dictionary<int,int>();
		}
		public string Label { get; set; }
		public string Comment { get; set; }
		List<FileData> GoodSnaps { get; set; }
		List<FileData> BadSnaps { get; set; }
		Dictionary<int, int> SelectedColorsSoFar;
		private bool ProcessFirstGoodFile(FileData data)
		{
			SelectedColorsSoFar = new Dictionary<int, int>(data.ownStats);
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessNextGoodFile(FileData data)
		{
			foreach (int color in data.ownStats.Keys)
				if (SelectedColorsSoFar.ContainsKey(color))
					SelectedColorsSoFar[color] += data.ownStats[color];
				else
					SelectedColorsSoFar.Remove(color);
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessGoodFiles()
		{
			foreach (FileData snap in GoodSnaps)
				if (SelectedColorsSoFar.Count == 0)
				{
					if (!ProcessFirstGoodFile(snap)) return false;
				}
				else
					if (!ProcessNextGoodFile(snap)) return false;
			return SelectedColorsSoFar.Count>0;
		}
		private bool ProcessBadFile(FileData data)
		{
			foreach (int color in data.ownStats.Keys)
				if (SelectedColorsSoFar.ContainsKey(color))
					SelectedColorsSoFar.Remove(color);					
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessBadFiles()
		{
			foreach (FileData snap in BadSnaps)
				if (!ProcessBadFile(snap)) return false;
			return SelectedColorsSoFar.Count > 0;
		}
		private bool SaveResultInFile()
		{
			return false;
		}
		public bool Process()
		{
			if (ProcessGoodFiles() && ProcessBadFiles())
			{
				if (!SaveResultInFile()) return false;
				return true;
			}
			return false;
		}
	}

	public class Moulinette
	{		
	}
}
