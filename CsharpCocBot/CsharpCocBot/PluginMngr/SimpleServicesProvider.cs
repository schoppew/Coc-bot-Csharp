using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoC.Bot.BotEngine;
using CoC.Bot.Plugin;
using CoC.Bot.Tools;
using CoC.Bot.ViewModels;
using PluginContracts;

namespace CoC.Bot.PluginMngr
{
	public class SimpleServicesProvider : ICocBot
	{
		MainViewModel vm = null;
		PluginLoader parent = null;
		public SimpleServicesProvider(PluginLoader _parent, MainViewModel _vm)
		{
			this.parent = _parent;
			vm = _vm;
		}
		#region ICocBot implementation
		IntPtr ICocBot.GetCOCWindow()
		{
			return BlueStacksHelper.GetBlueStacksWindowHandle();
		}
		Bitmap ICocBot.GetLastScreenCapture()
		{
			//CoCHelper.MakeFullScreenCapture
			return null;
		}
		void ICocBot.Click(int x, int y, int delayInMs, int nbClicks)
		{
			CoCHelper.Click(new Data.ClickablePoint(x, y), nbClicks, delayInMs);
		}
		Color ICocBot.GetPixel(int x, int y)
		{
			return Color.Empty;
		}

		void ICocBot.WriteToOutput(string format, params object[] args)
		{
			if (vm!=null)
				vm.WriteToOutput(string.Format(format, args));
		}

		public ICollection<ICocPlugin> Plugins { get { return parent.Plugins; } }

		#endregion ICocBot implementation
	}
}
