using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;
using DCF.Core.Modules;
using DCF.Core.Plugins;
using EnsureThat;

namespace DCF.Core.Container
{
    /// <summary>
    /// Implements the discovery of assemblies containing manifests
    /// that represent plug-ins.  Also responsible for loading a 
    /// plug-ins types and modules.  
    /// 
    /// Having this component load the plug-in types decouples the AppContainer 
    /// from .NET assemblies and makes the design loosely coupled and easy to unit-test.
    /// </summary>
    public class TypeResolver : ITypeResolver
    {
        private readonly String[] _searchPatterns;

        protected TypeResolver()
        {

        }

        public TypeResolver(String[] searchPatterns)
        {
            Ensure.That(searchPatterns, nameof(searchPatterns)).IsNotNull();
            this._searchPatterns = searchPatterns;
        }

        public String[] SearchPatterns
        {
            get { return this._searchPatterns; }
        }

        public virtual void SetManifests(IManifestRegistry registry)
        {
            Ensure.That(registry, nameof(registry)).IsNotNull().WithExtraMessageOf(p=> "registry not specified");

            Assembly[] pluginAssemblies = this.GetPluginAssemblies(this._searchPatterns);
            this.SetManifestTypes(registry, pluginAssemblies);
        }

        private Assembly[] GetPluginAssemblies(String[] searchPatterns)
        {
            DirectoryInfo probeDirectory = this.GetAssemblyProbeDirectory();
            AssemblyName[] filteredAssemblyNames = this.ProbeForMatchingAssemblyNames(probeDirectory, searchPatterns);

            Assembly[] assemblies = this.LoadAssemblies(filteredAssemblyNames).ToArray();

            try
            {
                return assemblies.Where(a => a.GetTypes()
                    .Any(t => t.IsDerivedFrom<IPluginManifest>())).ToArray();
            }
            catch (ReflectionTypeLoadException ex)
            {
                var loadErrors = ex.LoaderExceptions.Select(le => le.Message).Distinct().ToList();
                throw new ContainerException("Error loading plug-in assembly.", loadErrors, ex);
            }
        }

        protected void SetManifestTypes(IManifestRegistry registry, Assembly[] pluginAssemblies)
        {
            IEnumerable<Type> pluginTypes = pluginAssemblies.SelectMany(pa => pa.GetTypes());
            registry.AllManifests = pluginTypes.CreateMatchingInstances<IPluginManifest>().ToList();

            foreach (IPluginManifest manifest in registry.AllManifests)
            {
                Assembly assembly = manifest.GetType().Assembly;
                manifest.AssemblyName = assembly.FullName;
                manifest.AssemblyVersion = assembly.GetName().Version.ToString();
                manifest.MachineName = Environment.MachineName;
            }
        }

        private DirectoryInfo GetAssemblyProbeDirectory()
        {
            return new DirectoryInfo(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory);
        }

        protected AssemblyName[] ProbeForMatchingAssemblyNames(DirectoryInfo probeDirectory, String[] searchPatterns)
        {
            IEnumerable<String> fileNames = probeDirectory.GetMatchingFileNames(searchPatterns);
            return fileNames.Select(AssemblyName.GetAssemblyName).ToArray();
        }

        private Assembly[] GetLoadedMatchingAssemblies(AssemblyName[] matchingAssemblyNames)
        {
            IEnumerable<String> matchingAssemblyCodeBases = matchingAssemblyNames.Select(an => an.CodeBase);

            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => matchingAssemblyCodeBases.Contains(a.CodeBase, StringComparer.Ordinal))
                .Where(a => a != this.GetType().Assembly) // Excluded this assembly which contains testing types.
                .ToArray();
        }

        protected IEnumerable<Assembly> LoadAssemblies(IEnumerable<AssemblyName> assemblyNames)
        {
            var loadedAssemblies = new List<Assembly>();

            foreach (AssemblyName assemblyName in assemblyNames)
            {
                Assembly assembly = null;
                try
                {
                    assembly = Assembly.Load(assemblyName);
                    loadedAssemblies.Add(assembly);
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var loadErrors = ex.LoaderExceptions.Select(le => le.Message).Distinct().ToList();
                    throw new ContainerException("Error loading plug-in assembly.", loadErrors, ex);
                }
                catch (Exception ex)
                {
                    throw new ContainerException(
                        $"Error loading assembly: {assemblyName.CodeBase}",
                        ex);
                }
            }
            return loadedAssemblies;
        }

        public virtual void SetPluginTypes(IPlugin plugin)
        {
            Ensure.That(plugin, nameof(plugin)).IsNotNull().WithExtraMessageOf(p=>"plug-in not specified");

            Assembly pluginAssembly = plugin.Manifest.GetType().Assembly;
            plugin.PluginTypes = pluginAssembly.GetTypes()
                .Select(t => new PluginType(plugin, t, pluginAssembly.GetName().Name))
                .ToArray();
        }

        public void SetPluginModules(IPlugin plugin)
        {
            Ensure.That(plugin, nameof(plugin)).IsNotNull().WithExtraMessageOf(p =>"plug-in not specified");

            if (plugin.PluginTypes == null)
            {
                throw new InvalidOperationException(
                    "Plug-in types must loaded before modules can be discovered.");
            }
            plugin.AppModules = plugin.PluginTypes.CreateMatchingInstances<IApplicationModule>().ToArray();
        }

        // Automatically populates all properties on a plug-in module that are an enumeration of
        // a derived IPluginKnownType.  The plug-in known types specific to the module are returned
        // for use by the consumer for logging what types a module discovered.
        public IEnumerable<Type> SetDiscoverTypes(IApplicationModule forModule, IEnumerable<IPluginType> fromPluginTypes)
        {
            Ensure.That(forModule, nameof(forModule)).IsNotNull().WithExtraMessageOf(p=>"module to discover known types not specified");
            Ensure.That(fromPluginTypes, nameof(fromPluginTypes)).IsNotNull().WithExtraMessageOf(p=>"list of plug-in types not specified");

            IEnumerable<PropertyInfo> knownTypeProps = this.GetKnownTypeProperties(forModule);
            knownTypeProps.ForEach(ktp => this.SetKnownPropertyInstances(forModule, ktp, fromPluginTypes));
            return knownTypeProps.Select(ktp => ktp.PropertyType.GenericTypeArguments.First());
        }

        private IEnumerable<PropertyInfo> GetKnownTypeProperties(IApplicationModule module)
        {
            BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            return module.GetType().GetProperties(bindings)
                .Where(p =>
                    p.PropertyType.IsClosedGenericTypeOf(typeof(IEnumerable<>), typeof(IKnownPluginType))
                    && p.CanWrite);
        }

        private void SetKnownPropertyInstances(IApplicationModule forModule, PropertyInfo KnownTypeProperty,
           IEnumerable<IPluginType> fromPluginTypes)
        {
            var knownType = KnownTypeProperty.PropertyType.GetGenericArguments().First();
            var discoveredInstances = fromPluginTypes.CreateMatchingInstances(knownType).ToList();

            // Set the module property to the collection of objects matching its derived known type.
            ArrayList list = new ArrayList(discoveredInstances.ToArray());
            Array newArray = list.ToArray(knownType);

            KnownTypeProperty.SetValue(forModule, newArray);
        }

        public String GetResourceAsText(IPlugin plugin, String resourceName)
        {
            var pluginAssembly = plugin.Manifest.GetType().Assembly;

            using (Stream stream = pluginAssembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
