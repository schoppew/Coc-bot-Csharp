using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorOptimizer
{
	public partial class ScanData
	{

		public ScanData(string rootDirectory, string className)
		{
			if (!Directory.Exists(rootDirectory))
			{
				MessageBox.Show(string.Format("{0} is missing or empty!\nPlease create one subdirectory for each item you need to detect,\nand put in each directory all the bitmaps corresponding to that item.\n(only .bmp or .png, no .jpg)", rootDirectory));
				return;
			}
			List<DataPerCategory> mainList = new List<DataPerCategory>();
			var directories = Directory.EnumerateDirectories(rootDirectory);
			foreach (string directory in directories)
			{
				string path = directory;
				string name = Path.GetFileName(path);
				string[] files = Directory.EnumerateFiles(path, "*.png").Concat(Directory.EnumerateFiles(path, "*.bmp")).ToArray();
				DataPerCategory category = new DataPerCategory(name, files);
				if (category.GoodSnaps.Count > 0)
					mainList.Add(new DataPerCategory(name, files));
			}

			//OK, now all initial Good color lists are processed, let remove those that are bad
			foreach (DataPerCategory category in mainList)
				category.Process(mainList);
			ExportFile(Path.Combine(rootDirectory, className + ".cs"), className, mainList);
		}

	
	}
}
