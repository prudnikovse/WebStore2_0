using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Infrastructure.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _Next;
        private readonly ILogger<ErrorHandlingMiddleware> _Logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _Next = next;
            _Logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _Next(context);
            }
            catch(Exception ex)
            {
                await HandleException(context, ex);
                throw;
            }           
        }

        private Task HandleException(HttpContext context, Exception exception)
        {
            _Logger.LogError(exception, "Ошибка при обработке запроса {0}", context.Request.Path);
            return Task.CompletedTask;
        }
    }
}
