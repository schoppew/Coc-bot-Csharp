using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
	/// <summary>
	/// This small class is design to simplify benchmarking. 
	/// It is used this way:
	///     using (var chrono = new Chrono("I explain here what I'm doing"))
	///			{
	///			// Put here the code you want to benchmark
	///			}
	/// </summary>
	public class Chrono : IDisposable
	{
		static bool starting = true;
		int _nbLoop;
		Stopwatch sw;
		string _description;
		public string Comment { get; set; }
		static public StringEventHandler callOnComplete;
		public bool? Success { get; set; }
		[SerializableAttribute]
		[ComVisibleAttribute(true)]
		public delegate void StringEventHandler(
			Object sender,
			string description,
			double elapsedms
		);

		public Chrono(string description, int nbLoop=1)
		{
			if (!Directory.Exists("Logs"))
				Directory.CreateDirectory("Logs");
			_nbLoop = nbLoop;
			sw = Stopwatch.StartNew();
			_description = description;
		}

		public void LogMe(string line)
		{
			if (starting)
			{
				File.AppendAllText(@"Logs\TU_log.txt", string.Format("\r\n====================== {0} =======================\r\n", DateTime.Now.ToString()));
				
				starting = false;
			}
			File.AppendAllText(@"Logs\TU_log.txt", line + "\r\n");
			if (!string.IsNullOrEmpty(Comment))
				File.AppendAllText(@"Logs\TU_log.txt", "\t\t" + Comment + "\r\n");

		}
		public void Dispose()
		{
			double elapsed = sw.Elapsed.TotalMilliseconds / _nbLoop;
			if (callOnComplete != null)
			{
				callOnComplete(this, _description, elapsed);
			}
			string line = null;
			switch (Success)
			{
				case null:
					line = string.Format("{0} completed in {1:00.000}ms", _description, elapsed);
					break;
				case true:
					line = string.Format("{0} succeeded in {1:00.000}ms", _description, elapsed);
					break;
				case false:
					line = string.Format("{0} failed in {1:00.000}ms", _description, elapsed);
					break;
			}
			Debug.WriteLine(line);
			LogMe(line);
		}
	}
}
