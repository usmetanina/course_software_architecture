using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace ScriptTest
{
    class Script
    {
        //void Script() { }
        string url = "http://localhost:9000/";
        HttpClient client = new HttpClient();

        private async Task Put(int id, string value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(url + "values/" + id, content);
            
            //return response.Content.ReadAsStringAsync().Result;
        }

        private string Get()
        {
            var responses = client.GetAsync(url + "values/").Result;
            return responses.Content.ReadAsStringAsync().Result;
        }
        private string Get(int id)
        {
            var responses = client.GetAsync(url + "values/" + id).Result;
            return responses.Content.ReadAsStringAsync().Result;
        }

        public void run_script()
        {
            client.DeleteAsync(url + "db/");

            for (int i = 0; i < 200; i++)
            {
                Random rnd = new Random();
                int number = rnd.Next();
                //Task.WaitAll(Task.Run(() => Put(number, number + "ths")));
                Put(number, number + "ths").Wait();
                Console.WriteLine(i);
            }

            var answers_json = Get();
            var itog_answers = JsonConvert.DeserializeObject<Dictionary<int, string>>(answers_json);
            int countInsert = itog_answers.Count;
            Console.WriteLine(countInsert);
            if (200 == countInsert)
            {
                Console.WriteLine("OK insert 200");
            }
            else
            {
                Console.WriteLine("NOT insert 200");
            }

            var answer = Put(1, "first");

            if ("200 OK".Equals(answer))
            {
                Console.WriteLine("OK put first");
            }
            else
            {
                Console.WriteLine("NOT put first");
            }

            var answers = Get(1);
            if ("first".Equals(answers))
            {
                Console.WriteLine("OK get first");
            }
            else
            {
                Console.WriteLine("NOT get first");
            }


            Put(1, "first");
            answers = Get(1);
            if ("200 OK".Equals(answers))
            {
                Console.WriteLine("OK put first");
            }
            else
            {
                Console.WriteLine("NOT put first");
            }


            Put(1, "first_first");
            answers = Get(1);
            if ("200 OK".Equals(answers))
            {
                Console.WriteLine("OK edit first");
            }
            else
            {
                Console.WriteLine("NOT edit first");
            }


            var id = 1;
            var deleted = client.DeleteAsync(url + "values/" + id).Result;
            var response = client.GetAsync(url + "values/" + id).Result;
            answers = response.Content.ReadAsStringAsync().Result;
            if ("404 Not Found".Equals(answers))
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
            Console.WriteLine("Test script run \n ================");
            var a = new Script();
            a.run_script();
            Console.WriteLine("Test script done \n ================");
            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}
