using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Data.ColorList.Tools;

namespace CoC.Bot.Data.ColorList
{
	static public class AllColorLists
	{
		static public ColorListBase[] ColorLists = {
				new TH(),     // Hall Towns
				new Wall(),   // Walls
		};

		static public void MakeStatsFromDirectory(string csvFullFileName, string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
				return;
			foreach(string fileName in Directory.EnumerateFiles(directoryPath))
				switch (Path.GetExtension(fileName).ToUpperInvariant())
				{
					case ".GIF":
					case ".BMP":
					case ".PNG":
						MakeStats(csvFullFileName, fileName);
						break;
				}
		}

		const string separator = ";";
		static public void MakeStats(string csvFullFileName, string fileName)
		{
			ExtBitmap.ExtBitmap bitmap = new ExtBitmap.ExtBitmap();
			if (!bitmap.LoadFromFile(fileName))
			{
				File.AppendAllText(fileName, string.Format("File {0} not found\r\n", fileName));
				return;
			}
			MakeStats(csvFullFileName, bitmap);
		}

		static public void MakeStats(string csvFullFileName, ExtBitmap.ExtBitmap bitmap)
		{
            //DataCollection.DebugData = new System.Collections.ObjectModel.ObservableCollection<Type.DebugDataModel>();
			List<string> detailedReport = new List<string>();
			List<string> syntheticReport = new List<string>();
			try
			{
				if (!File.Exists(csvFullFileName))
				{
					var res = ColorLists.Select(cl => cl.Items.Select(it => cl.GenericName + " - " + it.Name)).Aggregate((ies1, ies2) => ies1.Concat(ies2));
					syntheticReport.Add(string.Join(separator, (new string[] { "FileName" }).Concat(res)));
					return;
				}
				
				List<string> items = new List<string>();
				items.Add(bitmap.FileName);
				foreach (ColorListBase colorList in ColorLists)
				{
					foreach (ColorListItem colors in colorList.Items)
					{
						decimal res = ColorListsHelper.Count(colors, bitmap, 0);
						DataCollection.DebugData.Add(new Type.DebugDataModel() { Count = res, Description = colors.Name });
						items.Add(string.Format("{0}", res.ToString("G")));
					}
				}
				syntheticReport.Add(string.Join(separator, items));
			}
			finally
			{
				File.AppendAllLines(csvFullFileName, syntheticReport);
			}
		}

		
	}
}
