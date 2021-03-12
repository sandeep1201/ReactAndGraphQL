using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface IUSP_GetLastWWOrLFInstance
    {
        string    ProgramCode               { get; set; }
        DateTime? StatusDate                { get; set; }
        string    WorkerId                  { get; set; }
        bool?     ParticipantIsConfidential { get; set; }
    }
}
