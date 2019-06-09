
using log4net;
using log4net.Config;
using ManagerLogbook.Services.Providers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;

namespace ManagerLogbook.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Log4Net Configuration
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            var fileInfo = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config");
            XmlConfigurator.Configure(logRepository, fileInfo);

            var host = BuildWebHost(args);
            await SeedData.SeedDatabase(host);
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
