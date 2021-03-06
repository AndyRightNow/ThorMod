﻿using System;
using System.IO;
using System.Reflection;

namespace ADModUtils
{
    public class Logger
    {
        public static void Log(string logLevel, params string[] message)
        {
            var datetime = DateTime.Now;
            string logPath = Path.ChangeExtension(Assembly.GetExecutingAssembly().Location, ".log");
            if (!File.Exists(logPath))
            {
                File.WriteAllText(logPath, String.Empty);
            }

            try
            {
                var fs = new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read);
                var sw = new StreamWriter(fs);

                try
                {
                    sw.Write(string.Concat("[", datetime.ToString("HH:mm:ss"), "] ", logLevel, " "));

                    foreach (string str in message)
                    {
                        sw.Write(str);
                    }

                    sw.WriteLine();
                }
                finally
                {
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
