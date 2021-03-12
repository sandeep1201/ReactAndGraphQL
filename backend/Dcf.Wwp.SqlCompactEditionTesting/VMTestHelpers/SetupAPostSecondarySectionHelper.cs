//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers
//{
//	public class SetupAPostSecondarySectionHelper
//	{

//		public static void PostSecondaryEducationSectionData(Wwp.Data.Sql.Model.WwpEntities Db)
//		{
//			var pse = new Data.Sql.Model.PostSecondaryEducationSection();
//			pse.DidAttendCollege = true;
//			pse.IsWorkingOnLicensesOrCertificates = true;
//			pse.DoesHaveDegrees = true;
//			pse.Notes = "Graduated";
//			pse.ModifiedBy = "Sohinder";
//			pse.ModifiedDate = DateTime.Now;
//			Db.PostSecondaryEducationSections.Add(pse);
//			Db.SaveChanges();

//			var pl = new Data.Sql.Model.PostSecondaryLicense();
//			//pl.ModifiedDate = DateTime.Now;
//			pl.Name = "Madison Authority";
//			pl.PostSecondaryEducationSection = pse;
//			Db.PostSecondaryLicenses.Add(pl);
//			Db.SaveChanges();

//			var pdt = new Data.Sql.Model.DegreeType();
//			pdt.Code = "Bachelors";
//			pdt.Id = 1;

//			var pd = new Data.Sql.Model.PostSecondaryDegree();
//			pd.DegreeType = pdt;
//			pd.Name = "Bachelors in Education";
//			pd.PostSecondaryEducationSection = pse;
//			Db.PostSecondaryDegrees.Add(pd);
//			Db.SaveChanges();


//			var s = new Data.Sql.Model.State();
//			s.Code = "WN";

//			var psc = new Data.Sql.Model.PostSecondaryCollege();
//			psc.HasGraduated = true;
//			psc.State = s;
//			psc.PostSecondaryEducationSection = pse;
//			Db.PostSecondaryColleges.Add(psc);
//			Db.SaveChanges();


//			var p = new Data.Sql.Model.Participant();
//			p.CaseNumber = 300;
//			p.LastName = "cheedu";
//			p.PinNumber = 123;
//			Db.Participants.Add(p);
//			Db.SaveChanges();

//			var ia = new Data.Sql.Model.InformalAssessment();
//			ia.Participant = p;
//			ia.ParticipantId = 1;
//			ia.PostSecondaryEducationSection = pse;
//			ia.ModifiedDate = DateTime.Now;
//			Db.InformalAssessments.Add(ia);
//			Db.SaveChanges();

//		}
//	}
//}
