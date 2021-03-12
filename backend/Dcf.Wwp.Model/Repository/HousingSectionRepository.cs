using System;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IHousingSectionRepository
    {
        public ICurrentAddressDetails CwwCurrentAddressDetails(string pin)
        {
            var cad = new Cww.CurrentAddressDetails();

            if (string.IsNullOrWhiteSpace(pin))
                return null;

            var rentPaid = _db.SP_RentPaid(pin, Database).FirstOrDefault();

            cad.ShelterAmount = rentPaid?.SHELTER_PAY_AMT;
            //if (!String.IsNullOrWhiteSpace(rentPaid?.SHELTER_PAY_AMT))
            //	cad.ShelterAmount = Convert.ToDecimal(rentPaid.SHELTER_PAY_AMT);

            var caseAddress = _db.SP_CaseAddress(pin, Database).FirstOrDefault();

            if (caseAddress != null)
            {
                cad.Address = $"{caseAddress.LINE_1_ADDRESS?.Trim()} {caseAddress.LINE_2_ADDRESS?.Trim()}";
                cad.City    = caseAddress.CITY_ADR?.Trim();
                cad.State   = caseAddress.STATE_ADR?.Trim();
                cad.Zip     = caseAddress.ZIP_ADR?.ToString();
            }

            string yymm       = DateTime.Now.ToString("yyyyMM");
            var    subHousing = _db.SP_SubsidizedHousing(pin, yymm, Database).FirstOrDefault();

            if (subHousing != null)
            {
                cad.IsSubsidized = subHousing.SUBSD_HSE_CD?.Trim().ToUpper() == "Y";
            }

            return cad;
        }

        public IHousingSection NewHousingSection(IParticipant parentParticipant, string user)
        {
            var section = new HousingSection
                          {
                              ModifiedDate  = DateTime.Now,
                              ModifiedBy    = user,
                              ParticipantId = parentParticipant.Id
                          };
            _db.HousingSections.Add(section);

            return section;
        }
    }
}
