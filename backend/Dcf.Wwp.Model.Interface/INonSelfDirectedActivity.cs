using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface INonSelfDirectedActivity : ICommonModelFinal, ICloneable
    {
        int       Id            { get; set; }
        int       ActivityId    { get; set; }
        string    BusinessName  { get; set; }
        int?      CityId        { get; set; }
        decimal?  PhoneNumber   { get; set; }
        string    StreetAddress { get; set; }
        string    ZipAddress    { get; set; }
        IActivity Activity      { get; set; }
        ICity     City          { get; set; }
    }
}
