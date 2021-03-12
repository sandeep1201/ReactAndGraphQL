using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IEnrolledProgramStatusCode
    {
        int Id { get; set; }
        string StatusCode { get; set; }
        Nullable<int> SortOrder { get; set; }
        string ModifiedBy { get; set; }
        Nullable<System.DateTime> ModifiedDate { get; set; }
        byte[] RowVersion { get; set; }
        bool IsDeleted { get; set; }
    }
}
