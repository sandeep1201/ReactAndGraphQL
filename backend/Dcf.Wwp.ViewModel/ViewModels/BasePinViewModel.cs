using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public abstract class BasePinViewModel : BaseViewModel
    {
        protected BasePinViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser) { }

        public string Pin { get; set; }

        public IParticipant Participant { get; set; }

        public bool IsPinValid => (Participant != null);

        public void InitializeFromPin(string pin)
        {
            // We may get a pin that contains leading 0's.
            Pin = pin.TrimStart('0');

            // Use the value in the table by default.  If it doesn't exist, then call
            // the stored procedure to refresh the participant table.
            var result = Repo.GetParticipant(pin) ?? Repo.GetRefreshedParticipant(pin);

            if (result == null) return;

            Participant = result;
            OnParticipantLoaded();
        }

        protected virtual void OnParticipantLoaded() { }
    }
}
