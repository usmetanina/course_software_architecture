using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using System.IO;
using Coordinator;
using System.Net.Http;
using Newtonsoft.Json;

namespace OwinSelfHostSample
{
    public class ValuesController : ApiController
    {
        Client client = new Client();

        public string getUrl(int id)
        {
            Console.WriteLine("key=id " + id);
            var bucket = ShardFunction(id);
            Console.WriteLine("bucket=ostatok "+bucket);
            var shard = Storage.bucketShardTable[bucket];
            Console.WriteLine("shard" + shard);
            return "http://localhost:" + shard + "/values/" + id;
        }

        private int ShardFunction(int key)
        {
            //Console.WriteLine("Node ports " + Storage.nodePorts[0] + Storage.nodePorts[1]);
            Console.WriteLine("ccount shard " + Storage.bucketShardTable.Count);
            var bucket = key % Storage.bucketShardTable.Count;
            return bucket;
        }

        // GET /values/5 
        public string Get(int id)
        {
            Console.WriteLine("hi");
            Console.WriteLine("get " + id);
            client.url = getUrl(id);
            return client.Get(id);
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            client.url = getUrl(id);
            Storage.keyBucketTable[ShardFunction(id)].Add(id);
            //var response = client.Put(id, value);
            Storage.WriteKeyBucketTable();
            return client.Put(id, value);// response;
                
        }

        // DELETE /values/5 
        public string Delete(int id)
        {
            client.url = getUrl(id);
            Storage.keyBucketTable[ShardFunction(id)].Remove(id);
            var response = client.Delete(id);
            Storage.WriteKeyBucketTable();
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
