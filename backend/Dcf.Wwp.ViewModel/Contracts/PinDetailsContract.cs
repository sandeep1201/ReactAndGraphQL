using System.Collections.Generic;
using System;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PinDetailsContract
    {
        // This forces us to use the Create factory method.
        private PinDetailsContract()
        {
        }

        public int? OfficeTransferId { get; set; }
        public short? OfficeTransferNumber { get; set; }
        public BasicInfoContract BasicInfo { get; set; }
        public AddressContract AddressInfo { get; set; }
        public OfficeCountyContract OfficeCountyInfo { get; set; }
        public W2EligibilityContract W2EligibilityInfo { get; set; }
        public EnrolledProgramContract EnrolledProgramInfo { get; set; }
        public List<RelatedPersonContract> RelatedPersons { get; set; }
        public OtherDemographicInformationContract OtherDemographicInformation { get; set; }
        public OfficeTransferContract CwwTransferDetails { get; set; }
        public SP_MostRecentFEPFromDB2_Result MostRecentFEPFromDB2_Result { get; set; }


        public static PinDetailsContract Create(BasicInfoContract basicInfo, AddressContract addressInfo,
                OfficeCountyContract officeCountyInfo, W2EligibilityContract w2EligibilityInfo,
                EnrolledProgramContract enrolledProgramInfo, List<RelatedPersonContract> relatedPersons, OfficeTransferContract cwwTransferDetails, SP_MostRecentFEPFromDB2_Result mostRecentFEPFromDB2_Result, int? officeTransferId = null, short? officeTransferNumber = null)
        {
            return new PinDetailsContract
            {
                OfficeTransferId = officeTransferId,
                OfficeTransferNumber = officeTransferNumber,
                BasicInfo = basicInfo,
                AddressInfo = addressInfo,
                OfficeCountyInfo = officeCountyInfo,
                W2EligibilityInfo = w2EligibilityInfo,
                EnrolledProgramInfo = enrolledProgramInfo,
                RelatedPersons = relatedPersons,
                CwwTransferDetails = cwwTransferDetails,
                MostRecentFEPFromDB2_Result = mostRecentFEPFromDB2_Result
            };
        }
    }
}
