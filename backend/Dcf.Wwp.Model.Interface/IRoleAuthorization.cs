namespace Dcf.Wwp.Model.Interface
{
    public interface IRoleAuthorization : ICommonDelModel
    {
        int RoleId { get; set; }
        int AuthorizationId { get; set; }

        IAuthorization Authorization { get; set; }
    }
}