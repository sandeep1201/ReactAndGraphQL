using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dcf.Wwp.Api.Library.Contracts;
using FluentValidation;

namespace Dcf.Wwp.Api.Library.Validators
{
   public class HousingHistoriesValidator:AbstractValidator<HousingHistoryContract>
    {
       public HousingHistoriesValidator()
       {
           
       }

    }
}
