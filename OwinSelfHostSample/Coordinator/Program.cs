using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coordinator
{
    public static class Storage
    {
        static public string port;
        public static string countNode;
        public static List<string> nodePorts = new List<string>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Storage.port = args[0];
            Storage.countNode = args[1];

            for (int i = 2; i < int.Parse(Storage.countNode) + 2; i++)
            {
                Console.WriteLine(args[i]);
                Storage.nodePorts.Add(args[i]);
            }
            Console.WriteLine(Storage.nodePorts[0]);
            Console.WriteLine(Storage.nodePorts[1]);
            string baseAddress = "http://localhost:" + Storage.port + "/";
            using (WebApp.Start<OwinSelfHostSample.Startup>(url: baseAddress))
            {
                Console.WriteLine("Proxy starts on port " + Storage.port);
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
