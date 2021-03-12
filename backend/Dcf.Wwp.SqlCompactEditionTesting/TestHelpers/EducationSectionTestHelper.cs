//using Dcf.Wwp.Web.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Dcf.Wwp.SqlCompactEditionTesting.TestHelpers
//{
//    public class EducationSectionTestHelper
//    {
//        public static void SetupSchool_State_GradStatus_Gradelevel(Wwp.Data.Sql.Model.WwpEntities Db)

//        {         
//            byte[] v1 = new byte[2] { 1, 2 };

//            var st = new Data.Sql.Model.State();
//            st.Code = "WI";
//            var code = st.Code.SafeTrim();
           
//            Db.States.Add(st);

//            var s = new Data.Sql.Model.School();
//            s.ModifiedDate = DateTime.Now;
//            s.State = st;
//            s.Name = "Francis";
//            s.City = "Md";
//            s.Street = "GreenBriarApts";

//            Db.Schools.Add(s);

//            //end of edu
//            var c1 = new Data.Sql.Model.SchoolGraduationStatus();
//            Db.SchoolGraduationStatus1.Add(c1);

//            var c2 = new Data.Sql.Model.SchoolGradeLevel();
//            Db.SchoolGradeLevels.Add(c2);

//            var ea = new Data.Sql.Model.EducationSection();
//            //ea.CreatedDate = DateTime.Now;
//            ea.ModifiedBy = "Sohi";
//            ea.ModifiedDate = DateTime.Now;
//            ea.School = s;
//            ea.SchoolGraduationStatus = c1;
//            ea.LastGradeLevelCompleted = c2;           
//            Db.EducationSections.Add(ea);
//            Db.SaveChanges();
//        }
//        public static void SetupMultipleSchools_State_GradStatus_Gradelevel(Wwp.Data.Sql.Model.WwpEntities Db)

//        {
//            byte[] v1 = new byte[2] { 1, 2 };

//            var st = new Data.Sql.Model.State();
//            st.Code = "WI";
//            st.Name = "WIS";
//            Db.States.Add(st);

//            var s = new Data.Sql.Model.School();
//            s.ModifiedDate = DateTime.Now;
//            s.Name = "St.Claret";
//            s.State = st;
//            s.Street = "123";
//            s.City = "MD";
//            Db.Schools.Add(s);
//            var s1 = new Data.Sql.Model.School();
//            s1.ModifiedDate = DateTime.Now;
//            s1.Name = "St.Francis";
//            s1.City = "Md";
//            s1.State = st;
//            s1.Street = "123";
//            Db.Schools.Add(s1);

//            //end of edu
//            var c1 = new Data.Sql.Model.SchoolGraduationStatus();
//            Db.SchoolGraduationStatus1.Add(c1);

//            var c2 = new Data.Sql.Model.SchoolGradeLevel();
//            Db.SchoolGradeLevels.Add(c2);

//            var ea = new Data.Sql.Model.EducationSection();
//            //ea.CreatedDate = DateTime.Now;
//            ea.ModifiedBy = "Sohi";
//            ea.ModifiedDate = DateTime.Now;
//            ea.School = s1;
//            ea.SchoolGraduationStatus = c1;
//            ea.LastGradeLevelCompleted = c2;
//            Db.EducationSections.Add(ea);
//            Db.SaveChanges();
//        }

//        public static void SetupMultipleGrades(Wwp.Data.Sql.Model.WwpEntities Db)

//        {
            
//            //end of edu
//            var g1 = new Data.Sql.Model.SchoolGradeLevel();
//            g1.Grade = 1;
//            Db.SchoolGradeLevels.Add(g1);
//            var g2 = new Data.Sql.Model.SchoolGradeLevel();
//            g2.Grade = 2;
//            Db.SchoolGradeLevels.Add(g2);

//            var ea = new Data.Sql.Model.EducationSection();
//            ea.LastGradeLevelCompleted = g1;
//            Db.EducationSections.Add(ea);
//            Db.SaveChanges();
//        }

//        public static void SetupMultipleCertificateissuers(Wwp.Data.Sql.Model.WwpEntities Db)

//        {
//            //end of edu
//            var ce = new Data.Sql.Model.CertificateIssuingAuthority();
//            ce.Name = "MadisonAuthority";
//            Db.CertificateIssuingAuthorities.Add(ce);
//            var ce1= new Data.Sql.Model.CertificateIssuingAuthority();
//            ce1.Name = "ChicagoAuthority";
//            Db.CertificateIssuingAuthorities.Add(ce);
//            var ea = new Data.Sql.Model.EducationSection();
//            ea.CertificateIssuingAuthority = ce;
//            Db.EducationSections.Add(ea);
//            Db.SaveChanges();
//        }
//    }
//}
