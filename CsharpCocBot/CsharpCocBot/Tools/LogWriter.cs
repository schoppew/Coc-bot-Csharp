namespace CoC.Bot.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _instance;
        private static Queue<LogEntry> _logQueue;
        private static readonly string _logDir = GlobalVariables.LogPath;
        private static readonly string _logFile = "CoC-Bot.txt";                // Your Log File Name
        private static readonly int _maxLogAge = 60;                            // Max Age in seconds
        private static readonly int _queueSize = 100;                           // Max Queue Size
        private static DateTime LastFlushed = DateTime.Now;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter() { }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get
            {
                // If the instance is null then create one and init the Queue
                if (_instance == null)
                {
                    _instance = new LogWriter();
                    _logQueue = new Queue<LogEntry>();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void WriteToLog(string message)
        {
            // Lock the queue while writing to prevent contention for the log file
            lock (_logQueue)
            {
                // Create the entry and push to the Queue
                LogEntry logEntry = new LogEntry(message);
                _logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_logQueue.Count >= _queueSize || DoPeriodicFlush())
                    FlushLog();
            }
        }

        /// <summary>
        /// Does the periodic flush.
        /// </summary>
        /// <returns><c>true</c> if max age since last flush, <c>false</c> otherwise.</returns>
        private static bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - LastFlushed;
            if (logAge.TotalSeconds >= _maxLogAge)
            {
                LastFlushed = DateTime.Now;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private static void FlushLog()
        {
            string logPath = Path.Combine(_logDir, string.Format("{0}_{1}", _logQueue.First().LogDate, _logFile));

			if (!Directory.Exists(_logDir))
				return;

            using (var fs = File.Open(logPath, FileMode.Append, FileAccess.Write))
            {
                using (var log = new StreamWriter(fs))
                {
                    while (_logQueue.Count > 0)
                    {
                        LogEntry entry = _logQueue.Dequeue();
                        log.WriteLine(string.Format("[{0}]\t{1}", entry.LogTime, entry.Message));
                    }
                }
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LogWriter"/> class.
        /// Log will be flushed even those limits are not reached yet when program is shutting down.
        /// </summary>
        ~LogWriter()
        {
            FlushLog();
        }
    }

    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class LogEntry
    {
        public string Message { get; set; }
        public string LogTime { get; set; }
        public string LogDate { get; set; }

        public LogEntry(string message)
        {
            Message = message;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("HH:mm:ss.fff");
        }
    }
}
