using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Services
{
    public interface IContentService : IDisposable
    {
        IContentPage GetContentPage(String pageSlug);
        IContentPage GetContentPage(Int32 id);
        IEnumerable<IContentModuleMeta> UpsertContentModuleMetas(IEnumerable<IContentModuleMeta> models);
        IEnumerable<IContentModule> UpsertContentModules(IEnumerable<IContentModule> models);
        IContentPage UpsertContentPage(IContentPage model);
        IContentModule GetContentModule(Int32 id);
        IContentModuleMeta GetContentModuleMeta(Int32 id);
    }
}