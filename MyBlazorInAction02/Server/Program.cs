using System;
using System.IO;
using System.Reflection;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MyBlazorInAction02.Server
{
    public class Program
    {
        public static Assembly HostAssy => Assembly.GetAssembly(typeof(Program));

        public static string ContentRoot()
        {
            var di = new DirectoryInfo(@$"..\Server");
            return di.FullName;
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseContentRoot(ContentRoot());
                    webBuilder.UseStaticWebAssets();
                    webBuilder.UseStartup<Startup>();
                })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }

    public static class ProgramExtensions
    {
        public static string HostAssyPath(this Assembly hostAssy)
        {
            return hostAssy.CallingAssyFile().Directory.FullName;
        }

        public static FileInfo CallingAssyFile(this Assembly hostAssy)
        {
            return new FileInfo(new Uri(hostAssy.Location).LocalPath);
        }

        public static string CallingAssyName(this Assembly hostAssy)
        {
            return hostAssy.CallingAssyFile().Name;
        }
    }
}
