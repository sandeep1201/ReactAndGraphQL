using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dcf.Wwp.Model.Interface
{
    public interface IDeleteContact
    {
        Int64 ID { get; set; }
        Int32? Return_Value { get; set; }
    }
}
