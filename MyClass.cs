using System;
using RestSharp;
using System.Text.Json;
using NUnit.Framework;

namespace kpi
{

    public class IdParse
    {
        public string id { get; set; }
        public string error_summary { get; set; }

    }
    [TestFixture]
    class MyPostman
    {
        [Test]
        public void Test1()
        {
            var t = new MyPostman();
            string upl = t.Upload();
            IdParse my =
               JsonSerializer.Deserialize<IdParse>(upl);
            string id_full = my.id;
            int result_f = 0;
            int result = id_full.Length;
            if (result > 1)
            {
                result_f = 1;
            }

            Assert.AreEqual(result_f, 1);
        }
        [Test]
        public void Test2()
        {
            var t = new MyPostman();
            string upl = t.Upload();
            IdParse my =
               JsonSerializer.Deserialize<IdParse>(upl);
            string id_full = my.id;
            string get_md = t.Get_file_metadata(id_full);
            int result_f = 0;
            int result = get_md.Length;
            if (result > 1)
            {
                result_f = 1;
            }

            Assert.AreEqual(result_f, 1);
        }
        [Test]
        public void Test3()
        {
            var t = new MyPostman();
            string u = t.Upload();

            string del = t.Delete();
            int result_f = 0;
            int result = del.Length;
            if (result > 1)
            {
                result_f = 1;
            }

            Assert.AreEqual(result_f, 1);
        }
        [Test]
        public void Test4()
        {
            var t = new MyPostman();
            string upl = t.Upload();
            string del = t.Delete();
            IdParse my =
               JsonSerializer.Deserialize<IdParse>(upl);
            string id_full = my.id;
            string get_md = t.Get_file_metadata(id_full);
            IdParse my2 =
          JsonSerializer.Deserialize<IdParse>(get_md);

            int result_f = 0;
            int result = my2.error_summary.Length;
            if (result > 1)
            {
                result_f = 1;
            }

            Assert.AreEqual(result_f, 1);
        }





        public string Upload()
        {
            var client = new RestClient("https://content.dropboxapi.com/2/files/upload");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Dropbox-API-Arg", "{\"path\": \"/NEWfile.txt\",\"mode\": \"add\",\"autorename\": true,\"mute\": false,\"strict_conflict\": false}");
            request.AddHeader("Content-Type", "application/octet-stream");
            request.AddHeader("Authorization", "Bearer cg5ZEE2ibAcAAAAAAAAAAVUnkICYj4ZnV9KFlQvY_AmSVJln_5obcfqbRfMTOLPb");
            var body = @"hello woÏrld!";
            request.AddParameter("application/octet-stream", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return response.Content;
        }
        public string Get_file_metadata(string id)
        {

            var client = new RestClient("https://api.dropboxapi.com/2/sharing/get_file_metadata");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer cg5ZEE2ibAcAAAAAAAAAAVUnkICYj4ZnV9KFlQvY_AmSVJln_5obcfqbRfMTOLPb");
            var body = @"{" + "\n" +
            $@"    ""file"" : ""{id}""," + "\n" +
            @"    ""actions"" : []" + "\n" +
            @"}";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return response.Content;
        }
        public string Delete()
        {
            var client = new RestClient("https://api.dropboxapi.com/2/files/delete_v2");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer cg5ZEE2ibAcAAAAAAAAAAVUnkICYj4ZnV9KFlQvY_AmSVJln_5obcfqbRfMTOLPb");
            var body = @"" + "\n" +
            @"{" + "\n" +
            @"    ""path"": ""/NEWfile.txt""" + "\n" +
            @"}" + "\n" +
            @"";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return response.Content;

        }









    }


    
}