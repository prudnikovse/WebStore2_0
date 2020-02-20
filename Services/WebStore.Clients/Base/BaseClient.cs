using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace WebStore.Clients.Base
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
    }
}
