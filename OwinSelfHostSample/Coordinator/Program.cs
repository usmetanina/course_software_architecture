using Microsoft.Owin.Hosting;
using System;

namespace Coordinator
{
    class Program
    {
        static void Main(string[] args)
        {
            StorageProxy.port = args[0];
            StorageProxy.countNode = args[1];

            for (int i = 2; i < int.Parse(StorageProxy.countNode) + 2; i++)
            {
                Console.WriteLine(args[i]);
                StorageProxy.nodePorts.Add(args[i]);
            }

            StorageProxy.keyBucketTable = StorageProxy.LoadKeyBucketTable("key_bucket.txt");
            Console.WriteLine(StorageProxy.keyBucketTable.Count);
            StorageProxy.bucketShardTable = StorageProxy.LoadBucketShardTable("bucket_shard.txt");

            string baseAddress = "http://localhost:" + StorageProxy.port + "/";
            using (WebApp.Start<OwinSelfHostSample.Startup>(url: baseAddress))
            {
                Console.WriteLine("Proxy starts on port " + StorageProxy.port);
                Console.WriteLine("Press any key to quit.");

                RelocationHandler relocHandler = new RelocationHandler();
                relocHandler.Reshard();

                Console.ReadLine();
            }
        }
    }
}
