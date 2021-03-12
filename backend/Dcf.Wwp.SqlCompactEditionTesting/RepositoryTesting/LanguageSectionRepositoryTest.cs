//using System.IO;
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
//    public class LanguageSectionRepositoryTest : BaseUnitTest
//    {
//        //public WwpEntities WwpContext { get; private set; }

//        [Fact]
//        public void UpsertLanguageSection_NewSection_SuccessfulSave()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            p.ModifiedDate = DateTime.Now;
//            p.ModifiedBy = "Dinesh";
//            p.CreatedDate = DateTime.Now;        
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;

//            // ia.ParticipantId = 1;
//            Db.InformalAssessments.Add(ia);

//            //Act-Controller
//            var ls = repo.NewLanguageSection(ia);
//            ls.InterpreterNotes = "Good at apeaking English";
//            ls.IsNeedingInterpreter = true;
//            ls.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();

//            //Assert
//            var data = (from x in Db.LanguageSections where x.Id == 1 select x).SingleOrDefault();
//            Assert.NotNull(data);
//        }

//        [Fact]
//        public void UpsertLanguageSection_ExistingSection_SuccessfulUpdate()
//        {
//            /// SETUP           
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);

//            //Act-Controller
//            var ls = repo.NewLanguageSection(ia);
//            ls.InterpreterNotes = "Good at speaking English";
//            ls.IsNeedingInterpreter = true;
//            ls.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();
//            var data1 = (from x in Db.LanguageSections where x.Id == 1 select x).Single();
//            var time1 = data1.RowVersion;
//            ls.Id = 1;
//            ls.InterpreterNotes = "Good at Speaking English";
//            ls.IsNeedingInterpreter = false;
//            ls.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();
//            var data2 = (from x in Db.LanguageSections where x.Id == 1 select x).Single();
//            var time2 = data2.RowVersion;

//            //Assert
//            Assert.False(time1 == time2);
//        }

//        [Fact]
//        public void UpsertLanguageSection_ExistingSection_UpdateFailure()
//        {
//            //Arrange
//            IRepository repo = new Repository(Db);
//            var p = new Participant();
//            Db.Participants.Add(p);
//            var ia = new InformalAssessment();
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);

//            //Act-Controller
//            var ls = repo.NewLanguageSection(ia);
//            ls.InterpreterNotes = "Good at speaking English";
//            ls.IsNeedingInterpreter = true;
//            ls.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();
//            var data1 = (from x in Db.LanguageSections where x.Id == 1 select x).Single();
//            var time1 = data1.RowVersion;
//            var p1 = new Participant();
//            Db.Participants.Add(p1);
//            var ia1 = new InformalAssessment();
//            ia1.Participant = p1;
//            Db.InformalAssessments.Add(ia1);

//            //Act-Controller
//            var ls1 = repo.NewLanguageSection(ia);
//            ls1.Id = 1;
//            ls1.InterpreterNotes = "Good at Speaking English";
//            ls1.IsNeedingInterpreter = false;
//            ls1.DetailsAndObservations = "Needs More training on Writng Skills in English";
//            repo.Save();
//            var data2 = (from x in Db.LanguageSections where x.Id == 1 select x).Single();
//            var time2 = data2.RowVersion;

//            //Assert
//            Assert.True(time1 == time2);
//        }
//        [Fact]
//        public void UpsertLanguageSection_NewInformalAssessmentWithNoParticipant_Error()
//        {
//            //Arrange       
//            IRepository repo = new Repository(Db);
//            var ia = new InformalAssessment();
//            Db.InformalAssessments.Add(ia);

//            //Act-Controller
//            var ls = repo.NewLanguageSection(ia);
//            ls.InterpreterNotes = "Good at apeaking English";
//            ls.IsNeedingInterpreter = true;
//            ls.DetailsAndObservations = "Needs More training on Writng Skills in English";

//            //Assert
//            Assert.Throws<System.Data.Entity.Infrastructure.DbUpdateException>(() => repo.Save());
//        }
//        [Fact]
//        public void UpsertLanguageSection_NewAssessmentWithNullInformalAssessmentObject_Failure()
//        {
//            //Setup
//            IRepository repo = new Repository(Db);
//            IInformalAssessment ia = null;   
            
//            //Act-Controller

//            //Assert
//            Assert.Throws<System.NullReferenceException>(() => repo.NewLanguageSection(ia));
//        }

//    }


//}





