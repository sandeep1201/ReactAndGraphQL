using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentModuleMeta : BaseEntity, IContentModuleMeta
    {
        IContentModule IContentModuleMeta.ContentModule
        {
            get { return this.ContentModule; }
            set { this.ContentModule = value as ContentModule; }
        }
    }
}
