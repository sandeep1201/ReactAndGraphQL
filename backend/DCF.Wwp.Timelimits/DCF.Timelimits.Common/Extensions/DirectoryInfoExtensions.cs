using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core
{
    public static class DirectoryInfoExtensions
    {
            public static IEnumerable<String> GetMatchingFileNames(this DirectoryInfo dirInfo, params String[] patterns)
            {
                var matchingFilesNames = new List<String>();

                foreach (String pattern in patterns)
                {
                    var matchingFiles = dirInfo.GetFiles(pattern);
                    matchingFilesNames.AddRange(matchingFiles.Select(f => f.FullName));
                }

                return matchingFilesNames.Distinct();
            }
    }
}
