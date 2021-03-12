using System;


namespace Dcf.Wwp.Model.Interface
{
    public interface ITransportationSectionMethodBridge : ICommonDelModel, ICloneable
    {
        int TransportationSectionId { get; set; }
        int? TransporationTypeId { get; set; }
        ITransportationType TransportationType { get; set; }
        ITransportationSection TransportationSection { get; set; }
    }
}
