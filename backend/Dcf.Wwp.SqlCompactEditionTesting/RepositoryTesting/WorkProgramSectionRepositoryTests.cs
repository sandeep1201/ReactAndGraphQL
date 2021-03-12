using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dcf.Wwp.SqlCompactEditionTesting.RepositoryTesting
{
    public class WorkProgramSectionRepositoryTests : BaseUnitTest
    {
        [Fact]
        public void UpsertWorkProgramSection_NewSection_SuccessfulSave()
        {
            //Arrange
            IRepository repo = new Repository(Db);
            var p = new Participant();
            Db.Participants.Add(p);
            var ia = new InformalAssessment();
            ia.Participant = p;
            Db.InformalAssessments.Add(ia);


            //Act-Controller
            var wps = repo.NewWorkProgramSection(ia);
            wps.IsInOtherPrograms = true;
            wps.Notes = "Testing";
            wps.ModifiedBy = "Sohinder";
            wps.ModifiedDate = DateTime.Now;
            repo.Save();
            var data = (from x in Db.WorkProgramSections where x.Id == 1 select x).SingleOrDefault();

            //Assert
            Assert.NotNull(data);
        }

        [Fact]
        public void UpsertWorkProgramSection_ExistingSection_SuccessfulUpdate()
        {
            /// SETUP           
            IRepository repo = new Repository(Db);
            var p = new Participant();
            Db.Participants.Add(p);
            var ia = new InformalAssessment();
            ia.Participant = p;
            Db.InformalAssessments.Add(ia);


            //Act-Controller
            var wps = repo.NewWorkProgramSection(ia);
            wps.IsInOtherPrograms = true;
            wps.Notes = "Testing";
            wps.ModifiedBy = "Sohinder";
            wps.ModifiedDate = DateTime.Now;
            repo.Save();
            var data = (from x in Db.WorkProgramSections where x.Id == 1 select x).SingleOrDefault();
            var time1 = data.RowVersion;
            //Act-Controller

            wps.IsInOtherPrograms = false;
            wps.Notes = "Testing";
            wps.ModifiedBy = "Dinesh";
            wps.ModifiedDate = DateTime.Now;
            repo.Save();
            var data1 = (from x in Db.WorkProgramSections where x.Id == 1 select x).SingleOrDefault();
            var time2 = data1.RowVersion;

            //Assert
            Assert.False(time1 == time2);
        }
    }
}
