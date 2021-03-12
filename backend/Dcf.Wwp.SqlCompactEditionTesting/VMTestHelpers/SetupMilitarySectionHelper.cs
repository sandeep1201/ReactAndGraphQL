//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers
//{
//    public class SetupMilitarySectionHelper
//    {
//        public static void WithMilitaryTrainingSectionData(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var mb = new Data.Sql.Model.MilitaryBranch();
//            mb.Name = "MarineCops";
//            mb.ModifiedBy = "cheedu";
//            mb.ModifiedDate = DateTime.Now;
//            Db.MilitaryBranches.Add(mb);

//            var mdt = new Data.Sql.Model.MilitaryDischargeType();
//            mdt.Name = "Currently Enlisted";
//            mdt.ModifiedBy = "cheedu";
//            mdt.ModifiedDate = DateTime.Now;
//            Db.MilitaryDischargeTypes.Add(mdt);

//            var mts = new Data.Sql.Model.MilitaryTrainingSection();
//            mts.MilitaryDischargeType = mdt;
//            mts.MilitaryBranch = mb;
//            mts.DoesHaveTraining = false;
//            mts.ModifiedBy = "cheedu";
//            mts.ModifiedDate = DateTime.Now;
//            Db.MilitaryTrainingSections.Add(mts);

//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "cheedu";
//            p.PinNumber = 123;
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            ia.MilitaryTrainingSection = mts;
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();
//        }

//        public static void WithoutMilitaryTrainingSectionData(Wwp.Data.Sql.Model.WwpEntities Db)
//        {
//            var mb = new Data.Sql.Model.MilitaryBranch();
//            mb.Name = "MarineCops";
//            mb.ModifiedBy = "cheedu";
//            mb.ModifiedDate = DateTime.Now;
//            Db.MilitaryBranches.Add(mb);

//            var mdt = new Data.Sql.Model.MilitaryDischargeType();
//            mdt.Name = "Currently Enlisted";
//            mdt.ModifiedBy = "cheedu";
//            mdt.ModifiedDate = DateTime.Now;
//            Db.MilitaryDischargeTypes.Add(mdt);

//            var p = new Data.Sql.Model.Participant();
//            p.CaseNumber = 300;
//            p.LastName = "cheedu";
//            p.PinNumber = 123;
//            Db.Participants.Add(p);
//            Db.SaveChanges();

//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.Participant = p;
//            ia.ParticipantId = 1;
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();
//        }
//    }
//}
