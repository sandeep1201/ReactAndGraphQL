
//using System.IO;
//using Dcf.Wwp.Data.Sql.Model;
//using Xunit;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using System.Linq;
//using Dcf.Wwp.Model.Interface;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;

//namespace Dcf.Wwp.SqlCompactEditionTesting.RepositoryTesting
//{
//    public class EducationSectionRepositoryTest : BaseUnitTest
//    {

//        [Fact]
//        public void UpsertInformalAssessment_NewAssessment_SuccessfulSave()
//        {
//            /// Arrange
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);

//            //Act-Controller
//            var es = repo.NewEducationSection(ia);
//            es.IsCurrentlyEnrolled = true;
//            es.ModifiedBy = "Sohinder";
//           // ls.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();

//            //Assert
//            var data = (from x in Db.EducationSections where x.Id == 1 select x).SingleOrDefault();
//            Assert.NotNull(data);
//        }
    
//        [Fact]
//        public void UpsertEducationSection_ExistingSection_SuccessfulUpdate()
//        {
//            /// Arrange           
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);
//            var lgc = new SchoolGradeLevel();
//            Db.SchoolGradeLevels.Add(lgc);
//            var sgc = new SchoolGraduationStatus();
//            Db.SchoolGraduationStatus1.Add(sgc);
//            var st = new State();
//            Db.States.Add(st);
//            var s = new School();
//            Db.Schools.Add(s);
//            var cia = new CertificateIssuingAuthority();
//            cia.SortOrder = 1;
//            Db.CertificateIssuingAuthorities.Add(cia);


//            //Act-Controller
//            var es = repo.NewEducationSection(ia);
//            es.IsCurrentlyEnrolled = true;
//            es.LastGradeLevelCompleted = lgc;
//            es.SchoolGraduationStatus = sgc;
//            es.School = s;
//            es.CertificateIssuingAuthority = cia;
//            es.IsCurrentlyEnrolled = true;
//            es.ModifiedBy = "Sohinder";
//            repo.Save();
//            var data1 = (from x in Db.EducationSections where x.Id == 1 select x).Single();
//            var time1 = data1.RowVersion;
//            es.IsCurrentlyEnrolled = false;
//            es.ModifiedBy = "Dinesh";
//            repo.Save();
//            var data2 = (from x in Db.EducationSections where x.Id == 1 select x).Single();
//            var time2 = data2.RowVersion;

//            //Assert
//            Assert.False(time1 == time2);

//        }

//        [Fact]
//        public void GetSchool_VerifyingBySchoolCityandStreetLookup_SaveWithExistingSchoolNameCityandStreetSuccess()
//        {
//            //Arrange
//            IRepository repo = new Repository(Db);
//            EducationSectionTestHelper.SetupSchool_State_GradStatus_Gradelevel(Db);
//            //Act
//            var data3 = repo.SchoolByNameCityState("Francis","Md","WI");
//            //Assert
//            Assert.NotNull(data3);

//        }

//        [Fact]
//        public void UpsertEducationSection_NewAssessmentWithNullObject_Success()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            IInformalAssessment ia = null;

//            //Act-Controller

//            //Assert
//            Assert.Throws<System.NullReferenceException>(() => repo.NewEducationSection(ia));
//        }

//    }
//}
