using System;
using System.IO;
using System.Text;

/// <summary>
/// Text Adventurer namespace
/// </summary>
namespace TextAdventurer
{
    /// <summary>
    /// Logger class
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// Info path
        /// </summary>
        private static string infoPath = "./info.log";

        /// <summary>
        /// Error path
        /// </summary>
        private static string errorPath = "./error.log";

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="path">Path</param>
        private static void Log(object obj, string path)
        {
            try
            {
                File.AppendAllText(path, DateTime.Now.ToString() + ((obj == null) ? "null" : obj.ToString()) + Environment.NewLine, Encoding.UTF8);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="obj">Object</param>
        public static void LogInfo(object obj) => Log(obj, infoPath);

        /// <summary>
        /// Log error
        /// </summary>
        /// <param name="obj">Object</param>
        public static void LogError(object obj) => Log(obj, errorPath);
    }
}
