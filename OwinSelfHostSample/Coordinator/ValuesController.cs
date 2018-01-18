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
        string url = "http://localhost:";
        HttpClient client = new HttpClient();

        private string ShardFunction(int key)
        {
            Console.WriteLine("Node ports " + Storage.nodePorts[0] + Storage.nodePorts[1]);
            return Storage.nodePorts[key % int.Parse(Storage.countNode)];
        }

        // GET /values/5 
        public string Get(int id)
        {
            Console.WriteLine("Shadr go " + ShardFunction(id));
            var responses = client.GetAsync(url + ShardFunction(id) + "/values/").Result;
            return responses.Content.ReadAsStringAsync().Result;
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            Console.WriteLine("Shadr go " + ShardFunction(id));
            var jsonString = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url + ShardFunction(id) + "/values/" + id, content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        // DELETE /values/5 
        public string Delete(int id)
        {
            var deleted = client.DeleteAsync(url + ShardFunction(id) + "/values/" + id).Result;
            return deleted.Content.ReadAsStringAsync().Result;
        }
    }
}
