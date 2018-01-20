using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{
    public static class Storage
    {
        static public string port;
        public static string countNode;
        public static string pathKeyBucketTable = "key_bucket.txt";
        public static List<string> nodePorts = new List<string>();

        public static Dictionary<int, List<int>> keyBucketTable
                    = new Dictionary<int, List<int>>();

        public static Dictionary<int, int> bucketShardTable = new Dictionary<int, int>();

        static public Dictionary<int, int> LoadBucketShardTable(string path)
        {
            var table = new Dictionary<int, int>();
            Console.WriteLine("path " + path);
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                foreach (var item in File.ReadLines(path).ToList())
                {
                    var words = item.Split(';');
                    Console.WriteLine("я в load bucketshard, bucket is " + int.Parse(words[0]));
                    Console.WriteLine("я в load bucketshard, shard is " + int.Parse(words[1]));
                    table.Add(int.Parse(words[0]),int.Parse(words[1]));
                }
            }
            return table;
        }

        static public void WriteKeyBucketTable()
        {
            if (!File.Exists(pathKeyBucketTable))
            {
                File.Create(pathKeyBucketTable);
            }
            else
            {
            using (StreamWriter wr = new StreamWriter(pathKeyBucketTable, false))
                {
                    foreach (var row in keyBucketTable)
                    {
                        wr.Write(row.Key);
                        string keys = "";
                        foreach (var item in row.Value)
                            keys += ";" + item.ToString();
                        wr.WriteLine(keys);
                    }
                }
            }
        }

        static public new Dictionary<int, List<int>> LoadKeyBucketTable(string path)
        {
            var table = new Dictionary<int, List<int>>();
            Console.WriteLine("path " + path);
            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                foreach (var item in File.ReadLines(path).ToList())
                {
                    var words = item.Split(';');
                    int bucket = Convert.ToInt32(words[0]);
                    Console.WriteLine("я в load keybucket" + bucket);
                    words.ToList().RemoveAt(0);
                    var arr = new List<int>();
                    for (int i = 1; i < words.Length; i++)
                    {
                        Console.WriteLine("я в load keybucket, key" + int.Parse(words[i]));
                        arr.Add(int.Parse(words[i]));
                    }
                    table.Add(bucket, arr);
                }
            }
            return table;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Storage.port = args[0];
            Storage.countNode = args[1];

            for (int i = 2; i < int.Parse(Storage.countNode) + 2; i++)
            {
                Console.WriteLine(args[i]);
                Storage.nodePorts.Add(args[i]);
            }
            Console.WriteLine(Storage.nodePorts[0]);
            Console.WriteLine(Storage.nodePorts[1]);

            Storage.keyBucketTable = Storage.LoadKeyBucketTable("key_bucket.txt");
            Console.WriteLine(Storage.keyBucketTable.Count);
            Storage.bucketShardTable = Storage.LoadBucketShardTable("bucket_shard.txt");

            string baseAddress = "http://localhost:" + Storage.port + "/";
            using (WebApp.Start<OwinSelfHostSample.Startup>(url: baseAddress))
            {
                Console.WriteLine("Proxy starts on port " + Storage.port);
                Console.WriteLine("Press any key to quit.");

                RelocationHandler relocHandler = new RelocationHandler();
                relocHandler.Reshard();

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
