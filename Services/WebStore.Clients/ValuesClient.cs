using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebStore.Domain.Consts;
using WebStore.Interfaces.Api;

namespace WebStore.Clients
{
    public class ValuesClient : BaseClient, IValuesService
    {
        public ValuesClient(IConfiguration config) 
            : base(config, WebApiConsts.Values)
        {
        }

        public HttpStatusCode Delete(int id) => DeleteAsync(id).Result;

        public async Task<HttpStatusCode> DeleteAsync(int id) => (await _Client.DeleteAsync($"{_ServiceAddress}/{id}")).StatusCode;

        public IEnumerable<string> Get() => GetAsync().Result;

        public string Get(int id) => GetAsync(id).Result;

        public async Task<IEnumerable<string>> GetAsync()
        {
            var response = await _Client.GetAsync(_ServiceAddress);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<string>>();
            else
                return Array.Empty<string>();
        }

        public async Task<string> GetAsync(int id)
        {
            var response = await _Client.GetAsync($"{_ServiceAddress}/{id}");

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<string>();
            else
                return string.Empty;
        }

        public Uri Post(string value) => PostAsync(value).Result;

        public async Task<Uri> PostAsync(string value)
        {
            var response = await _Client.PostAsJsonAsync($"{_ServiceAddress}/post", value);
            return response.EnsureSuccessStatusCode().Headers.Location;
        }

        public HttpStatusCode Put(int id, string value) => PutAsync(id, value).Result;

        public async Task<HttpStatusCode> PutAsync(int id, string value)
        {
            var response = await _Client.PutAsJsonAsync($"{_ServiceAddress}/put", value);
            return response.EnsureSuccessStatusCode().StatusCode;
        }
    }
}
