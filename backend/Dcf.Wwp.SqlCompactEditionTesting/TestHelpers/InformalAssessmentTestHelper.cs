//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.SqlCompactEditionTesting.TestHelpers
//{
//	public static class InformalAssessmentTestHelper
//	{
//		public static void SetupAInformalAssessmentWithSingleParticipant(Wwp.Data.Sql.Model.WwpEntities Db)
//		{
			
//			var p = new Data.Sql.Model.Participant();
//			p.CaseNumber = 300;
//            p.LastName = "cheedu";
//            p.PinNumber = 123;
//			Db.Participants.Add(p);
//			Db.SaveChanges();
//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.ModifiedDate = DateTime.Now;
//            ia.Participant = p;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();

//            var part = (from x in Db.InformalAssessments where x.Id == 1 select x).FirstOrDefault();
//		}

//        public static void SetupAInformalAssessmentWithoutParticipant(Wwp.Data.Sql.Model.WwpEntities Db)
//        {            
//            var ia = new Data.Sql.Model.InformalAssessment();
//            ia.ModifiedDate = DateTime.Now;
//            Db.InformalAssessments.Add(ia);
//            Db.SaveChanges();

//            var part = (from x in Db.InformalAssessments where x.Id == 1 select x).FirstOrDefault();
//        }

//        //public static void SetupInformalAssessmentParticipant(Wwp.Data.Sql.Model.WwpEntities Db)
//        //{

//        //	var la = new Data.Model.LanguageSection();
//        //	Db.LanguageSections.Add(la);


//        //	var p = new Data.Model.Participant();
//        //	p.Case = c;
//        //	p.Individual = i;
//        //	Db.Participants.Add(p);


//        //	byte[] v1 = new byte[2] {1, 2};

//        //	var st = new Data.Model.State();
//        //	st.Code = "WI";
//        //	Db.States.Add(st);

//        //	var s = new Data.Model.School();
//        //	s.ModifiedDate = DateTime.Now;
//        //	s.State = st;
//        //	Db.Schools.Add(s);


//        //	var c1 = new Data.Model.SchoolGraduationStatus();
//        //	Db.SchoolGraduationStatuses.Add(c1);

//        //	var c2 = new Data.Model.SchoolGradeLevel();
//        //	Db.SchoolGradeLevels.Add(c2);


//        //	var ea = new Data.Model.EducationSection();
//        //	ea.ModifieDby = "Sohi";
//        //	ea.ModifiedDate = DateTime.Now;

//        //	ea.LastAttendedSchool = s;
//        //	ea.HighSchoolGraduationStatus = c1;
//        //	ea.LastGradeLevelCompleted = c2;

//        //	Db.EducationSections.Add(ea);


//        //	var ia = new Data.Model.InformalAssessment();

//        //	ia.ModifiedDate = DateTime.Now;
//        //	ia.EducationSection = ea;
//        //	//ia.EducationAssessment.HighSchoolOrEquivalentSchool = s;
//        //	ia.LanguageSection = la;
//        //	ia.Participant = p;
//        //	Db.InformalAssessments.Add(ia);
//        //	Db.SaveChanges();
//        //	var part = (from x in Db.Participants where x.Individual.PinNumber == 123m select x).FirstOrDefault();
//        //}

//			/*
//        public static void SetupInformalAssessment(Wwp.Data.WwpContext Db)
//		{
//			//var c = new Data.Model.Case();
//			//c.CaseNumber = 123m;
//			//Db.Cases.Add(c);

//			//var i = new Data.Model.Individual();
//			//i.PinNumber = 123m;
//			//Db.Individuals.Add(i);

//			//var la = new Data.Model.LanguageSection();
//			//Db.LanguageSections.Add(la);

//			//var p = new Data.Model.Participant();
//			//p.Case = c;
//			//p.Individual = i;
//			//Db.Participants.Add(p);

//			//byte[] v1 = new byte[2] { 1, 2 };

//			//var st = new Data.Model.State();
//			//st.Code = "WI";
//			//Db.States.Add(st);

//			//var s = new Data.Model.School();
//			//s.ModifiedDate = DateTime.Now;
//			//s.State = st;
//			//Db.Schools.Add(s);

//			////end of edu


//			//var c1 = new Data.Model.SchoolGraduationStatus();
//			//Db.SchoolGraduationStatuses.Add(c1);

//			//var c2 = new Data.Model.SchoolGradeLevel();
//			//Db.SchoolGradeLevels.Add(c2);


//			//var ea = new Data.Model.EducationSection();
//			//ea.ModifieDby = "Sohi";
//			//ea.ModifiedDate = DateTime.Now;

//			//ea.LastAttendedSchool = s;
//			//ea.HighSchoolGraduationStatus = c1;
//			//ea.LastGradeLevelCompleted = c2;

//			////ea.Version = v1;
//			//Db.EducationSections.Add(ea);

//			////var c = new Data.Model.Case();
//			////c.CaseNumber = 123m;
//			////Db.Cases.Add(c);

//			////var i = new Data.Model.Individual();
//			////i.PinNumber = 123m;
//			////Db.Individuals.Add(i);

//			////var p = new Data.Model.Participant();
//			////p.Case = c;
//			////p.Individual = i;
//			////Db.Participants.Add(p);

//			////var la = new Data.Model.LanguageAssessment();
//			////la.ModifiedDate = DateTime.Now;
//			////Db.LanguageAssessments.Add(la);


//			//Data.Model.Language[] ixx = new Data.Model.Language[45];

//			//for (int ix = 0; ix < 41; ix++)
//			//{
//			//	ixx[ix] = new Data.Model.Language();
//			//	{
//			//		ixx[ix].Name = "abc";
//			//	}

//			//	Db.Languages.Add(ixx[ix]);

//			//}

//			////var ea = new Data.Model.EducationAssessment();
//			//////ea.CreatedDate = DateTime.Now;
//			////ea.ModifiedDate = DateTime.Now;
//			////Db.EducationAssessments.Add(ea);

//			//var kl = new Data.Model.KnownLanguage();
//			//kl.LanguageSection = la;
//			////kl.Language = ixx;
//			//kl.ModifiedDate = DateTime.Now;
//			//kl.LanguageId = 1;
//			//Db.KnownLanguages.Add(kl);

//			var ia = new Data.Model.InformalAssessment();			
//			ia.ModifiedDate = DateTime.Now;
//			Db.InformalAssessments.Add(ia);

//			Db.SaveChanges();
//		}
//		*/
//	}
//}
