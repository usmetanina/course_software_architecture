using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin.Hosting;
using System.IO;

namespace OwinSelfHostSample
{
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
                    

                    if (arr.ContainsKey(int.Parse(words[0])))
                    {
                        Console.WriteLine("double key " + words[0]);
                        arr[int.Parse(words[0])] = words[1];
                    }
                    else
                    {
                        if (StorageDB.val.ContainsKey(int.Parse(words[0])))
                        {
                            StorageDB.val[int.Parse(words[0])] = words[1];
                            Console.WriteLine("double key " + words[0]);
                        }
                        else
                        {
                            Console.WriteLine("no double");
                            arr.Add(int.Parse(words[0]), words[1]);
                        }
                    }
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
            StorageDB.port = args[0];
            StorageDB.slaves = args.Skip(1).ToList();
            Console.WriteLine(StorageDB.port);
            StorageDB.pathDB = "fileDB_" + args[0] + ".txt";
            StorageDB.pathLog = "fileLog_" + args[0]+ ".txt";


            StorageDB.val = getDB(StorageDB.pathLog);

            StorageDB.val = StorageDB.val.Concat(getDB(StorageDB.pathDB))
                       .ToDictionary(x => x.Key, x => x.Value);

            string baseAddress = "http://localhost:" + StorageDB.port + "/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Web Server starts on port " + StorageDB.port);
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
            }
        }
    }
}


