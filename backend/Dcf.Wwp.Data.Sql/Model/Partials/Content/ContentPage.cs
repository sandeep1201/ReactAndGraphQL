using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class ContentPage : BaseEntity, IContentPage
    {
        ICollection<IContentModule> IContentPage.ContentModules
        {
            get { return this.ContentModules.Cast<IContentModule>().ToList(); }

            set { this.ContentModules = value.Cast<ContentModule>().ToList(); }
        }
    }
}
