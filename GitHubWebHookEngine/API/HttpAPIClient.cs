using System.Net.Http.Json;

namespace GitHubWebHookEngine.API
{
    public class HttpAPIClient : IHttpAPIClient
    {
        public HttpClient _Client { get; }

        public HttpAPIClient(HttpClient httpClient)
        {
            if(httpClient == null)
            {
                return;
            }

            if(httpClient.BaseAddress == null)
            {
                httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("GitHubAPIUrl"));
            }

            httpClient.DefaultRequestHeaders.UserAgent.Add(new System.Net.Http.Headers.ProductInfoHeaderValue("AppName", "1.0"));
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            //Require bespoke media type for topics whice are in preview mode
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.github.mercy-preview+json"));

            if (httpClient.DefaultRequestHeaders?.Authorization == null)
            {
                httpClient.DefaultRequestHeaders.Authorization = httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", Environment.GetEnvironmentVariable("PrivateToken"));
            }

            _Client = httpClient;
        }

        public async Task<string> Get(string url)
        {
            return await _Client.GetStringAsync(url);
        }

        public async Task<string> Put(string url, string[] names)
        {
            using (var client = _Client)
            {
                var namesRoot = new
                {
                    names = names
                };

                var response = client.PutAsJsonAsync(url, namesRoot).Result;

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
