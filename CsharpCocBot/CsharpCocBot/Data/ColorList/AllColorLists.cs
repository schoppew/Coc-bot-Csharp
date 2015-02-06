using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoC.Bot.Data.ColorList
{
	public class AllColorLists
	{
		public ColorListBase[] ColorLists = {
				new TH(),     // Hall Towns
				new Wall(),   // Walls
		};

		const string separator = ";";
		void MakeStats(string csvFullFileName, string syntheticFileName, ExtBitmap.ExtBitmap bitmap)
		{
			List<string> detailedReport = new List<string>();
			List<string> SyntheticReport = new List<string>();

			if (!File.Exists(syntheticFileName))
				SyntheticReport.Add(string.Join(";", (new string[] { "FileName" }).Concat(ColorLists.Select(cl => cl.GenericName))));
			

		}
	}
}
