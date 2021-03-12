using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IContentModuleMeta : ICommonDelCreatedModel
    {
        String Name { get; set; }
        String Data { get; set; }
        Int32? ContentModuleId { get; set; }
        IContentModule ContentModule { get; set; }
    }
}