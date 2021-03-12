using System;
namespace DCF.Core.Modules
{
    public interface IApplicationModuleManager
    {
        void Initialize(Type startupModule);
        void ShutdownModules();
        void StartModules();
        IApplicationModuleInfo StartupModule { get; }
    }
}
