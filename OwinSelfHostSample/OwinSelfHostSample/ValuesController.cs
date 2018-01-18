using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.Http;
using System.IO;

namespace OwinSelfHostSample
{
    public class ValuesController : ApiController
    {
                      
        // GET /values 
        public Dictionary<int, string> Get()
        {
            return Storage.val;
        }

        // GET /values/5 
        public string Get(int id)
        {
            Console.WriteLine(id);
            if (!Storage.val.ContainsKey(id) || Storage.val[id]=="deleted")
            {
                return "404 Not Found";
            }
            else
            {
                return Storage.val[id];
            }
        }

        // POST /values 
        public void Post([FromBody]string value)
        {
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            Storage.val[id] = value;
            if (Storage.val.Count % 5 == 0)
                Storage.WriteDictToFile();
            else
                Storage.WriteDictToLog(id, value);
            return "200 OK";
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

        // DELETE /values/5 
        public void Delete(int id)
        {
            Storage.val.Remove(id);
            Storage.WriteDictToLog(id,"deleted");
        }
    }
}
