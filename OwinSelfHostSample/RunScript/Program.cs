using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunScript
{
    class Program
    {
        static void Main(string[] args)
        {
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9002");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9003");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9001 9002 9003");

            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9005");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9004 9005");

            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9007");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9006 9007");

            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9009");
            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\OwinSelfHostSample\bin\Release\OwinSelfHostSample.exe", "9008 9009");

            Process.Start(@"C:\Users\home-pc\Documents\Visual Studio 2015\Projects\OwinSelfHostSample\OwinSelfHostSample\Coordinator\bin\Release\Coordinator.exe", "9000 4 9001 9004 9006 9008");
        }
    }
}
