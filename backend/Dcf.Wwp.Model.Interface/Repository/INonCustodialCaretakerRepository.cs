namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface INonCustodialCaretakerRepository
    {
        INonCustodialCaretaker NewNonCustodialCaretaker(INonCustodialParentsSection section, string user);
    }
}