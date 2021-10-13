namespace Watcher
{
    public class ProjectOptions
    {
        public string[] Args { get; set; }
        public object ProjectName { get; set; }
        public string ProjectPath { get; set; }
        public object ClientProjectName { get; set; }
        public string ClientProjectPath { get; set; }

        public string DllPath { get; set; }
        public string DotNetPath { get; set; }

        public string TempPath { get; set; }
    }
}