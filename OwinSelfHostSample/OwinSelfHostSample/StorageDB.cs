using System.Collections.Generic;
using System.IO;

namespace OwinSelfHostSample
{
    public static class StorageDB
    {
        static public string port;
        public static string pathLog;
        public static string pathDB;
        public static Dictionary<int, string> val = new Dictionary<int, string>();
        public static List<string> slaves;

        static public void WriteDictToFile()
        {
            if (File.Exists(pathLog))
            {
                File.WriteAllText(pathLog, string.Empty);
            }

            StreamWriter file = new StreamWriter(pathDB);
            lock (file)
            {
                using (file)
                    foreach (var entry in val)
                        file.WriteLine("{0};{1}", entry.Key, entry.Value);
            }
        }

        static public void WriteDictToLog(int key, string value)
        {
            using (StreamWriter sw = File.AppendText(pathLog))
            {
                sw.WriteLine("{0};{1}", key, value);
            }
        }
    }
}
