using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.IO;


namespace OwinSelfHostSample
{
    public static class Storage
    {
        static public string port;
        public static string pathLog;
        public static string pathDB;
        public static Dictionary<int, string> val = new Dictionary<int, string>();

        static public void WriteDictToFile()
        {
            //System.IO.File.AppendAllText(pathLog, String.Empty);
            if (File.Exists(pathLog))
            {
                StreamWriter Writer = new StreamWriter(pathLog, false, Encoding.UTF8);
                Writer.WriteLine("");
                Writer.Close();
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

    class Programx
    {
        public static Dictionary<int, string> getDB(string path)
        {
            var arr = new Dictionary<int, string>();

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                foreach (var item in File.ReadLines(path).ToList())
                {
                    var words = item.Split(';');
                    arr.Add(int.Parse(words[0]), words[1]);
                }
            }

            //var arr = File.ReadLines(path)
            //                .Select(line => line.Split(';'))
            //                .Where( split => split[0] != "Id" )
            //                .ToDictionary(split => int.Parse(split[0]), split => split[1]);
            return arr;
            //return (Dictionary<int, string>)arr.Where(a => a.Value != "deleted");

        }
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            Storage.port = args[0];
            Console.WriteLine(Storage.port);
            Storage.pathDB = "fileDB_" + args[0] + ".txt";
            Storage.pathLog = "fileLog_" + args[0]+ ".txt";


            Storage.val = getDB(Storage.pathLog);

            Storage.val = Storage.val.Concat(getDB(Storage.pathDB))
                       .ToDictionary(x => x.Key, x => x.Value);

            string baseAddress = "http://localhost:" + Storage.port + "/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Web Server starts on port " + Storage.port);
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
                // Create HttpCient and make a request to api/values 
                //HttpClient client = new HttpClient();

                //var response = client.GetAsync(baseAddress + "values").Result;

                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                //Console.ReadLine();
            }
        }
    }
}


