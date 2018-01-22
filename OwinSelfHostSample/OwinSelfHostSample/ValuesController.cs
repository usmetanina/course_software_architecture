using System;
using System.Text;
using System.Web.Http;
using Coordinator;
using System.Net.Http;
using Newtonsoft.Json;

namespace OwinSelfHostSample
{
    public class ValuesController : ApiController
    {

        public string Get(int id)
        {
            Console.WriteLine(id);
            if (!StorageDB.val.ContainsKey(id) || StorageDB.val[id]=="deleted")
            {
                return "404 Not Found";
            }
            else
            {
                return StorageDB.val[id];
            }
        }

        public string Put(int id, [FromBody]string value)
        {
            StorageDB.val[id] = value;
            if (StorageDB.val.Count % 5 == 0)
                StorageDB.WriteDictToFile();
            else
                StorageDB.WriteDictToLog(id, value);

            Client client = new Client();
            WriteToReplics(id, value);

            //foreach (var port in StorageTable.slaves)
            //{
            //    Console.WriteLine("write to " + port);
            //    client.url = "http://localhost:" + port + "/values/" + id;
            //    Console.WriteLine(client.url);
            //    client.Put(id, value);
            //}
            return "200 OK";
        }

        async private void WriteToReplics(int id, string value)
        {
            var httpClient = new HttpClient();
            foreach (var port in StorageDB.slaves)
            {
                var jsonString = JsonConvert.SerializeObject(value);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                Console.WriteLine("write to " + port + " " + DateTime.Now.Millisecond);
                var request = httpClient.PutAsync("http://localhost:" + port + "/values/" + id, content).Result;
                var response = await request.Content.ReadAsStringAsync();
                Console.WriteLine("wrote to " + port + " " + DateTime.Now.Millisecond);
            }
        }
        //public string Put(int id, [FromBody]string value)
        //{
        //    Task t = Task.Run(() => {
        //       val[id] = value;
        //       if (val.Count % 5 == 0)
        //           WriteDictToFile();
        //       else
        //           WriteDictToLog(id, value);
        //       //return "200 OK";
        //   });
        //    while (true)
        //    {
        //        if (t.IsCompleted) return "200 OK";
        //    }
        //}

        public string Delete(int id)
        {
            StorageDB.val.Remove(id);
            StorageDB.WriteDictToLog(id,"deleted");

            Client client = new Client();
            foreach (var port in StorageDB.slaves)
            {
                client.url = "http://localhost:" + port + "/values/" + id; ;
                client.Delete(id);
            }
            return "200 OK";
        }
    }
}
