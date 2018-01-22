using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        string url = "http://localhost:9000/proxy/";
        HttpClient client = new HttpClient();

        private string Put(int id, string value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = client.PutAsync(url + id, content).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        private string Get()
        {
            var responses = client.GetAsync(url).Result;
            return responses.Content.ReadAsStringAsync().Result;
        }

        private string Get(int id)
        {
            var responses = client.GetAsync(url + id).Result;
            return responses.Content.ReadAsStringAsync().Result;
        }

        [TestMethod]
        public void TestMethodAdd200RowReturnOK()
        {
            HttpClient client = new HttpClient();
            for (int i = 0; i < 200; i++)
            {
                Put(i, i + "ths");
            }
            var answers_json = Get();
            var itog_answers = JsonConvert.DeserializeObject<Dictionary<int, string>>(answers_json);
            Assert.AreEqual(200, itog_answers.Count);
            var answer = Get(50);
            Assert.AreEqual("\"50ths\"", answer);
        }

        [TestMethod]
        public void TestMethodPutValueByIdReturnOK()
        {
            var answer = Put(1, "first");
            Assert.AreEqual("\"200 OK\"", answer);
        }

        [TestMethod]
        public void TestMethodGetValueByIdReturnValue()
        {
            var answer = Get(1);
            Assert.AreEqual("\"first\"", answer);
        }

        [TestMethod]
        public void TestMethodUpdateValueByIdReturnValue()
        {
            Put(1, "first");
            var answer = Get(1);
            Assert.AreEqual("\"first\"", answer);

            Put(1, "first_first");
            answer = Get(1);
            Assert.AreEqual("\"first_first\"", answer);
        }

        [TestMethod]
        public void TestMethodGetValueByIdReturnNotFound()
        {
            HttpClient client = new HttpClient();
            var id = 1;
            var deleted = client.DeleteAsync(url + id).Result;

            var response = client.GetAsync(url + id).Result;
            var answer = response.Content.ReadAsStringAsync().Result;
            Assert.AreEqual("\"404 Not Found\"", answer);
        }
    }
}
