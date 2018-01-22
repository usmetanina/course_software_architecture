using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Coordinator
{
    public class Client
    {
        public string url = "http://localhost:9000";
        HttpClient client = new HttpClient();

        //public string Get()
        //{
        //    var responses = client.GetAsync(url).Result;
        //    return responses.Content.ReadAsStringAsync().Result;
        //}

        public string Get(int id)
        {
            var responses = client.GetAsync(url).Result;
            return responses.Content.ReadAsStringAsync().Result;
        }

        public string Put(int id, [FromBody]string value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url, content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        public string Delete(int id)
        {
            var deleted = client.DeleteAsync(url).Result;
            return deleted.Content.ReadAsStringAsync().Result;
        }
    }
}
