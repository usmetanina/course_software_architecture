using System;
using System.IO;
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
            StorageDB.val.Clear();

            File.WriteAllText(pathDB, string.Empty);
            File.WriteAllText(pathLog, string.Empty);
            return "200 OK";
        }

        // GET /db get count of elements 
        public int Get()
        {
            return StorageDB.val.Count;
        }
    }
}
