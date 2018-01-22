using System;
using System.Threading;
using Coordinator;

namespace ScriptTest
{
    class myThread
    {
        Thread thread;
        public myThread()
        {
            thread = new Thread(this.run_script);
            thread.Start();
        }

        string url = "http://localhost:9000/proxy/";
        Client client = new Client();


        //private async Task<string> Put(int id, string value)
        //{
        //    var jsonString = JsonConvert.SerializeObject(value);
        //    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //    var response = await client.PutAsync(url + "values/" + id, content);
        //    //using (var response = await client.PutAsync(url + "values/" + id, content))
        //    //{
        //    //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    //    {
        //    //        var contents = await response.Content.ReadAsStringAsync();
        //    //        //var result = contents.Result;
        //    //    }
        //    //}
        //    return response.Content.ReadAsStringAsync().Result;
        //}

        private void run_script()
        {
            //client.DeleteAsync(url + "db/");

            for (int i = 0; i < 200; i++)
            {
                Random rnd = new Random();
                int number = rnd.Next();
                //Task.WaitAll(Task.Run(() => Put(number, number + "ths")));
                client.url = url + number;
                client.Put(number, number + "ths");
                Console.WriteLine(i);
            }

            var id = 1;
            client.url = url + id;
            var answer = client.Put(1, "first");
            Console.WriteLine(answer);
            if ("\"200 OK\"".Equals(answer))
            {
                Console.WriteLine("OK put first");
            }
            else
            {
                Console.WriteLine("NOT put first");
            }

            var answers = client.Get(1);
            if ("\"first\"".Equals(answers))
            {
                Console.WriteLine("OK get first");
            }
            else
            {
                Console.WriteLine("NOT get first");
            }

            client.Put(1, "first_first");
            answers = client.Get(1);
            if ("\"first_first\"".Equals(answers))
            {
                Console.WriteLine("OK edit first");
            }
            else
            {
                Console.WriteLine("NOT edit first");
            }

            var deleted = client.Delete(id);
            answers = client.Get(id);
            //answers = response.Content.ReadAsStringAsync().Result;
            if ("\"404 Not Found\"".Equals(answers))
            {
                Console.WriteLine("OK delete first");
            }
            else
            {
                Console.WriteLine("NOT delete first");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test scripts run \n ================");
            myThread t1 = new myThread();
            myThread t2 = new myThread();
            Console.WriteLine("Test script done \n ================");
            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}
