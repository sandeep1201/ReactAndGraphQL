using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DCF.Timelimits.Rules.Scripting
{
    /// <summary>
    ///     Intent: Get referenced assemblies, either recursively or flat. Not thread safe, if running in a multi
    ///     threaded environment must use locks.
    /// </summary>
    public static class AssemblyHelper
    {
        public class MissingAssembly
        {
            public MissingAssembly(string missingAssemblyName, string missingAssemblyNameParent)
            {
                this.MissingAssemblyName = missingAssemblyName;
                this.MissingAssemblyNameParent = missingAssemblyNameParent;
            }

            public string MissingAssemblyName { get; set; }
            public string MissingAssemblyNameParent { get; set; }
        }

        private static Dictionary<string, Assembly> _dependentAssemblyList;
        private static List<MissingAssembly> _missingAssemblyList;

        /// <summary>
        ///     Intent: Get assemblies referenced by entry assembly. Not recursive.
        /// </summary>
        public static List<string> MyGetReferencedAssembliesFlat(this Type type)
        {
            var results = type.Assembly.GetReferencedAssemblies();
            return results.Select(o => o.FullName).OrderBy(o => o).ToList();
        }

        /// <summary>
        ///     Intent: Get assemblies currently dependent on entry assembly. Recursive.
        /// </summary>
        public static Dictionary<string, Assembly> MyGetReferencedAssembliesRecursive(this Assembly assembly)
        {
            AssemblyHelper._dependentAssemblyList = new Dictionary<string, Assembly>();
            AssemblyHelper._missingAssemblyList = new List<MissingAssembly>();

            AssemblyHelper.InternalGetDependentAssembliesRecursive(assembly);

            // Only include assemblies that we wrote ourselves (ignore ones from GAC).
            var keysToRemove = AssemblyHelper._dependentAssemblyList.Values.Where(
                o => o.GlobalAssemblyCache == true).ToList();

            foreach (var k in keysToRemove)
            {
                AssemblyHelper._dependentAssemblyList.Remove(k.FullName.MyToName());
            }

            return AssemblyHelper._dependentAssemblyList;
        }

        /// <summary>
        ///     Intent: Get missing assemblies.
        /// </summary>
        public static List<MissingAssembly> MyGetMissingAssembliesRecursive(this Assembly assembly)
        {
            AssemblyHelper._dependentAssemblyList = new Dictionary<string, Assembly>();
            AssemblyHelper._missingAssemblyList = new List<MissingAssembly>();
            AssemblyHelper.InternalGetDependentAssembliesRecursive(assembly);

            return AssemblyHelper._missingAssemblyList;
        }

        /// <summary>
        ///     Intent: Internal recursive class to get all dependent assemblies, and all dependent assemblies of
        ///     dependent assemblies, etc.
        /// </summary>
        private static void InternalGetDependentAssembliesRecursive(Assembly assembly)
        {
            // Load assemblies with newest versions first. Omitting the ordering results in false positives on
            // _missingAssemblyList.
            var referencedAssemblies = assembly.GetReferencedAssemblies()
                .OrderByDescending(o => o.Version);

            foreach (var r in referencedAssemblies)
            {
                if (String.IsNullOrEmpty(assembly.FullName))
                {
                    continue;
                }

                if (AssemblyHelper._dependentAssemblyList.ContainsKey(r.FullName.MyToName()) == false)
                {
                    try
                    {
                        var a = Assembly.ReflectionOnlyLoad(r.FullName);
                        AssemblyHelper._dependentAssemblyList[a.FullName.MyToName()] = a;
                        AssemblyHelper.InternalGetDependentAssembliesRecursive(a);
                    }
                    catch (Exception ex)
                    {
                        AssemblyHelper._missingAssemblyList.Add(new MissingAssembly(r.FullName.Split(',')[0], assembly.FullName.MyToName()));
                    }
                }
            }
        }

        private static string MyToName(this string fullName)
        {
            return fullName.Split(',')[0];
        }
    }
}