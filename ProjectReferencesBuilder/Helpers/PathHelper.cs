using System.IO;

namespace ProjectReferencesBuilder.Helpers
{
    public static class PathHelper
    {
        public static string GetAbsolutePath(string path, string basePath)
        {
            return Path.GetFullPath(path.Replace('\\', '/'), basePath.Replace('\\', '/')).Replace('\\', '/');
        }
        public static string GetAbsolutePath(string path)
        {
            return Path.GetFullPath(path.Replace('\\', '/').Replace('\\', '/'));
        }
    }
}
