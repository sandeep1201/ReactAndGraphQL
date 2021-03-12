using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class FinalistAddressContract : IFinalistAddress, IIsEmpty
    {
        public string   AddressLine1        { get; set; }
        public string   City                { get; set; }
        public string   State               { get; set; }
        public string   Zip                 { get; set; }
        public string   FullAddress         { get; set; }
        public string[] ErrorMsg            { get; set; }
        public bool     IsValid             { get; set; }
        public bool     UseSuggestedAddress { get; set; }
        public bool     UseEnteredAddress   { get; set; }

        #region IIsEmpty

        public bool IsEmpty()
        {
            // This logic needs to match the expected experience for the user.
            return string.IsNullOrWhiteSpace(AddressLine1) &&
                   string.IsNullOrWhiteSpace(City)         &&
                   string.IsNullOrWhiteSpace(State);
        }

        #endregion
    }
}
