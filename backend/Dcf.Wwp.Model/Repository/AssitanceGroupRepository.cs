using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Dates;
using DCF.Common.Extensions;
using DCF.Timelimits.Rules.Domain;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IAssistanceGroupRepository
    {
        public IEnumerable<AssistanceGroupMember> ParticipantAssistanceGroupByPin(String pin)
        {
            Decimal                            pinDec;
            IEnumerable<AssistanceGroupMember> aGMemebrs = new List<AssistanceGroupMember>();
            if (Decimal.TryParse(pin, out pinDec))
            {
                aGMemebrs = this._doParticipantAssistanceGroupByPin(pin, DateTime.Now.StartOf(DateTimeUnit.Month), DateTime.Now.EndOf(DateTimeUnit.Month));
            }

            return aGMemebrs.ToList();
        }

        private IEnumerable<AssistanceGroupMember> _doParticipantAssistanceGroupByPin(string pin, DateTime beginDate, DateTime endDate)
        {
            return new List<AssistanceGroupMember>(this._db.SP_OtherParticipant(pin, beginDate, endDate, this.Database).ToList().Select(x => new AssistanceGroupMember()
                                                                                                                                             {
                                                                                                                                                 AGE                          = x.AGE,
                                                                                                                                                 SourcePinNumber              = x.PARTICIPANT,
                                                                                                                                                 ISINPLACEMENTPLACED          = x.ISINPLACEMENTPLACED,
                                                                                                                                                 BIRTH_DATE                   = x.BIRTH_DATE,
                                                                                                                                                 DEATH_DATE                   = x.DEATH_DATE,
                                                                                                                                                 ELIGIBILITY_PART_STATUS_CODE = x.ELIGIBILITY_PART_STATUS_CODE,
                                                                                                                                                 FIRST_NAME                   = x.FIRST_NAME,
                                                                                                                                                 GENDER                       = x.GENDER,
                                                                                                                                                 LAST_NAME                    = x.LAST_NAME,
                                                                                                                                                 MIDDLE_INITIAL_NAME          = x.MIDDLE_INITIAL_NAME,
                                                                                                                                                 PinNumber                    = x.OTHER_PARTICIPANT,
                                                                                                                                                 RELATIONSHIP                 = x.RELATIONSHIP,
                                                                                                                                                 EffectiveDateRange           = new DateTimeRange(x.PAYMENT_BEGIN_DATE, x.PAYMENT_END_DATE)
                                                                                                                                             }));
        }
    }
}
