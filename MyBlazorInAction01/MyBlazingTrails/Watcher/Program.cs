using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Watcher
{
    public class Program
    {
        public static Assembly HostAssy => Assembly.GetAssembly(typeof(Program));

        public static readonly string HostAssyTemp =
            Path.GetFullPath(Path.Combine(
                HostAssy.HostAssyPath().Directory.FullName,
                "../../../../",
                "Temp"));


        private static readonly string ServerProjectName = @"MyBlazorInAction02.Server";
        private static readonly string ServerProjectPath = 
            Path.GetFullPath(Path.Combine(
                HostAssy.HostAssyPath().Directory.FullName,
                "../../../../",
                "Server"));

        private static readonly string NetCoreAppTargetFrameWork = "net6.0";
        private static readonly string DllPath = Path.Combine(ServerProjectPath, $@"bin\Debug\{NetCoreAppTargetFrameWork}\{ServerProjectName}.dll");

        public static void Main(string[] args)
        {
            _ = new DirectoryInfo(ServerProjectPath).Exists ? true :
                throw new DirectoryNotFoundException(ServerProjectPath);

            var hb = new HostBuilder()
                      .UseContentRoot(ServerProjectPath)
                      .ConfigureLogging(logging =>
                      {
                          logging.AddConsole()
                                 .AddFilter("Watcher", LogLevel.Debug)
                                 .SetMinimumLevel(LogLevel.Warning);
                      })
                      .ConfigureServices(services =>
                      {
                          services.AddHostedService<WatcherService>();
                          services.AddSingleton<HostingServer>();

                          services.Configure<ProjectOptions>(o =>
                          {
                              o.ProjectName = ServerProjectName;
                              o.ProjectPath = ServerProjectPath;
                              o.DllPath = DllPath;
                              o.DotNetPath = DotNetMuxer.MuxerPathOrDefault();
                              o.Args = args;
                              o.TempPath = HostAssyTemp;
                          });
                      })
                      .ConfigureWebHostDefaults(webBuilder =>
                      {
                          webBuilder.Configure(app =>
                          {
                              app.UseDeveloperExceptionPage();

                              var server = app.ApplicationServices.GetRequiredService<HostingServer>();

                              app.Run(async context =>
                              {
                                  var application = await server.WaitForApplicationAsync(default);

                                  await application(context);
                              });
                          });
                      })
                      .Build();

            hb.Run();

        }

        private static void CleanTempFolder(ProjectOptions projectOptions)
        {
            var di = new DirectoryInfo(Path.Combine(projectOptions.TempPath, "temp"));
            if (di.Exists)
            {
                di.Delete();
            }
        }
    }
}