using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IAgeCategoryRepository
    {
        int AgeCategoryByName(string name);
        IEnumerable<IAgeCategory> AllAgeCategories();
    }
}
