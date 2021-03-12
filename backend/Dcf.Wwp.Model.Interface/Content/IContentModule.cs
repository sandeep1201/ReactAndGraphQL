using System;
using System.Collections.Generic;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Model.Interface
{
    public interface IContentModule : ICommonDelCreatedModel
    {
        String Name { get; set; }
        String Title { get; set; }
        Boolean ShowTitle { get; set; }
        String Description { get; set; }
        Boolean ShowDescription { get; set; }
        ModuleStatus Status { get; set; }
        Int32 SortOrder { get; set; }
        Nullable<Int32> ContentPageId { get; set; }
        IContentPage ContentPage { get; set; }
        ICollection<IContentModuleMeta> ContentModuleMetas { get; set; }
    }
}