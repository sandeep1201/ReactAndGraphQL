using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Logging;
using DCF.Core.Collections;

namespace DCF.Core.Reflection
{
    public class TypeFinder : ITypeFinder
    {
        public ILogger Logger { get; set; }

        private readonly IAssemblyFinder _assemblyFinder;
        private readonly Object _syncObj = new Object();
        private Type[] _types;

        public TypeFinder(IAssemblyFinder assemblyFinder)
        {
            this._assemblyFinder = assemblyFinder;
            this.Logger = NullLogger.Instance;
        }

        public Type[] Find(Func<Type, Boolean> predicate)
        {
            return this.GetAllTypes().Where(predicate).ToArray();
        }

        public Type[] FindAll()
        {
            return this.GetAllTypes().ToArray();
        }

        private Type[] GetAllTypes()
        {
            if (this._types == null)
            {
                lock (this._syncObj)
                {
                    if (this._types == null)
                    {
                        this._types = this.CreateTypeList().ToArray();
                    }
                }
            }

            return this._types;
        }

        private List<Type> CreateTypeList()
        {
            var allTypes = new List<Type>();

            var assemblies = this._assemblyFinder.GetAllAssemblies().Distinct();

            foreach (var assembly in assemblies)
            {
                try
                {
                    Type[] typesInThisAssembly;

                    try
                    {
                        typesInThisAssembly = assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        typesInThisAssembly = ex.Types;
                    }

                    if (typesInThisAssembly.IsNullOrEmpty())
                    {
                        continue;
                    }

                    allTypes.AddRange(typesInThisAssembly.Where(type => type != null));
                }
                catch (Exception ex)
                {
                    this.Logger.Warn(ex.ToString(), ex);
                }
            }

            return allTypes;
        }
    }
}