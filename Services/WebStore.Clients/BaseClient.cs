using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients
{
    public abstract class BaseClient : IDisposable
    {
        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _Disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_Disposed)
            {
                if (disposing)
                {
                    //Free managed objects
                }

                _Client.Dispose();

                _Disposed = true;
            }
        }

        ~BaseClient()
        {
            Dispose(false);
        }

        #endregion

        protected readonly HttpClient _Client;

        protected readonly string _ServiceAddress;

        protected BaseClient(IConfiguration config, string serviceAddress)
        {
            _ServiceAddress = serviceAddress;

            _Client = new HttpClient()
            {
                BaseAddress = new Uri(config["WebApiUrl"])
            };

            var headers = _Client.DefaultRequestHeaders.Accept;
            headers.Clear();
            headers.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        }

        protected T Get<T>(string url)
            where T : new() 
            => GetAsync<T>(url).Result;


        protected async Task<T> GetAsync<T>(string url, CancellationToken token = default)
            where T : new()
        {
            var response = await _Client.GetAsync(url, token);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<T>();
            else
                return (T)(object)null;
        }

        protected HttpResponseMessage Post<T>(string url, T source)
            where T : new() 
            => PostAsync(url, source).Result;

        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T source, CancellationToken token = default)
            where T : new()
        {
            var response = await _Client.PostAsJsonAsync(url, source, token);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Put<T>(string url, T source)
            where T : new()
            => PutAsync(url, source).Result;

        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T source, CancellationToken token = default)
            where T : new()
        {
            var response = await _Client.PutAsJsonAsync(url, source, token);

            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url)
            => DeleteAsync(url).Result;

        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken token = default)
        {
            var response = await _Client.DeleteAsync(url, token);

            return response.EnsureSuccessStatusCode();
        }
    }
}
