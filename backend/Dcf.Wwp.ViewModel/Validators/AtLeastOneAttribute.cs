using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Dcf.Wwp.Api.Library.Validators
{
    public class AtLeastOneAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is IList list)
            {
                return list.Count > 0;
            }

            return false;
        }
    }
}
