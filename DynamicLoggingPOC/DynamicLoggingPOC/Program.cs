using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.Extensions.Logging;
using Steeltoe.Extensions.Logging.SerilogDynamicLogger;

namespace DynamicLoggingPOC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.SetBasePath(builderContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddCloudFoundry();
                })
                .ConfigureLogging((builderContext, loggingBuilder) =>
                    {
                        loggingBuilder.AddConfiguration(builderContext.Configuration.GetSection("Logging"));
                        loggingBuilder.AddSerilogDynamicConsole();
                    })
                .UseStartup<Startup>();
    }
}