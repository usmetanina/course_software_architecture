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
        private string ShardFunction(int key)
        {

            //Console.WriteLine("Node ports " + Storage.nodePorts[0] + Storage.nodePorts[1]);
            return Storage.nodePorts[key % int.Parse(Storage.countNode)];
        }

        // GET /values/5 
        public string Get(int id)
        {
            client.url = "http://localhost:" + ShardFunction(id) + "/values/" + id;
            //Console.WriteLine("Shadr go " + ShardFunction(id));
            return client.Get(id);
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            //Console.WriteLine("Shadr go " + ShardFunction(id));
            client.url = "http://localhost:" + ShardFunction(id) + "/values/" + id;
            return client.Put(id, value);
        }

        // DELETE /values/5 
        public string Delete(int id)
        {
            client.url = "http://localhost:" + ShardFunction(id) + "/values/" + id;
            return client.Delete(id);
        }
    }
}
