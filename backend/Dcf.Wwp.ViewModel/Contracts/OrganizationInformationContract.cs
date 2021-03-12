using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class OrganizationInformationContract
    {
        public int                            Id               { get; set; }
        public int                            ProgramId        { get; set; }
        public string                         ProgramName      { get; set; }
        public int                            OrganizationId   { get; set; }
        public string                         OrganizationName { get; set; }
        public List<FinalistLocationContract> Locations        { get; set; }
        public string                         ModifiedBy       { get; set; }
        public DateTime                       ModifiedDate     { get; set; }
    }

    public class FinalistLocationContract
    {
        public int                     Id              { get; set; }
        public FinalistAddressContract FinalistAddress { get; set; }
        public string                EffectiveDate   { get; set; }
        public string               EndDate         { get; set; }
    }
}
