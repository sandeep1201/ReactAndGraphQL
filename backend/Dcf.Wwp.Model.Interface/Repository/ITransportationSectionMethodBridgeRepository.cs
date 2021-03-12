namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface ITransportationSectionMethodBridgeRepository
    {
        ITransportationSectionMethodBridge NewTransportationSectionMethodBridge(ITransportationSection parent, string user);
    }
}