using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentModule : BaseEntity, IContentModule
    {
        ICollection<IContentModuleMeta> IContentModule.ContentModuleMetas
        {
            get { return this.ContentModuleMetas.Cast<IContentModuleMeta>().ToList(); }
            set { this.ContentModuleMetas = new List<ContentModuleMeta>(value.Cast<ContentModuleMeta>()); }
        }

        IContentPage IContentModule.ContentPage
        {
            get { return this.ContentPage; }
            set { this.ContentPage = value as ContentPage; }
        }

        ModuleStatus IContentModule.Status
        {
            get { return (ModuleStatus) this.Status; }
            set { this.Status = (Int32) value; }
        }
    }
}
