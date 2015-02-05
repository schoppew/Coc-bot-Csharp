using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using FastFind;

namespace CoC.Bot.Tools.FastFind
{
	static public class FastFindTesting
	{
		const string fullSize = "FullSize";
		const string smallSize = "SmallSize";
		static public void Test()
		{
			FastFindWrapper.SetHWnd(BlueStackHelper.GetBlueStackWindowHandle(), true); // Bind FastFind with BlueStack window
			FastFindWrapper.SnapShot(0, 0, 860, 720, 0); // Take full window capture
			FastFindWrapper.SnapShot(200, 200, 600, 500, 1); // Take just a small part
			FastFindWrapper.SetDebugMode(FastFindWrapper.DEBUG_SYSTEM_ERROR); // Console and File - Detailed System Message
			MessageBox.Show("FastFind Version " + FastFindWrapper.Version);			
			if (!FastFindWrapper.SaveBMP(0, fullSize))
			
				MessageBox.Show("Failed to save full BS capture into " + FastFindWrapper.LastSavedFileName);
			else
				MessageBox.Show("Succeeded to save full BS capture into " + FastFindWrapper.LastSavedFileName);
			string file0 = FastFindWrapper.LastSavedFileName;
			
			if (!FastFindWrapper.SaveJPG(1, smallSize, 100))
				MessageBox.Show("Failed to save partial BS capture into " + FastFindWrapper.LastSavedFileName);
			else
				MessageBox.Show("Succeeded to save partial BS capture into " + FastFindWrapper.LastSavedFileName);
			string file1 = FastFindWrapper.LastSavedFileName;
			int color10x10 = FastFindWrapper.GetPixel(10,10,0);//Change the Snapshot 0
			int color10x10_2 = FastFindHelper.GetPixel(10, 10);
			//FastFindWrapper.SnapShot(500, 500, 550, 550, 0);//Change the Snapshot 0
			//int color10x10_2 = FastFindWrapper.GetPixel(10, 10, 0);//Change the Snapshot 0
			
			//if (!FastFindWrapper.LoadFromFile(2, file0))
			//	MessageBox.Show("Failed to reload full BS capture from " + file0);
			//int color10x10_3 = FastFindWrapper.GetPixel(10, 10, 2);//Change the Snapshot 0
			//string lastFileName = fullSize + "(2)";
			//if (!FastFindWrapper.SaveJPG(2, lastFileName, 100))
			//	MessageBox.Show("Failed to resave full BS capture");
			var sc = new ExtBitmap.ExtBitmap();
			sc.SnapShot(BlueStackHelper.GetBlueStackWindowHandle(),true);
			sc.Save("SC_True.jpg");
			sc.SnapShot(BlueStackHelper.GetBlueStackWindowHandle(),false);
			sc.Save("SC_False.jpg");
		}
	}
}
