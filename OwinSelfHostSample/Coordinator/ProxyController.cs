using System;
using System.Collections.Generic;
using System.Web.Http;
using Coordinator;

namespace OwinSelfHostSample
{
    public class ProxyController : ApiController
    {
        Client client = new Client();

        public string CombineUrl(int id)
        {
            Console.WriteLine("key=id " + id);
            var bucket = ShardFunction(id);
            Console.WriteLine("bucket=ostatok "+bucket);
            var shard = StorageProxy.bucketShardTable[bucket];
            Console.WriteLine("shard" + shard);
            return "http://localhost:" + shard + "/values/" + id;
        }

        private int ShardFunction(int key)
        {
            Console.WriteLine("ccount shard " + StorageProxy.bucketShardTable.Count);
            var bucket = key % StorageProxy.bucketShardTable.Count;
            return bucket;
        }

        public string Get(int id)
        {
            Console.WriteLine("hi");
            Console.WriteLine("get " + id);
            client.url = CombineUrl(id);
            return client.Get(id);
        }

        public string Put(int id, [FromBody]string value)
        {
            client.url = CombineUrl(id);
            if (!StorageProxy.keyBucketTable[ShardFunction(id)].Contains(id))
            {
                StorageProxy.keyBucketTable[ShardFunction(id)].Add(id);
            }

            StorageProxy.WriteKeyBucketTable();
            return client.Put(id, value);
                
        }

        public string Delete(int id)
        {
            client.url = CombineUrl(id);
            StorageProxy.keyBucketTable[ShardFunction(id)].Remove(id);
            var response = client.Delete(id);
            StorageProxy.WriteKeyBucketTable();
            return response;
        }

        //сделать так, чтобы он завершился, его дождались все
        public void SendOn(List<int> ids, int oldShard, int newShard)
        {
            foreach (var id in ids)
            {
                Console.WriteLine("move "+id+ " from " + oldShard + " to " + newShard);

                var oldUrl = "http://localhost:" + oldShard + "/values/" + id;
                var newUrl = "http://localhost:" + newShard + "/values/" + id;
                client.url = oldUrl;
                var value = client.Get(id);

                client.url = newUrl;
                client.Put(id, value);

                client.url = oldUrl;
                client.Delete(id);
            }
        }
    }
}
