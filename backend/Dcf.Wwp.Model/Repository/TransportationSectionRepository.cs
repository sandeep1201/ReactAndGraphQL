using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;


namespace Dcf.Wwp.Model.Repository
{
    partial class Repository : ITransportationSectionRepository
    {
        public ITransportationSection NewTransportationSection(int participantId, string user)
        {
            var section = new TransportationSection
            {
                ParticipantId = participantId,
                ModifiedBy = user,
                ModifiedDate = DateTime.Now
            };

            _db.TransportationSections.Add(section);

            return section;
        }

        public IEnumerable<IDriversLicenseInvalidReasonType> DriversLicenseInvalidReasonTypes()
        {
            return from x in _db.DriversLicenseInvalidReasonTypes where !x.IsDeleted orderby x.SortOrder select x;
        }

        public IEnumerable<IDriversLicenseInvalidReasonType> AllDriversLicenseInvalidReasonTypes()
        {
            return from x in _db.DriversLicenseInvalidReasonTypes orderby x.SortOrder select x;
        }

        public IEnumerable<ITransportationType> TransportationTypes()
        {
            return from x in _db.TransportationTypes where !x.IsDeleted orderby x.SortOrder select x;
        }
    }
}
