namespace Dcf.Wwp.Model.Interface
{
    public interface IFinalistAddress
    {
        #region Properties

        string   AddressLine1 { get; set; }
        string   City         { get; set; }
        string   State        { get; set; }
        string   Zip          { get; set; }
        string   FullAddress  { get; set; }
        string[] ErrorMsg     { get; set; }
        bool     IsValid      { get; set; }

        #endregion

        #region Methods

        #endregion
    }
}
