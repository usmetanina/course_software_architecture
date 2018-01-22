using OwinSelfHostSample;
using System;

namespace Coordinator
{
    class RelocationHandler
    {
        public void Reshard()
        {
            var oldTable = StorageProxy.LoadBucketShardTable("old_bucket_shard.txt");
            foreach (var row in oldTable)
            {
                Console.WriteLine("old shard " + row.Value);
                Console.WriteLine("new shard " + StorageProxy.bucketShardTable[row.Key]);
                if (row.Value != StorageProxy.bucketShardTable[row.Key])
                {
                    new ProxyController().SendOn(StorageProxy.keyBucketTable[row.Key], row.Value, 
                        StorageProxy.bucketShardTable[row.Key]);
                }
            }
        }
    }
}
