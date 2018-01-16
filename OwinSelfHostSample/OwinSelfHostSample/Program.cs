using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using System.IO;


namespace OwinSelfHostSample
{
    class Programx
    {

        public static Dictionary<int, string> getDB(string path)
        {
            var arr = new Dictionary<int, string>();

            if (!File.Exists(path))
            {
                File.Create(path);
            }
            else
            {
                foreach (var item in File.ReadLines(path).ToList())
                {
                    var words = item.Split(';');
                    arr[int.Parse(words[0])] = words[1];
                }
            }

            //var arr = File.ReadLines(path)
            //                .Select(line => line.Split(';'))
            //                .Where( split => split[0] != "Id" )
            //                .ToDictionary(split => int.Parse(split[0]), split => split[1]);
            return arr;
            //return (Dictionary<int, string>)arr.Where(a => a.Value != "deleted");

        }
        static void Main(string[] args)
        {
            ValuesController.val = getDB(ValuesController.pathLog);

            ValuesController.val = ValuesController.val.Concat(getDB(ValuesController.pathDB))
                       .ToDictionary(x => x.Key, x => x.Value);

            string baseAddress = "http://localhost:9000/";
            using (WebApp.Start<Startup>(url: baseAddress))
            {
                Console.WriteLine("Web Server is running.");
                Console.WriteLine("Press any key to quit.");
                Console.ReadLine();
                // Create HttpCient and make a request to api/values 
                //HttpClient client = new HttpClient();

                //var response = client.GetAsync(baseAddress + "values").Result;

                //Console.WriteLine(response);
                //Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                //Console.ReadLine();
            }
        }
    }
}


