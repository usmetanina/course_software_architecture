using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OwinSelfHostSample
{
    public class DbController : ApiController
    {
        public static string pathLog = "logDB.txt";
        public static string pathDB = "fileDB.txt";

        public string Delete()
        {
            Console.WriteLine("clear DB");
            Storage.val.Clear();

            File.WriteAllText(pathDB, string.Empty);
            File.WriteAllText(pathLog, string.Empty);

            //if (File.Exists(pathDB))
            //{
            //    StreamWriter Writer = new StreamWriter(pathDB, false, Encoding.UTF8);
            //    Writer.WriteLine("");
            //    Writer.Close();
            //}
            return "200 OK";
        }
    }
}
