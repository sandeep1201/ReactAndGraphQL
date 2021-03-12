
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IPostSecondaryCollegeRepository 
    {        
        IPostSecondaryCollege NewCollege(IPostSecondaryEducationSection parentSection, string user);
    }
}
