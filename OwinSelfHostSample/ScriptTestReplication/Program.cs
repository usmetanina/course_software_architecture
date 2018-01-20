using System;
using Coordinator;
using System.Diagnostics;

namespace ScriptTestReplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test scripts replication run \n ================");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9002");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9003");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9001 9002 9003");

            Client client = new Client();
            var id = 5961;
            client.url = "http://localhost:9001/values/" + id;
            client.Put(id, "first");
            var valueFromMaster = client.Get(id);

            client.url = "http://localhost:9002/values/" + id;
            var valueFromSlave1 = client.Get(id);

            client.url = "http://localhost:9003/values/" + id;
            var valueFromSlave2 = client.Get(id);
            Console.WriteLine(valueFromMaster);
            Console.WriteLine(valueFromSlave1);
            Console.WriteLine(valueFromSlave2);

            if (valueFromMaster.Equals(valueFromSlave1) && valueFromMaster.Equals(valueFromSlave2))
            {
                Console.WriteLine("Values from master and slaves are equals \n");
            }
            else
            {
                Console.WriteLine("Values from master and slaves are not equals \n");
            }
            Console.WriteLine("Test script done \n ================");
            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}
