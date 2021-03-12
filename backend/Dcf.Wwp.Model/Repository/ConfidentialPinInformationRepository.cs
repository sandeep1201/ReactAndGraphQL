using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        #region Properties

        #endregion

        #region Methods

        public IConfidentialPinInformation NewConfidentialPinInformation(IParticipant participant)
        {
            // check with chris if the following could ever happen
            // decimal? pin = null;
            // if (participant.PinNumber.HasValue && participant.PinNumber != 0)
            // {
            //     pin = participant.PinNumber;
            // }

            var newRow = new ConfidentialPinInformation() as IConfidentialPinInformation;
            newRow.Participant  = participant;
            newRow.PinNumber    = participant.PinNumber;
            newRow.ModifiedDate = DateTime.Now;
            newRow.ModifiedBy   = _authUser.Username;
            _db.ConfidentialPinInformations.Add((ConfidentialPinInformation) newRow);

            return (newRow);
        }

        public ICollection<IConfidentialPinInformation> GetConfidentialPinInformation(int participantId)
        {
            var r = _db.ConfidentialPinInformations.Where(i => i.ParticipantId == participantId).Cast<IConfidentialPinInformation>().ToList();

            return (r);
        }

        public bool HasConfidentialInfomation(decimal pin)
        {
            var r = _db.ConfidentialPinInformations.Any(i => i.Participant.PinNumber == pin && i.IsConfidential == true && i.IsDeleted == false);

            return (r);
        }

        #endregion
    }
}
