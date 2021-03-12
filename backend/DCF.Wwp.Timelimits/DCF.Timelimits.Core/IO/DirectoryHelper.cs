using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.IO
{
    public static class DirectoryHelper
    {
        public static String CreateIfNotExists(String directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        public static IEnumerable<String> GetFiles(String path, String[] searchPatterns, SearchOption searchOption)
        {
            return
                searchPatterns.AsParallel().SelectMany(pattern => Directory.EnumerateFiles(path, pattern, searchOption));
        }
    }
}
