//using Dcf.Wwp.Data.Sql.Model;
//using Xunit;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using System.Linq;
//using Dcf.Wwp.Model.Interface;
//using System;

//namespace Dcf.Wwp.SqlCompactEditionTesting.RepositoryTesting
//{
//    public class PostSecondaryEducationRepositoryTest : BaseUnitTest
//    {
//        [Fact]
//        public void UpsertPostSecondaryEducationSection_NewSection_SuccessfulSave()
//        {
//            //Arrange
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);


//            //Act-Controller
//            var pse = repo.NewPostSecondaryEducationSection(ia);
//            pse.DidAttendCollege = true;
//            pse.IsWorkingOnLicensesOrCertificates = true;
//            pse.DoesHaveDegrees = true;
//            pse.Notes = "Graduated";
//            pse.ModifiedBy = "Sohinder";
//            pse.ModifiedDate = DateTime.Now;
//            repo.Save();
//            var data = (from x in Db.PostSecondaryEducationSections where x.Id == 1 select x).SingleOrDefault();
   
//            //Assert
//            Assert.NotNull(data);
//        }

//        [Fact]
//        public void UpsertPostSecondaryEducationSection_ExistingSection_SuccessfulUpdate()
//        {
//            /// SETUP           
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);

            
//            //Act-Controller
//            var pse = repo.NewPostSecondaryEducationSection(ia);
//            pse.DidAttendCollege = true;
//            pse.IsWorkingOnLicensesOrCertificates = true;
//            pse.DoesHaveDegrees = true;
//            pse.Notes = "Graduated";
//            pse.ModifiedBy = "Sohinder";
//            pse.ModifiedDate = DateTime.Now;
//            repo.Save();
//            var data = (from x in Db.PostSecondaryEducationSections where x.Id == 1 select x).SingleOrDefault();
//            var time1 = data.RowVersion;
//            //Act-Controller
           
//            pse.DidAttendCollege = true;
//            pse.IsWorkingOnLicensesOrCertificates = true;
//            pse.DoesHaveDegrees = true;
//            pse.Notes = "Graduated";
//            pse.ModifiedBy = "Dinesh";
//            pse.ModifiedDate = DateTime.Now;
//            repo.Save();
//            var data1 = (from x in Db.PostSecondaryEducationSections where x.Id == 1 select x).SingleOrDefault();          
//            var time2 = data1.RowVersion;
            
//            //Assert
//            Assert.False(time1 == time2);
//        }

//    }
//}
