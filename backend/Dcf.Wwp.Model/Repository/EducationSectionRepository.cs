using System;
using System.Data.Entity.Infrastructure;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Data.Sql.Model;

namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository : IEducationSectionRepository
    {
        public IEducationSection NewEducationSection(IParticipant parentParticipant, string user)
        {
            var section = new EducationSection();
            section.ModifiedDate = DateTime.Now;
            section.ModifiedBy = user;
            section.ParticipantId = parentParticipant.Id;
            _db.EducationSections.Add(section);
            return section;
        }

        public bool HasEducationSectionChanged(IEducationSection educationSection)
        {
            var objectStateManager = ((IObjectContextAdapter)_db).ObjectContext.ObjectStateManager;

            var stateEntry = objectStateManager.GetObjectStateEntry(educationSection);
            var currentValues = stateEntry.CurrentValues;
            var originalValues = stateEntry.OriginalValues;
            var modifiedProperties = stateEntry.GetModifiedProperties();
            foreach (string modifiedProperty in modifiedProperties)
            {
                var currentValue = currentValues.GetValue(currentValues.GetOrdinal(modifiedProperty));
                var originalValue = originalValues.GetValue(originalValues.GetOrdinal(modifiedProperty));
                if (originalValue.Equals(currentValue))
                {
                    // Value not changed
                    return false;
                }
                else
                {
                    // Value changed
                    return true;
                }
            }

            return false;
        }
    }

}
