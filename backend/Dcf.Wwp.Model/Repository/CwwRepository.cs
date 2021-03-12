using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Model.Cww;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Cww;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : ICwwRepository
    {
        public List<ICurrentChild> CwwCurrentChildren(string pin)
        {
            var results = _db.SP_ParticpantsChildrenFromCARES(pin, Database).ToList();
            var db2HighDate = new DateTime(9999, 12, 31);

            //var list = new List<ICurrentChild>();

            //foreach (var pcrt in results)
            //{
            //	var cc = new CurrentChild();
            //	cc.Age = pcrt.AGE;
            //	cc.BirthDate = pcrt.DOB_DT;
            //	cc.Pin = pcrt.SOURCE_PIN_NUM.ToString();

            //	list.Add(cc);
            //}

            //return list;
            return
                results.Select(
                    pc =>
                        new CurrentChild
                        {
                            Age = pc.AGE,
                            BirthDate = pc.DOB_DT,
                            DeathDate = pc.DEATH_DT,
                            FirstName = pc.FIRST_NAM.Trim(),
                            LastName = pc.LAST_NAM.Trim(),
                            Pin = pc.SOURCE_PIN_NUM.ToString(),
                            Gender = pc.GENDER,
                            Middle = pc.MIDDLE_INITIAL_NAM.Trim(),
                            Relationship = pc.RELATIONSHIP
                        })
                    .Where(x => !x.DeathDate.HasValue || x.DeathDate.Value == db2HighDate)
                    .Cast<ICurrentChild>()
                    .ToList();
        }

        public ISP_CWWChildCareEligibiltyStatus_Result CwwChildCareEligibiltyStatus(string pin)
        {
            return _db.SP_CWWChildCareEligibiltyStatus(pin, Database).SingleOrDefault();
        }

        public List<ILearnfare> CwwLearnfare(string pin)
        {
            var results = _db.SP_LearnFareStatus(pin, Database).ToList();
            var db2HighDate = new DateTime(9999, 12, 31);

            //return list;
            return results.Select(pc => new Learnfare() { BirthDate = pc.DOB_DT, FirstName = pc.FIRST_NAM?.Trim(), LastName = pc.LAST_NAM?.Trim(), Middle = pc.MIDDLE_INITIAL_NAM?.Trim(), LearnfareStatus = pc.LEARN_FARE_STATUS?.Trim() }).Cast<ILearnfare>().ToList();
        }

        public List<ISocialSecurityStatus> CwwSocialSecurityStatus(string pin)
        {
            var results = _db.SP_SocialSecurityStatus(pin, Database).ToList();

            // Result List.
            return
                results.Select(
                    pc =>
                        new SocialSecurityStatus()
                        {
                            Participant = pc.PARTICIPANT.ToString(),
                            FirstName = pc.FIRST_NAM.Trim(),
                            Middle = pc.MIDDLE_INITIAL_NAM.Trim(),
                            LastName = pc.LAST_NAM.Trim(),
                            Dob = pc.DOB_DT,
                            Relationship = pc.REL_CD.Trim(),
                            Age = pc.AGE.Trim(),
                            FedSsi = pc.FED_SSI.Trim(),
                            StateSsi = pc.STATE_SSI.Trim(),
                            Ssa = pc.SSA.Trim()
                        }).Cast<ISocialSecurityStatus>().ToList();
        }
    }
}