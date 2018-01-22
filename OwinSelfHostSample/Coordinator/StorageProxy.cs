using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Coordinator
{
    class StorageProxy
    {

        static public string port;
        public static string countNode;
        public static string pathKeyBucketTable = "key_bucket.txt";
        public static List<string> nodePorts = new List<string>();

        public static Dictionary<int, List<int>> keyBucketTable
                    = new Dictionary<int, List<int>>();

        public static Dictionary<int, int> bucketShardTable = new Dictionary<int, int>();

        public static bool isResharding = false;

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
                    Console.WriteLine("load bucketshard, bucket is " + int.Parse(words[0]));
                    Console.WriteLine("load bucketshard, shard is " + int.Parse(words[1]));
                    table.Add(int.Parse(words[0]), int.Parse(words[1]));
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
}
