using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dcf.Wwp.Api.Library.ViewModels.ContactsApp
{
    public class ContactsViewModel : BasePinViewModel
    {
        public ContactsViewModel(IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
        }

        public IEnumerable<ContactContract> GetParticipantContacts()
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var contacts = new List<ContactContract>();

            foreach (var x in Participant.Contacts)
            {
                var c = new ContactContract
                {
                    Id = x.Id,
                    Name = x.Name.NullStringToBlank(),
                    TitleTypeId = x.TitleId,
                    TitleTypeName = x.ContactTitleType?.Name.SafeTrim(),
                    CustomTitle = x.CustomTitle,
                    Email = x.Email.NullStringToBlank(),
                    PhoneNumber = x.Phone.NullStringToBlank(),
                    PhoneExt = x.ExtensionNo.NullStringToBlank(),
                    FaxNumber = x.FaxNo.NullStringToBlank(),
                    Address = x.Address.NullStringToBlank(),
                    Notes = x.Notes,
                    RoiSignedDate = x.ReleaseInformationDate?.ToString("MM/dd/yyyy"),
                    RowVersion = x.RowVersion
                };

                contacts.Add(c);
            }

            return contacts;
        }


        public ContactContract GetParticipantContact(int id)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            ContactContract contact = null;

            // Return a list of all the contacts the match the pin.  We'll do this
            // so we have a layer of protection that the API user isn't getting a contact
            // for a participant they don't have access to

            var c = (from x in Participant.Contacts where x.Id == id select x).SingleOrDefault();

            if (c != null)
            {
                contact = new ContactContract
                {
                    Id = c.Id,
                    Name = c.Name.NullStringToBlank(),
                    TitleTypeId = c.TitleId,
                    TitleTypeName = c.ContactTitleType?.Name.SafeTrim(),
                    CustomTitle = c.CustomTitle,
                    Email = c.Email.NullStringToBlank(),
                    PhoneNumber = c.Phone.NullStringToBlank(),
                    PhoneExt = c.ExtensionNo.NullStringToBlank(),
                    FaxNumber = c.FaxNo.NullStringToBlank(),
                    Address = c.Address.NullStringToBlank(),
                    Notes = c.Notes,
                    RoiSignedDate = c.ReleaseInformationDate?.ToString("MM/dd/yyyy"),
                    RowVersion = c.RowVersion,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedDate = c.ModifiedDate,
                    ModifiedByName = Repo.GetWorkerNameByWamsId(c.ModifiedBy)
                };
                if (contact.RoiSignedDate != null)
                {
                    contact.IsRoiSigned = true;
                }

            }

            return contact;
        }

        public bool DeleteContact(int id, string user)
        {
            if (Participant == null)
            {
                throw new InvalidOperationException("Participant is null.");
            }

            var isDeleted = Repo.DeleteContact(id, user);
            return isDeleted;
        }

        public UpsertResponse<IContact> UpsertData(ContactContract model, string pin, int id, string user)
        {
            var response = new UpsertResponse<IContact>();

            var p = Repo.GetParticipant(pin);

            if (p == null)
                throw new InvalidOperationException("Pin not valid.");

            if (model == null)
                throw new InvalidOperationException("Contact data is missing.");

            IContact ic = null;
            //repo.ContactById(model.Contacts)
            //var returnList = new List<int>();

            // New Contacts.
            if (model.TitleTypeId != null && model.Name != null)
            {
                if (id != 0)
                {
                    // Updating Contacts.
                    ic = Repo.ContactById(id);
                }
                else
                {
                    ic = Repo.NewContact(p, user);
                }

                Repo.StartChangeTracking(ic);
                // TODO: This should be the Involved Work progrm ID.
                //if (c.InvolvedWorkProgramId != 0)
                //	ic.i = c.InvolvedWorkProgramId;                
                ic.Name = model.Name;
                ic.TitleId = model.TitleTypeId;
                IContactTitleType ctt = null;
                ctt = Repo.ContactTitleById(ic.TitleId);
                if (ctt.Name == "Other")
                {
                    ic.CustomTitle = model.CustomTitle;
                }
                else
                {
                    ic.CustomTitle = null;
                }
                ic.Email = model.Email;
                ic.Phone = model.PhoneNumber;
                ic.ExtensionNo = model.PhoneExt;
                ic.FaxNo = model.FaxNumber;
                ic.Address = model.Address;
                ic.Notes = model.Notes;
                if (model.IsRoiSigned.HasValue && model.IsRoiSigned.Value)
                {
                    ic.ReleaseInformationDate = model.RoiSignedDate.ToDateTimeMonthDayYear();
                }
                else
                {
                    ic.ReleaseInformationDate = null;
                }

                response.UpdatedModel = ic;

                // Do a concurrency check.
                if (!Repo.IsRowVersionStillCurrent(ic, model.RowVersion))
                {
                    response.HasConcurrencyError = true;
                }
            }

            Repo.SaveIfChanged(ic, user);
            return response;
        }
    }
}
