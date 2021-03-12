////using Dcf.Wwp.Entities.Interface;
//using Dcf.Wwp.Data.Sql.Model;
//using Dcf.Wwp.Model.Interface.Repository;
//using Dcf.Wwp.Model.Repository;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace Dcf.Wwp.SqlCompactEditionTesting.RepositoryTesting
//{
//    public class ParticipantRepositoryTests : BaseUnitTest
//    {
//        [Fact]
//        public void GetParticipant_ExistingParticipantById_Success()
//        {
//            InformalAssessmentTestHelper.SetupAInformalAssessmentWithSingleParticipant(Db);
//            IRepository repo = new Repository(Db);
//            //InformalAssesmentTestHelper.SetupAInformalAssessment(WwpContext);
//            var i = repo.ParticipantByPin("123");
//            Assert.NotNull(i);
//        }
//    }
//}
