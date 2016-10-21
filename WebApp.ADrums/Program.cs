using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace WebApp.ADrums
{
    public class Program
    {
        public const string APP_URL = "http://localhost:5001";
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(APP_URL)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            System.Diagnostics.Process.Start(APP_URL);

            host.Run();
        }
    }
}
