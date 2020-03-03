using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace WebStore
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateWebHostBuilder(args)
               .Build()
               .Run();

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseUrls("http://0.0.0.0:8080")
                //.ConfigureLogging((host, log) =>
                //{
                //    log.AddFilter("Microsoft", level => level > LogLevel.Information);
                //    log.ClearProviders();
                //    log.AddConsole(opt => opt.IncludeScopes = true);
                //    log.AddDebug();
                //})
                .UseStartup<Startup>();
    }
}
