namespace Dcf.Wwp.Model.Interface
{
    public interface ITransportationType : ICommonDelModel
    {
        string Name                        { get; set; }
        int    SortOrder                   { get; set; }
        bool   DisablesOthersFlag          { get; set; }
        bool?  RequiresInsurance           { get; set; }
        bool?  RequiresCurrentRegistration { get; set; }
    }
}
