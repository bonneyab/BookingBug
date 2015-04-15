using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Core;
using Newtonsoft.Json.Linq;

namespace BookingBugBookingIntegration
{
    public class BookingBugClient : IBookingBugClient
    {
        //Todo: Consider introducing a layer of abstraction for the configuration manager.
        //TODO: Consider a constants file for the keys? They're only really used one place, abstraction for the config manager might settle this issue.
        //TODO: I've got a lot of strings running around in this file and the api file. I think I'm ok with that...but it does feel a bit awk.
        private HttpClient _client;
        private readonly string _email = ConfigurationManager.AppSettings["UserName"] ;
        private readonly string _password = ConfigurationManager.AppSettings["Password"];
        private readonly string _appId = ConfigurationManager.AppSettings["AppId"];
        private readonly string _appKey = ConfigurationManager.AppSettings["AppKey"];
        private readonly string _baseUrl = ConfigurationManager.AppSettings["BaseUrl"];

        //it's annoying that this gets called right away by the dependency injection framework.
        public BookingBugClient()
        {
            Authenticate();
        }

        public JObject GetJson(string targetUrl)
        {
            var response = _client.GetAsync(targetUrl).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to retreive Booking Bug data due to: " + response.ReasonPhrase);
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return json;
        }

        public JObject PostData(string targetUrl, Dictionary<string, string> item)
        {
            var content = new FormUrlEncodedContent(item.ToArray());
            var response = _client.PostAsync(targetUrl, content).Result;
            if (!response.IsSuccessStatusCode)
                throw new Exception("Unable to post Booking Bug data due to: " + response.ReasonPhrase);
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);
            return json;
        }

        public List<JObject> GetPagedJson(string target)
        {
            var hasPages = true;
            var page = 1;
            var jsonData = new List<JObject>();
            while (hasPages)
            {
                //yuck
                target = target.Replace("?page=" + (page - 1), "?page=" + page);
                target = target.Replace("&page=" + (page - 1), "&page=" + page);
                var response = _client.GetAsync(target).Result;
                if (!response.IsSuccessStatusCode)
                    throw new Exception("Unable to retreive data from Booking Bug due to: " + response.ReasonPhrase);
                var bookings = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                //Sometimes next goes back to page = 0
                if (bookings["_links"]["next"] == null || bookings["_links"]["next"].ToString().Contains("?page=0&"))
                {
                    hasPages = false;
                }
                page++;
                jsonData.Add(bookings);
            }

            return jsonData;
        }

        public void Authenticate()
        {
            _client = new HttpClient {BaseAddress = new Uri(_baseUrl)};
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("App-Id", _appId);
            _client.DefaultRequestHeaders.Add("App-Key", _appKey);

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("email", _email),
                new KeyValuePair<string, string>("password", _password),
            });

            var response = _client.PostAsync("api/v1/login", content).Result;
            if (!response.IsSuccessStatusCode) throw new Exception("Unable to authenticate with Booking Bug");
            var json = JObject.Parse(response.Content.ReadAsStringAsync().Result);

            _client.DefaultRequestHeaders.Add("Auth-Token", json.Value<string>("auth_token"));
        }
    }

    public interface IBookingBugClient : IDependency
    {
        JObject GetJson(string targetUrl);
        List<JObject> GetPagedJson(string target);
        JObject PostData(string targetUrl, Dictionary<string, string> item);
    }
}