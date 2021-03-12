namespace Dcf.Wwp.Model.Interface.Core
{
    public interface IAuthUser
    {
        bool IsAuthenticated { get; }
        string Username { get; set; }
        string AgencyCode { get; set; }
    }
}