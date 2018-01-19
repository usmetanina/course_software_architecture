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
        private void getUrl(int id)
        {
            Console.WriteLine("key=id " + id);
            var bucket = ShardFunction(id);
            Console.WriteLine("bucket=ostatok "+bucket);
            var shard = Storage.bucketShardTable[bucket+1];
            Console.WriteLine("shard" + shard);
            client.url = "http://localhost:" + shard + "/values/" + id;
        }
        private int ShardFunction(int key)
        {
            //Console.WriteLine("Node ports " + Storage.nodePorts[0] + Storage.nodePorts[1]);
            Console.WriteLine("ccount shard " + Storage.bucketShardTable.Count);
            var bucket = key % Storage.bucketShardTable.Count;
            if (bucket == 0) return 1;
            else return bucket;
        }

        // GET /values/5 
        public string Get(int id)
        {
            getUrl(id);
            return client.Get(id);
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            getUrl(id);
            Storage.keyBucketTable[ShardFunction(id)].Add(id);
            return client.Put(id, value);
        }

        // DELETE /values/5 
        public string Delete(int id)
        {
            getUrl(id);
            return client.Delete(id);
        }
    }
}
