using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.Plugin;

namespace PluginContracts
{
	// This interface is implemented by the Coc Bot, and exposes tools to be used by the plugin
	public interface ICocBot
	{
		/// <summary>
		/// Gets the HWND to the BlueStacks Window
		/// </summary>
		/// <returns></returns>
		IntPtr GetCOCWindow();
		
		/// <summary>
		/// Not implemented yet
		/// </summary>
		/// <returns></returns>
		Bitmap GetLastScreenCapture();

		/// <summary>
		/// Simulates a mouse click in the Clash of Clan game. 
		/// x & y are relative to the client area of the BlueStacks window
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="delayInMs"></param>
		/// <param name="nbClicks"></param>
		void Click(int x, int y, int delayInMs = 10, int nbClicks=1);
		
		/// <summary>
		/// Get the color pixel from a given point
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		Color GetPixel(int x, int y);
		
		/// <summary>
		/// Writes texts to the output textbox of the bot
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		void WriteToOutput(string format, params object[] args);


		/// <summary>
		/// Gives access to the other plugins, so different plugins can communicate. 
		/// </summary>
		ICollection<ICocPlugin> Plugins { get; }

	}
}
