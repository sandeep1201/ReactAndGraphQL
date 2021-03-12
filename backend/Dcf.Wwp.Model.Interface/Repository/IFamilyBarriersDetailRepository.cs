
namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IFamilyBarriersDetailRepository
    {
        IFamilyBarriersDetail NewFamilyBarriersDetail(IFamilyBarriersSection parentObject, string user);
        IFamilyBarriersDetail FamilBarrierDetailsByDetailId(int? id);
    }
}
