using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyBlazorInAction02.Server.Middleware
{
    public class RazorPageNotifierHub : Hub
    {
        public Task Reload()
            => Clients.Others.SendAsync("Reload");
    }

    public class RazorPageWatcherMiddleware
    {
        private List<string> ExtensionsToIgnore = new List<string> { ".dll", ".obj", ".pdb", ".exe", ".g.cs", };
        private List<string> ExtensionsToReBuild = new List<string> { ".cs", ".razor", ".razor.css" };
        private List<string> ExtensinosToReLoad = new List<string> { ".html", ".css", ".ts", "js" };

        private const string RELOAD = "reload";
        private const int TICK_INTERVAL = 5000000;

        private readonly RequestDelegate _next;
        private readonly IHubContext<RazorPageNotifierHub> _hubContext;
        private readonly ILogger<RazorPageWatcherMiddleware> _logger;

        private string _tempPath;
        private string _watchPath;
        private bool _reloadFiles;
        private long _lastRefresh;
        private readonly FileSystemWatcher watcher = new FileSystemWatcher();

        public RazorPageWatcherMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env,
            IHubContext<RazorPageNotifierHub> hubContext,
            ILogger<RazorPageWatcherMiddleware> logger)
        {
            _logger = logger;
            _next = next;
            _hubContext = hubContext;
            _tempPath = Path.GetFullPath("../Temp", env.ContentRootPath);
            _watchPath = Path.GetFullPath("../", env.ContentRootPath);
            _lastRefresh = DateTime.Now.Ticks;

            DirectoryInfo di;
#if DEBUG
            di = new DirectoryInfo(_tempPath);
#else
            di  = new DirectoryInfo("IGNORE");
#endif

            _reloadFiles = di.Exists;

            if (_reloadFiles)
            {
                _logger.LogInformation($"Watching under {_watchPath}");

                watcher = new FileSystemWatcher
                {
                    Path = _watchPath,
                    IncludeSubdirectories = true,
                    Filter = "*.*",
                    EnableRaisingEvents = true
                };

                watcher.Renamed += Watcher_Renamed;


            }
            else
            {
                _logger.LogInformation($"Not Watching files.....");
            }
        }

        public string RELOAD_CHANGE_FILE_NAME => RELOAD;

        private async void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            var refreshTicks = DateTime.Now.Ticks - _lastRefresh;
            if (!_reloadFiles || refreshTicks <= TICK_INTERVAL) return;

            if (ExtensionsToIgnore.Any(f => e.FullPath.EndsWith(f)))
            {
                _logger.LogDebug($"Ignoring File changed {e.FullPath}");
            }
            else if (ExtensionsToReBuild.Any(f => e.FullPath.EndsWith(f)))
            {
                _logger.LogInformation($"Rebuild File changed {e.FullPath}");

                await _hubContext.Clients.All.SendAsync("Reload");

                var reloadTriggerFile = new FileInfo(Path.Combine(
                    _tempPath,
                    $"{RELOAD_CHANGE_FILE_NAME}.{refreshTicks}"
                    ));

                WriteTempFile(reloadTriggerFile);
            }
            else if (ExtensinosToReLoad.Any(f => e.FullPath.EndsWith(f)))
            {
                var fi = new FileInfo(e.FullPath);
                if (fi.Exists)
                {
                    _logger.LogInformation("Sending reload command from Reload OnChange...");
                    await _hubContext.Clients.All.SendAsync("Reload");
                }
            }

            _lastRefresh = refreshTicks;
        }

        private void WriteTempFile(FileInfo reloadTriggerFile)
        {
            int iter = 5;
            do
            {
                try
                {
                    _logger.LogDebug("Writing Temp reload file...");
                    using (var tw = reloadTriggerFile.CreateText())
                    {
                        tw.WriteLine();
                        _reloadFiles = false;
                        return;
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                    _logger.LogWarning($"Error writing temp file, attempt-{iter}");
                }
            }
            while (iter-- >= 0);
            _logger.LogWarning($"Failed to write temp file...............");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}