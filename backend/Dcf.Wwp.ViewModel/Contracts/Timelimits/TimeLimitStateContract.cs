using System;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Contracts.Timelimits;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class TimeLimitStateContract : BaseModelContract
    {
        public String Code { get; set; }
        public String Name { get; set; }
        public Int32? CountryId { get; set; }
    }
}