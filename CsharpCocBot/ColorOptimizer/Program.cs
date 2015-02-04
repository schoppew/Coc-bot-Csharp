using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorOptimizer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			new ScanData(Properties.Settings.Default.DataPath1, Properties.Settings.Default.Name1);
			new ScanData(Properties.Settings.Default.DataPath2, Properties.Settings.Default.Name2);
			//Application.Run(new Form1());
		}
	}
}
