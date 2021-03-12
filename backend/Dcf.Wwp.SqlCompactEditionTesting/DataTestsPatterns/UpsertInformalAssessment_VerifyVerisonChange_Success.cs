//using Dcf.Wwp.Data.Model;
//using Dcf.Wwp.Entities;
//using Dcf.Wwp.Entities.Interface;
//using Dcf.Wwp.SqlCompactEditionTesting.TestHelpers;
//using Microsoft.Data.Entity;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;
//using Xunit.Extensions;

//namespace Dcf.Wwp.SqlCompactEditionTesting.DataTestsPatterns
//{


//	public class UpsertInformalAssessment_VerifyVerisonChange_Success : BaseUnitTest
//	{

//		// Creates an informal assessment with a modifieDby property set. Later the 
//		// property is changed, but the row version remains unchanged!

//		//Testing our Data Models By passing Model objects to Db(SQLCompact Edition)
//		//[Fact]
//		public void UpsertInformalAssessment_VerifyVersionChange_Failure()
//		{
//			InformalAssessmentTestHelper.SetupASingleParticipant(WwpContext);

//			IRepository repo = new Repository(WwpContext);
//			//Create informal Assessment
//			var ia = new Wwp.Data.Model.InformalAssessment();
//			ia.ModifiedDate = DateTime.Now;
//			ia.ModifieDby = "Sohinder";
//			ia.ParticipantId = 1;
//			WwpContext.InformalAssessments.Add(ia);
//			WwpContext.SaveChanges();

//			//get version
//			var data = (from x in WwpContext.InformalAssessments where x.Id == 1 select x).Single();

//			var t1 = data.RowVersion;

//			//Change value modifieDby value
//			ia.ModifieDby = "Dinesh";


//			//get new updated verison
//			var data2 = (from x in WwpContext.InformalAssessments where x.Id == 1 select x).Single();


//			var t2 = data2.RowVersion;


//			// RESULTS ROW VERSION DID NOT CHANGE
//			Assert.False(t1 != t2);

//		}

//		//[Fact]
//		public void UpsertInformalAssessment_VerifyVersionChange_Success()
//		{


//			InformalAssessmentTestHelper.SetupASingleParticipant(WwpContext);

//			//IRepository repo = new Repository(WwpContext);
//			//Create informal Assessment
//			var ia = new Wwp.Data.Model.InformalAssessment();

//			ia.ModifiedDate = DateTime.Now;
//			ia.ModifieDby = "Sohinder";
//			ia.ParticipantId = 1;
//			WwpContext.InformalAssessments.Add(ia);
//			WwpContext.SaveChanges();

//			var ia3 = new Wwp.Data.Model.InformalAssessment();

//			ia3.ModifiedDate = DateTime.Now;
//			ia3.ModifieDby = "Sohinder2";
//			ia3.ParticipantId = 1;
//			WwpContext.InformalAssessments.Add(ia3);
//			WwpContext.SaveChanges();

//			//get version
//			var data = (from x in WwpContext.InformalAssessments where x.Id == 1 select x).Single();
//			var data3 = (from x in WwpContext.InformalAssessments where x.Id == 2 select x).Single();

//			var dataall = (WwpContext.InformalAssessments);

//			var t1 = data.RowVersion;

//			//Change value modifieDby value

//			data.ModifieDby = "Dinesh";
//			WwpContext.SaveChanges();


//			//get new updated verison
//			var data2 = (from x in WwpContext.InformalAssessments where x.Id == 1 select x).Single();


//			var t2 = data2.RowVersion;


//			//harvest database of data
//			//var del = WwpContext.InformalAssessments.SingleOrDefault(x => x.Id == 2);
//			//var del2 = WwpContext.InformalAssessments.SingleOrDefault(x => x.Id == 3);

//			//if (del != null)
//			//{
//			//	WwpContext.InformalAssessments.Remove(del);
//			//	WwpContext.SaveChanges();
//			//}


//			// RESULTS ROW VERSION DID NOT CHANGE
//			//	Assert.False(t1 == t2);






//		}




//		//[Fact]
//		public void ConcurrentlyUpsertInformalAssessment_VerifyVersionChange_UpdateIAinDb()
//		{


//			//is newer than database
//			byte[] ViewVersion = { 0, 0, 0, 0, 0, 0, 2, 25};

//			InformalAssessmentTestHelper.SetupASingleParticipant(WwpContext);

//			var ia = new Wwp.Data.Model.InformalAssessment();

//			ia.ModifiedDate = DateTime.Now;
//			ia.ModifieDby = "Sohinder";
//			ia.ParticipantId = 1;
//			WwpContext.InformalAssessments.Add(ia);
//			WwpContext.SaveChanges();
			

//			var data = (from x in WwpContext.InformalAssessments where x.Id == 1 select x).Single();

//			var datasave = data;
		

//			if (ViewVersion == data.RowVersion)
//			{
//				Assert.False(true);
//			}
				


//			data.ModifiedDate = DateTime.Now;
//			data.ModifieDby = "Dinesh";
//			data.ParticipantId = 1;
//			WwpContext.InformalAssessments.Attach(data);
//			WwpContext.SaveChanges();

//			Assert.True(data.RowVersion[7] > ViewVersion[7]);

//			var dataall = (WwpContext);


			
//		}


//	}
//}
