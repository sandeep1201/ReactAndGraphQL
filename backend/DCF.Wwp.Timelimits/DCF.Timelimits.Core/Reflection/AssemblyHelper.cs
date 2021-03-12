using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DCF.Core.IO;

namespace DCF.Core.Reflection
{
    public static class AssemblyHelper
    {

        public static IEnumerable<String> GetAllAssembliesInFolder(String folderPath, SearchOption searchOption)
        {
            var assemblies = DirectoryHelper.GetFiles(folderPath, new String[] {"*.dll", "*.exe"}, searchOption);
            return assemblies;
        }
        
    }
}
