using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace NewEnvisioBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureKestrel(options => options.ListenLocalhost(7925));
                    // For running in Railway
                    var portVar = Environment.GetEnvironmentVariable("PORT");
                    if (portVar is { Length: > 0 } && int.TryParse(portVar, out int port))
                    {
                        webBuilder.ConfigureKestrel(options =>
                        {
                            options.ListenAnyIP(port);
                        });
                    }
                });
    }
}
