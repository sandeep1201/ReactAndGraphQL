using System;
using Dcf.Wwp.Model.Interface.Cww;

namespace Dcf.Wwp.Model.Cww
{
    public class Learnfare : ILearnfare
    {
        public string    FirstName       { get; set; }
        public string    LastName        { get; set; }
        public string    Middle          { get; set; }
        public DateTime? BirthDate       { get; set; }
        public string    LearnfareStatus { get; set; }
    }
}
