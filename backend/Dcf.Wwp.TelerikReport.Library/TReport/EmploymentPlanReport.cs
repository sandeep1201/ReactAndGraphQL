using Telerik.Reporting;
using Dcf.Wwp.Api.Library.Contracts;
using Telerik.Reporting.Drawing;

namespace Dcf.Wwp.TelerikReport.Library
{
    public partial class EmploymentPlanReport : Report
    {
        #region Properties

        #endregion

        #region Methods

        public EmploymentPlanReport(PrintedEmployabilityPlanReportContract contract)
        {
            InitializeComponent();

            if (!string.IsNullOrWhiteSpace(contract.WorkerPhone))
                TR_WorkerPhone.Value = $@"{decimal.Parse(contract.WorkerPhone):(###) ###-####}";
            else
            {
                Phone.Style.Visible           = false;
                TR_WorkerPhone.Style.Visible  = false;
                textBox2.Style.Visible        = false;
                SpaceBox_Phone1.Style.Visible = false;
            }

            if (string.IsNullOrWhiteSpace(contract.WorkerEmail))
            {
                WorkerEmail.Style.Visible = false;
                WorkEmail2.Style.Visible  = false;
                email.Style.Visible       = false;
            }
            else
            {
                WorkerEmail.Value = contract.WorkerEmail;
                WorkEmail2.Value  = contract.WorkerEmail;

                if (contract.WorkerEmail.Length > 23)
                    WorkerEmail.Style.Visible = false;
                else
                    WorkEmail2.Style.Visible = false;
            }

            //There must be at least one goal
            GoalList.DataSource = contract.Goals;

            //EmploymentContract
            if (contract.Employment != null && contract.Employment.Count > 0)
                EmploymentList.DataSource = contract.Employment;
            else
                EmploymentList.Style.Visible = false;

            //ActivityContract
            if (contract.Activites != null && contract.Activites.Count > 0)
            {
                ActivitiyList.DataSource = contract.Activites;
            }
            else
            {
                ActivitiyList.Style.Visible = false;
                AssignedPanel.Style.Visible = false;
            }

            //SupportContract List
            if (contract.Support != null && contract.Support.Count > 0)
            {
                SupportiveList.DataSource = contract.Support;
            }
            else
            {
                Supportive_Services.Style.Visible = false;
                SpaceBoxSupport1.Style.Visible    = false;
                SupportiveList.Style.Visible      = false;
                SupportPanel.Style.Visible        = false;
            }

            //Notes
            if (EPNotes.Value.Equals(""))
            {
                NotesPanel.Style.Visible = false;
            }

            //learnfare requires parent signature

            if (contract.Placement.Equals("LearnFare"))
            {
                Sig_Signature.Value = @"Signature of Parent";
            }

            //Schedules
            if (contract.Schedule.Count > 0)
                ScheduleList.DataSource = contract.Schedule;
            else
                ScheduleSection.Style.Visible = false;
        }

        #endregion
    }
}
