using System;


namespace Dcf.Wwp.Model.Interface
{
    public interface IAssessmentType
    {
        int Id { get; set; }
        string Name { get; set; }
        string ModifiedBy { get; set; }
        DateTime ModifiedDate { get; set; }
        string Code { get; set; }
    }
}
