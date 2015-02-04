using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorOptimizer
{
	public class DataPerCategory
	{
		public DataPerCategory(string name, string[] files)
		{
			Label = name;
			GoodSnaps = new List<FileData>();
			BadSnaps = new List<FileData>();
			foreach (string file in files)
			{
				FileData fileData = new FileData(file);
				if (fileData.OK) // Skip corrected or empty files
					GoodSnaps.Add(fileData);
			}
			SelectedColorsSoFar = new Dictionary<int, int>();
			ColorsLeftAfterFirstGood = ColorsLeftAfterLastGood = ColorsLeftAfterAllBad = 0;
		}

		public void AddBadCategory(DataPerCategory badCategory)
		{
			if (badCategory == this) return; // not bad, it's me!
			foreach (FileData file in badCategory.GoodSnaps) // If they are good for it, they are bad for me
				BadSnaps.Add(file);
		}

		public string Label { get; set; }
		public string Comment { get; set; }
		public int ColorsLeftAfterFirstGood { get; private set; }
		public int ColorsLeftAfterLastGood { get; private set; }
		public int ColorsLeftAfterAllBad { get; private set; }
		public List<FileData> GoodSnaps { get; set; }
		public List<FileData> BadSnaps { get; set; }
		public Dictionary<int, int> SelectedColorsSoFar { get; set; }
		public string KilledBy { get; private set; }
		private bool ProcessFirstGoodFile(FileData data)
		{
			SelectedColorsSoFar = new Dictionary<int, int>(data.OwnStats);
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessNextGoodFile(FileData data)
		{
			SelectedColorsSoFar = SelectedColorsSoFar.Where(kvp => data.OwnStats.ContainsKey(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value + data.OwnStats[kvp.Key]);
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessGoodFiles()
		{
			ColorsLeftAfterFirstGood = 0;
			ColorsLeftAfterLastGood = 0;
			try
			{
				foreach (FileData snap in GoodSnaps)
					if (SelectedColorsSoFar.Count == 0)
					{
						if (!ProcessFirstGoodFile(snap)) return false;
						ColorsLeftAfterFirstGood = SelectedColorsSoFar.Count();
					}
					else
						if (!ProcessNextGoodFile(snap))
						{
							Debug.WriteLine("The file {0} finished to kill its category {1}", snap.FileName, Label);
							KilledBy = snap.FileName;
							return false;
						}
				return SelectedColorsSoFar.Count > 0;
			}
			finally
			{
				ColorsLeftAfterLastGood = SelectedColorsSoFar.Count;
				Debug.WriteLine("Processing {0} good files on {1} => Colors dropped from {2} down to {3}", GoodSnaps.Count, Label, ColorsLeftAfterFirstGood, ColorsLeftAfterLastGood);
			}

		}
		private bool ProcessBadFile(FileData data)
		{
			foreach (int color in data.OwnStats.Keys)
				if (SelectedColorsSoFar.ContainsKey(color))
					SelectedColorsSoFar.Remove(color);
			//SelectedColorsSoFar.Remove(color);
			return SelectedColorsSoFar.Count > 0;
		}
		private bool ProcessBadFiles()
		{
			try
			{
				foreach (FileData snap in BadSnaps)
					if (!ProcessBadFile(snap))
					{
						Debug.WriteLine("The file {0} finished to kill the category {1}", snap.FileName, Label);
						KilledBy = snap.FileName;
						return false;
					}
				return SelectedColorsSoFar.Count > 0;
			}
			finally
			{
				ColorsLeftAfterAllBad = SelectedColorsSoFar.Count;
				Debug.WriteLine("Processing {0} bad files on {1} => Colors dropped from {2} down to {3}", BadSnaps.Count, Label, ColorsLeftAfterLastGood, ColorsLeftAfterAllBad);
			}
		}
		public bool Process(List<DataPerCategory> allCategories)
		{
			foreach (DataPerCategory category in allCategories)
				AddBadCategory(category);
			if (ProcessGoodFiles() && ProcessBadFiles())
				return true;
			return false;
		}
	}
}
