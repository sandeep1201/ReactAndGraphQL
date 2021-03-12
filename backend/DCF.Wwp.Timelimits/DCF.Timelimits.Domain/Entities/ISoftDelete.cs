using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCF.Core.Domain.Entities
{
    public interface ISoftDelete
    {
        /// <summary>
        /// Used to mark an Entity as 'Deleted'. 
        /// </summary>
        Boolean IsDeleted { get; set; }
    }
}
