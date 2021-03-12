using System;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public class USP_GetLastWWOrLFInstance : IUSP_GetLastWWOrLFInstance
    {
        public string    ProgramCode               { get; set; }
        public DateTime? StatusDate                { get; set; }
        public string    WorkerId                  { get; set; } // represents either a W2 or a LF worker
        public bool?     ParticipantIsConfidential { get; set; }
    }
}
