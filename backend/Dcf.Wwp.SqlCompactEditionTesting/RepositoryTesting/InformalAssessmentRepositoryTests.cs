//using System.IO;
//using Dcf.Wwp.Data.Sql.Model;
//using Xunit;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using System.Linq;
//using Dcf.Wwp.Model.Interface;
//using System;

//namespace Dcf.Wwp.SqlCompactEditionTesting
//{
//    // This project can output the Class library as a NuGet Package.
//    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
//    public class InformalRepositoryTests : BaseUnitTest
//    {

//        [Fact]
//        public void UpsertInformalAssessment_NewAssessment_SuccessfulSave()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            var p = new Participant();

//            //Act-Controller
//            var ia = repo.NewInformalAssessment();
//            ia.Participant = p;
//            ia.ModifiedBy = "Dinesh";
//            repo.Save();

//            //Assert
//            var data = (from x in Db.InformalAssessments where x.Id == 1 select x).SingleOrDefault();
//            Assert.NotNull(data);
//        }

//        [Fact]
//        public void UpsertInformalAssessment_NewAssessmentWithNoParticipant_Error()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            IParticipant p = null;

//            //Act-Controller
//            var ia = repo.NewInformalAssessment();
//            ia.Participant = p;
//            ia.ModifiedBy = "Dinesh";
            
           
//            //Assert
//            Assert.Throws<System.Data.Entity.Infrastructure.DbUpdateException>(() => repo.Save());
//        }


//        [Fact]
//        public void UpsertInformalAssessment_VerifyVersionChange_Success()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            var p = new Participant();

//            //Act-Controller
//            var ia = repo.NewInformalAssessment();
//            ia.Participant = p;
//            ia.ModifiedBy = "Dinesh";
//            ia.CreatedDate = DateTime.Now;
//            repo.Save();
//            var data = (from y in Db.InformalAssessments                        
//                        where y.Id == 1
//                        select y).Single();

//            var time = data.RowVersion;

//            ia.Id = 1;
//            ia.ModifiedBy = "Sohinder";
//            ia.ParticipantId = 1;
//            ia.ModifiedDate = DateTime.Now;
//            repo.Save();

//            var data2 = (from y in Db.InformalAssessments
//                         where y.Id == 1
//                         select y).Single();
//            var time2 = data.RowVersion;
//            //Assert
//            Assert.False(time == time2);
//        }

//        [Fact]
//        public void GetInformalAssessment_ExistingAssessmentById_Success()
//        {
//            InformalAssessmentTestHelper.SetupAInformalAssessmentWithSingleParticipant(Db);
//            IRepository repo = new Repository(Db);
//            var ia = new InformalAssessment();
//            ia.ModifiedDate = DateTime.Now;
//            ia.ParticipantId = 1;
//            repo.InformalAssessmentById(1);
//            repo.Save();
//            //InformalAssesmentTestHelper.SetupAInformalAssessment(WwpContext);
//            var i = repo.InformalAssessmentById(1);
//            Assert.NotNull(i);            
//        }

//    }
//}


////    }
////}
