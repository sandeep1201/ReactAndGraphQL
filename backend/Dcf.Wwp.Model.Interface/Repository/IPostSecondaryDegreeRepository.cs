
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IPostSecondaryDegreeRepository
    {
        IPostSecondaryDegree NewDegree(IPostSecondaryEducationSection parentSection, string user);
    }
}
