using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IOrganization : ICommonDelModel
    {
        string                     EntsecAgencyCode { get; set; }
        string                     AgencyName       { get; set; }
        string                     DB2AgencyName    { get; set; }
        DateTime?                  ActivatedDate    { get; set; }
        DateTime?                  InActivatedDate  { get; set; }
        ICollection<IContractArea> ContractAreas    { get; set; }
        ICollection<IWorker>       Workers          { get; set; }
    }
}
