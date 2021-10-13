using System;
using System.IO;
using System.Reflection;

namespace Watcher
{
    public static class ProgramExtensions
    {
        public static string HostAssyName(this Assembly hostAssy) => new FileInfo(new Uri(hostAssy?.Location ?? "").LocalPath).Name;

        public static FileInfo HostAssyPath(this Assembly hostAssy) => new FileInfo(new Uri(hostAssy?.Location ?? "").LocalPath);

    }
}