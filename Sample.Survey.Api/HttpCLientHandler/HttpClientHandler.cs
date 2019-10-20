using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sample.Survey.Api
{
    public class HttpHandler
    {

        public string BaseUrl { get; set; }

        private HttpClient _httpclient => CreateHttpClient();



        public virtual HttpResponseMessage Get(string url)
        {
            return GetAsync(url).Result;
        }

        public virtual HttpResponseMessage Post(string url, HttpContent content)
        {
            return PostAsync(url, content).Result;
        }

        public virtual async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _httpclient.GetAsync(url);
        }

        public virtual async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _httpclient.PostAsync(url, content);
        }

        private HttpClient CreateHttpClient()
        {
            if (string.IsNullOrEmpty(this.BaseUrl))
            {
                return new HttpClient();
            }
            else
            {
                var client = new HttpClient();
                client.BaseAddress = new System.Uri(this.BaseUrl);
                return client;
            }
        }
    }
}
