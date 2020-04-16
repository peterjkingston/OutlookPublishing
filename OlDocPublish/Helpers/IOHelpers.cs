using System;
using System.IO;

namespace Helpers.IO
{
    public static class Paths
    {
        public static string GetParentDirectoryName(this string path)
        {
            string[] dirs = path.Split(Path.DirectorySeparatorChar);
            return dirs[dirs.Length -1];
        }
    }
}