using Dcf.Wwp.Api.Library.Interfaces;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class PostSecondaryCollegeContract : BaseRepeaterContract, IIsEmpty
    {
        public string Name { get; set; }

        public LocationContract Location { get; set; }

        public bool? HasGraduated { get; set; }

        public int? LastYearAttended { get; set; }

        public bool? IsCurrentlyAttending { get; set; }

        public int? Semesters { get; set; }

        public decimal? Credits { get; set; }

        public string Details { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return !HasGraduated.HasValue &&
                   string.IsNullOrWhiteSpace(Name) &&
                   !LastYearAttended.HasValue &&
                   !IsCurrentlyAttending.HasValue &&
                   !Semesters.HasValue &&
                   !Credits.HasValue &&
                   string.IsNullOrWhiteSpace(Details) &&
                   (Location == null || Location.IsEmpty());
        }

        #endregion IIsEmpty
    }
}
