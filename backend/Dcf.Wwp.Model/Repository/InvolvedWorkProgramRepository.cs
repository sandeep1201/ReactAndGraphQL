using System;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Model.Repository
{
    public partial class Repository
    {
        public IInvolvedWorkProgram NewInvolvedWorkProgram(IWorkProgramSection parentSection, string user)
        {
            IInvolvedWorkProgram iw = new InvolvedWorkProgram();

            iw.WorkProgramSection = parentSection;
            iw.ModifiedDate = DateTime.Now;
            iw.ModifiedBy = user;
            _db.InvolvedWorkPrograms.Add((InvolvedWorkProgram)iw);

            return iw;
        }

        public void DeleteWorkProgram(IInvolvedWorkProgram involvedWorkProgram)
        {
            _db.InvolvedWorkPrograms.Remove(involvedWorkProgram as InvolvedWorkProgram);
        }


        public void AppendInvolvedWorkProgram(IInvolvedWorkProgram iw)
        {
            _db.InvolvedWorkPrograms.Add((InvolvedWorkProgram)iw);
        }
    }
}
