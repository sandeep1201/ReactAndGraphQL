using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common.Logging;

namespace Dcf.Wwp.Api.Library.ViewModels
{
    public class PepViewModel
    {
        #region Properties

        private IRepository Repo   { get; }
        private ILog        Logger { get; }

        #endregion


        #region Methods

        public PepViewModel(IRepository repository)
        {
            Repo   = repository;
            Logger = LogProvider.GetLogger(GetType());
        }

        public List<EnrolledProgramContract> GetPepsByProgram(string pin, string programCode)
        {
            var part = Repo.GetParticipant(pin);

            if (part == null)
                throw new Exception("Invalid Pin");


            var peps = !programCode.IsNullOrEmpty()
                           ? part.ParticipantEnrolledPrograms.Where(x => x.EnrolledProgram?.ProgramCode.TrimAndLower() == programCode.TrimAndLower()).ToList()
                           : part.ParticipantEnrolledPrograms.ToList();

            var pepsContract = new List<EnrolledProgramContract>();

            peps.ForEach(pep =>
                         {
                             var pc = new EnrolledProgramContract(pep);
                             pepsContract.Add(pc);
                         });

            return pepsContract;
        }

        #endregion
    }
}
