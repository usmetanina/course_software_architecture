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
        public static string pathLog = "logDB.txt";
        public static string pathDB = "fileDB.txt";

        public static Dictionary<int, string> val = 
            new Dictionary<int, string>();
               
        // GET /values 
        public Dictionary<int, string> Get()
        {
            return val;
        }

        // GET /values/5 
        public string Get(int id)
        {
            Console.WriteLine(id);
            if (!val.ContainsKey(id) || val[id]=="deleted")
            {
                return "404 Not Found";
            }
            else
            {
                return val[id];
            }
        }

        // POST /values 
        public void Post([FromBody]string value)
        {
        }

        // PUT /values/5 
        public string Put(int id, [FromBody]string value)
        {
            val[id] = value;
            if (val.Count % 5 == 0)
                WriteDictToFile();
            else
                WriteDictToLog(id, value);
            return "200 OK";
        }

        // DELETE /values/5 
        public void Delete(int id)
        {
            //async 
            val.Remove(id);
            WriteDictToLog(id,"deleted");
        }

        private void WriteDictToFile()
        {
            //System.IO.File.AppendAllText(pathLog, String.Empty);
            if (File.Exists(pathLog))
            {
                StreamWriter Writer = new StreamWriter(pathLog, false, Encoding.UTF8);
                Writer.WriteLine("");
                Writer.Close();
            }

            StreamWriter file = new StreamWriter(pathDB);
            lock (file)
            {           
                using (file)
                    foreach (var entry in val)
                        file.WriteLine("{0};{1}", entry.Key, entry.Value);
           }
        }
        private void WriteDictToLog(int key, string value)
        {
            using (StreamWriter sw = File.AppendText(pathLog))
            {
                sw.WriteLine("{0};{1}", key, value);
            }	
        }
    }
}
