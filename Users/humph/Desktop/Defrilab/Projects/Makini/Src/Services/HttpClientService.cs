using Newtonsoft.Json; 
using System.Net.Http.Headers; 
using Makini.Src.Constants;

namespace Makini.Src.Services
{
    class HttpClientService 
    {
        private protected HttpClient _httpClient;
        private protected HttpClientHandler _handler;

        public HttpClientService()
        {
            _handler = new HttpClientHandler();
            _handler.AllowAutoRedirect = false;
            _handler.Credentials = default;
            _httpClient = new HttpClient(_handler);
            _httpClient.BaseAddress = new Uri(HttpClientServiceConstants.BaseUri);
        }

        public HttpClientService(string token)
        {
            _handler = new HttpClientHandler();
            _handler.AllowAutoRedirect = false;
            _handler.Credentials = default;
            _httpClient = new HttpClient(_handler);
            _httpClient.BaseAddress = new Uri(HttpClientServiceConstants.BaseUri);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<string> Get(string theUri)
        {
            var response = await _httpClient.GetAsync(theUri).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync();
           
            return content;
        }

        public async Task<string> Post(object objectToSend, string theUri)
        {
            var jsonDataUser = JsonConvert.SerializeObject(objectToSend);
            var httpContent = new StringContent(jsonDataUser);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PostAsync(theUri, httpContent).ConfigureAwait(false);
           
            return response.Content.ReadAsStringAsync().Result;
        }

        public  async Task<string> UpDate(object objectToUpdate, string theUri)
        {
            var jsonDataUser = JsonConvert.SerializeObject(objectToUpdate);
            var httpcontent = new StringContent(jsonDataUser);
            httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _httpClient.PutAsync(theUri, httpcontent);
           
            return response.StatusCode.ToString();
        }

        public async Task<string> Delete(string theUri)
        {
            var result = await _httpClient.DeleteAsync(theUri);

            return result.StatusCode.ToString();
        }       
    }
 }
