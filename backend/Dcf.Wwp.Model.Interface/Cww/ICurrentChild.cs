using System;


namespace Dcf.Wwp.Model.Interface.Cww
{
    public interface ICurrentChild
    {
        string Pin { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Middle { get; set; }
        DateTime? BirthDate { get; set; }
        DateTime? DeathDate { get; set; }
        string Gender { get; set; }
        string Relationship { get; set; }
        int? Age { get; set; }
    }
}