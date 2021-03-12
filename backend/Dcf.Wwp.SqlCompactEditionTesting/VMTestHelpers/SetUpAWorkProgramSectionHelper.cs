using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dcf.Wwp.SqlCompactEditionTesting.VMTestHelpers
{
	public class SetUpAWorkProgramSectionHelper
	{
		public static void WorkPorgramSectionData(Wwp.Data.Sql.Model.WwpEntities Db)
		{

			var wp = new Data.Sql.Model.WorkProgram();
			wp.Name = "DVR";
			wp.ModifiedBy = "Dinesh";
			wp.ModifiedDate = DateTime.Now;


			var wpst = new Data.Sql.Model.WorkProgramStatus();
			wpst.Name = "Past";
			wpst.ModifiedBy = "Dinesh";
			wpst.ModifiedDate = DateTime.Now;


			var wps = new Data.Sql.Model.WorkProgramSection();
			Db.WorkProgramSections.Add(wps);
	

			var iwp = new Data.Sql.Model.InvolvedWorkProgram();
			iwp.WorkProgramStatus = wpst;
			iwp.WorkProgram = wp;
			iwp.WorkProgramSection = wps;
			Db.SaveChanges();


			var p = new Data.Sql.Model.Participant();
			p.CaseNumber = 300;
			p.LastName = "cheedu";
			p.PinNumber = 123;
			Db.Participants.Add(p);
			Db.SaveChanges();

			var ia = new Data.Sql.Model.InformalAssessment();
			ia.Participant = p;
			ia.ParticipantId = 1;
			ia.WorkProgramSection = wps;
			ia.ModifiedDate = DateTime.Now;
			Db.InformalAssessments.Add(ia);
			Db.SaveChanges();
		}
	}
}
