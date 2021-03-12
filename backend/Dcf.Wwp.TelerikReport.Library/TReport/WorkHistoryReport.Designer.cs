namespace Dcf.Wwp.TelerikReport.Library
{
	partial class WorkHistoryReport
	{
		#region Component Designer generated code
		/// <summary>
		/// Required method for telerik Reporting designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.ParticipantName = new Telerik.Reporting.TextBox();
            this.SectionName = new Telerik.Reporting.TextBox();
            this.WHDetailSection = new Telerik.Reporting.DetailSection();
            this.EmploymentList = new Telerik.Reporting.List();
            this.EmplymentList = new Telerik.Reporting.Panel();
            this.JobTypeLabel = new Telerik.Reporting.TextBox();
            this.EmployerLabel = new Telerik.Reporting.TextBox();
            this.LocationLabel = new Telerik.Reporting.TextBox();
            this.LocationValue = new Telerik.Reporting.TextBox();
            this.ContactLabel = new Telerik.Reporting.TextBox();
            this.ContactNameValue = new Telerik.Reporting.TextBox();
            this.ContactNumberValue = new Telerik.Reporting.TextBox();
            this.DatesLabel = new Telerik.Reporting.TextBox();
            this.BeginDateValue = new Telerik.Reporting.TextBox();
            this.EndDateValue = new Telerik.Reporting.TextBox();
            this.BeginDateLabel = new Telerik.Reporting.TextBox();
            this.EndDateLabel = new Telerik.Reporting.TextBox();
            this.PayRateLabel = new Telerik.Reporting.TextBox();
            this.BeginPayValue = new Telerik.Reporting.TextBox();
            this.EndPayValue = new Telerik.Reporting.TextBox();
            this.BeginPayLabel = new Telerik.Reporting.TextBox();
            this.EndPayLabel = new Telerik.Reporting.TextBox();
            this.HoursValue = new Telerik.Reporting.TextBox();
            this.HoursLabel = new Telerik.Reporting.TextBox();
            this.PositionValue = new Telerik.Reporting.TextBox();
            this.PositionLabel = new Telerik.Reporting.TextBox();
            this.DutiesValue = new Telerik.Reporting.TextBox();
            this.DutiesLabel = new Telerik.Reporting.TextBox();
            this.JobTypeValue = new Telerik.Reporting.TextBox();
            this.EmployerValue = new Telerik.Reporting.TextBox();
            this.EndLine = new Telerik.Reporting.Shape();
            this.SpaceBox = new Telerik.Reporting.TextBox();
            this.WHFooterSection = new Telerik.Reporting.PageFooterSection();
            this.PageNumberFooter = new Telerik.Reporting.TextBox();
            this.reportHeaderSection1 = new Telerik.Reporting.ReportHeaderSection();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // ParticipantName
            // 
            this.ParticipantName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.ParticipantName.Name = "ParticipantName";
            this.ParticipantName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.268D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
            this.ParticipantName.Style.Font.Bold = true;
            this.ParticipantName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(18D);
            this.ParticipantName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.ParticipantName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.ParticipantName.Value = "= Parameters.Participant.Value";
            // 
            // SectionName
            // 
            this.SectionName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
            this.SectionName.Name = "SectionName";
            this.SectionName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.268D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.SectionName.Style.Font.Bold = true;
            this.SectionName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(16D);
            this.SectionName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.SectionName.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.SectionName.Value = "Work History";
            // 
            // WHDetailSection
            // 
            this.WHDetailSection.Height = Telerik.Reporting.Drawing.Unit.Inch(4.3D);
            this.WHDetailSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.EmploymentList});
            this.WHDetailSection.Name = "WHDetailSection";
            this.WHDetailSection.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            // 
            // EmploymentList
            // 
            this.EmploymentList.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Mm(204.93D)));
            this.EmploymentList.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Pixel(384D)));
            this.EmploymentList.Body.SetCellContent(0, 0, this.EmplymentList);
            tableGroup1.Name = "ColumnGroup";
            this.EmploymentList.ColumnGroups.Add(tableGroup1);
            this.EmploymentList.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.EmplymentList});
            this.EmploymentList.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(5.08D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EmploymentList.Name = "EmploymentList";
            tableGroup2.Groupings.Add(new Telerik.Reporting.Grouping(null));
            tableGroup2.Name = "DetailGroup";
            this.EmploymentList.RowGroups.Add(tableGroup2);
            this.EmploymentList.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(204.93D), Telerik.Reporting.Drawing.Unit.Mm(104.14D));
            this.EmploymentList.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.None;
            this.EmploymentList.Style.Padding.Bottom = Telerik.Reporting.Drawing.Unit.Inch(0.1D);
            // 
            // EmplymentList
            // 
            this.EmplymentList.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.JobTypeLabel,
            this.EmployerLabel,
            this.LocationLabel,
            this.LocationValue,
            this.ContactLabel,
            this.ContactNameValue,
            this.ContactNumberValue,
            this.DatesLabel,
            this.BeginDateValue,
            this.EndDateValue,
            this.BeginDateLabel,
            this.EndDateLabel,
            this.PayRateLabel,
            this.BeginPayValue,
            this.EndPayValue,
            this.BeginPayLabel,
            this.EndPayLabel,
            this.HoursValue,
            this.HoursLabel,
            this.PositionValue,
            this.PositionLabel,
            this.DutiesValue,
            this.DutiesLabel,
            this.JobTypeValue,
            this.EmployerValue,
            this.EndLine,
            this.SpaceBox});
            this.EmplymentList.Name = "EmplymentList";
            this.EmplymentList.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(204.93D), Telerik.Reporting.Drawing.Unit.Pixel(384D));
            this.EmplymentList.Style.Visible = true;
            // 
            // JobTypeLabel
            // 
            this.JobTypeLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(0D));
            this.JobTypeLabel.Name = "JobTypeLabel";
            this.JobTypeLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(22.86D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.JobTypeLabel.Style.Font.Bold = true;
            this.JobTypeLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.JobTypeLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.JobTypeLabel.Value = "Job Type";
            // 
            // EmployerLabel
            // 
            this.EmployerLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(99.06D), Telerik.Reporting.Drawing.Unit.Mm(0D));
            this.EmployerLabel.Name = "EmployerLabel";
            this.EmployerLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(25.4D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EmployerLabel.Style.Font.Bold = true;
            this.EmployerLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.EmployerLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EmployerLabel.Style.Visible = true;
            this.EmployerLabel.Value = "Employer";
            // 
            // LocationLabel
            // 
            this.LocationLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(12.7D));
            this.LocationLabel.Name = "LocationLabel";
            this.LocationLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(22.86D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.LocationLabel.Style.Font.Bold = true;
            this.LocationLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.LocationLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.LocationLabel.Style.Visible = true;
            this.LocationLabel.Value = "Location";
            // 
            // LocationValue
            // 
            this.LocationValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(17.78D));
            this.LocationValue.Name = "LocationValue";
            this.LocationValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(200.66D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.LocationValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.LocationValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.LocationValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.LocationValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.LocationValue.Style.Visible = true;
            this.LocationValue.Value = "= Fields.Address";
            // 
            // ContactLabel
            // 
            this.ContactLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(25.4D));
            this.ContactLabel.Name = "ContactLabel";
            this.ContactLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.ContactLabel.Style.Font.Bold = true;
            this.ContactLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.ContactLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.ContactLabel.Style.Visible = true;
            this.ContactLabel.Value = "Employer Contact";
            // 
            // ContactNameValue
            // 
            this.ContactNameValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(30.48D));
            this.ContactNameValue.Name = "ContactNameValue";
            this.ContactNameValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(86.36D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.ContactNameValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.ContactNameValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.ContactNameValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.ContactNameValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.ContactNameValue.Style.Visible = true;
            this.ContactNameValue.Value = "= Fields.ContactName";
            // 
            // ContactNumberValue
            // 
            this.ContactNumberValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(99.06D), Telerik.Reporting.Drawing.Unit.Mm(30.48D));
            this.ContactNumberValue.Name = "ContactNumberValue";
            this.ContactNumberValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(101.6D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.ContactNumberValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.ContactNumberValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.ContactNumberValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.ContactNumberValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.ContactNumberValue.Style.Visible = true;
            this.ContactNumberValue.Value = "= Fields.ContactNumber";
            // 
            // DatesLabel
            // 
            this.DatesLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(38.1D));
            this.DatesLabel.Name = "DatesLabel";
            this.DatesLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.DatesLabel.Style.Font.Bold = true;
            this.DatesLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.DatesLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.DatesLabel.Style.Visible = true;
            this.DatesLabel.Value = "Dates Employed:";
            // 
            // BeginDateValue
            // 
            this.BeginDateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(48.27D));
            this.BeginDateValue.Name = "BeginDateValue";
            this.BeginDateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(86.36D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.BeginDateValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.BeginDateValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.BeginDateValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.BeginDateValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.BeginDateValue.Style.Visible = true;
            this.BeginDateValue.Value = "= Fields.JobBeginDate";
            // 
            // EndDateValue
            // 
            this.EndDateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(99.06D), Telerik.Reporting.Drawing.Unit.Mm(48.27D));
            this.EndDateValue.Name = "EndDateValue";
            this.EndDateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(101.6D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EndDateValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.EndDateValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.EndDateValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.EndDateValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EndDateValue.Style.Visible = true;
            this.EndDateValue.Value = "= Fields.JobEndDate";
            // 
            // BeginDateLabel
            // 
            this.BeginDateLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(43.18D));
            this.BeginDateLabel.Name = "BeginDateLabel";
            this.BeginDateLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.BeginDateLabel.Style.Font.Bold = true;
            this.BeginDateLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.BeginDateLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.BeginDateLabel.Style.Visible = true;
            this.BeginDateLabel.Value = "Begin Date";
            // 
            // EndDateLabel
            // 
            this.EndDateLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(99.06D), Telerik.Reporting.Drawing.Unit.Mm(43.18D));
            this.EndDateLabel.Name = "EndDateLabel";
            this.EndDateLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EndDateLabel.Style.Font.Bold = true;
            this.EndDateLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.EndDateLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EndDateLabel.Style.Visible = true;
            this.EndDateLabel.Value = "End Date";
            // 
            // PayRateLabel
            // 
            this.PayRateLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(55.88D));
            this.PayRateLabel.Name = "PayRateLabel";
            this.PayRateLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.PayRateLabel.Style.Font.Bold = true;
            this.PayRateLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.PayRateLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.PayRateLabel.Style.Visible = true;
            this.PayRateLabel.Value = "Pay Rate:";
            // 
            // BeginPayValue
            // 
            this.BeginPayValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(66.05D));
            this.BeginPayValue.Name = "BeginPayValue";
            this.BeginPayValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.96D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.BeginPayValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.BeginPayValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.BeginPayValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.BeginPayValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.BeginPayValue.Style.Visible = true;
            this.BeginPayValue.Value = "= Fields.BeginPayRate";
            // 
            // EndPayValue
            // 
            this.EndPayValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(69.6D), Telerik.Reporting.Drawing.Unit.Mm(66.05D));
            this.EndPayValue.Name = "EndPayValue";
            this.EndPayValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.96D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EndPayValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.EndPayValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.EndPayValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.EndPayValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EndPayValue.Style.Visible = true;
            this.EndPayValue.Value = "= Fields.EndPayRate";
            // 
            // BeginPayLabel
            // 
            this.BeginPayLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(60.96D));
            this.BeginPayLabel.Name = "BeginPayLabel";
            this.BeginPayLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.BeginPayLabel.Style.Font.Bold = true;
            this.BeginPayLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.BeginPayLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.BeginPayLabel.Style.Visible = true;
            this.BeginPayLabel.Value = "Starting Pay";
            // 
            // EndPayLabel
            // 
            this.EndPayLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(69.6D), Telerik.Reporting.Drawing.Unit.Mm(60.96D));
            this.EndPayLabel.Name = "EndPayLabel";
            this.EndPayLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(45.72D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EndPayLabel.Style.Font.Bold = true;
            this.EndPayLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.EndPayLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EndPayLabel.Style.Visible = true;
            this.EndPayLabel.Value = "Ending Pay";
            // 
            // HoursValue
            // 
            this.HoursValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(139.7D), Telerik.Reporting.Drawing.Unit.Mm(66.05D));
            this.HoursValue.Name = "HoursValue";
            this.HoursValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.96D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.HoursValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.HoursValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.HoursValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.HoursValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.HoursValue.Style.Visible = true;
            this.HoursValue.Value = "= Fields.Hours";
            // 
            // HoursLabel
            // 
            this.HoursLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(139.7D), Telerik.Reporting.Drawing.Unit.Mm(60.96D));
            this.HoursLabel.Name = "HoursLabel";
            this.HoursLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(60.96D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.HoursLabel.Style.Font.Bold = true;
            this.HoursLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.HoursLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.HoursLabel.Style.Visible = true;
            this.HoursLabel.Value = "Average Weekly Hours";
            // 
            // PositionValue
            // 
            this.PositionValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(78.74D));
            this.PositionValue.Name = "PositionValue";
            this.PositionValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(200.66D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.PositionValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.PositionValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.PositionValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.PositionValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.PositionValue.Style.Visible = true;
            this.PositionValue.Value = "= Fields.JobPosition";
            // 
            // PositionLabel
            // 
            this.PositionLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(73.66D));
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(22.86D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.PositionLabel.Style.Font.Bold = true;
            this.PositionLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.PositionLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.PositionLabel.Style.Visible = true;
            this.PositionLabel.Value = "Position";
            // 
            // DutiesValue
            // 
            this.DutiesValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(91.44D));
            this.DutiesValue.Name = "DutiesValue";
            this.DutiesValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(200.66D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.DutiesValue.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.DutiesValue.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.DutiesValue.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.DutiesValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.DutiesValue.Style.Visible = true;
            this.DutiesValue.Value = "= Fields.JobDuties";
            // 
            // DutiesLabel
            // 
            this.DutiesLabel.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(86.36D));
            this.DutiesLabel.Name = "DutiesLabel";
            this.DutiesLabel.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(27.94D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.DutiesLabel.Style.Font.Bold = true;
            this.DutiesLabel.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.DutiesLabel.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.DutiesLabel.Style.Visible = true;
            this.DutiesLabel.Value = "Job Duties";
            // 
            // JobTypeValue
            // 
            this.JobTypeValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.JobTypeValue.Name = "JobTypeValue";
            this.JobTypeValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(86.36D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.JobTypeValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.JobTypeValue.Value = "= Fields.JobTypeName";
            // 
            // EmployerValue
            // 
            this.EmployerValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(99.06D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EmployerValue.Name = "EmployerValue";
            this.EmployerValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(101.6D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.EmployerValue.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.EmployerValue.Value = "= Fields.CompanyName";
            // 
            // EndLine
            // 
            this.EndLine.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(3.8D));
            this.EndLine.Name = "EndLine";
            this.EndLine.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.EndLine.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.9D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            // 
            // SpaceBox
            // 
            this.SpaceBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(0D), Telerik.Reporting.Drawing.Unit.Mm(22.86D));
            this.SpaceBox.Name = "SpaceBox";
            this.SpaceBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(200.66D), Telerik.Reporting.Drawing.Unit.Mm(2.54D));
            this.SpaceBox.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.SpaceBox.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.SpaceBox.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.SpaceBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.SpaceBox.Style.Visible = true;
            this.SpaceBox.Value = "";
            // 
            // WHFooterSection
            // 
            this.WHFooterSection.Height = Telerik.Reporting.Drawing.Unit.Inch(0.6D);
            this.WHFooterSection.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.PageNumberFooter});
            this.WHFooterSection.Name = "WHFooterSection";
            // 
            // PageNumberFooter
            // 
            this.PageNumberFooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(180.34D), Telerik.Reporting.Drawing.Unit.Mm(2.54D));
            this.PageNumberFooter.Name = "PageNumberFooter";
            this.PageNumberFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Mm(29.67D), Telerik.Reporting.Drawing.Unit.Mm(5.08D));
            this.PageNumberFooter.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.PageNumberFooter.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Point(0.5D);
            this.PageNumberFooter.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.PageNumberFooter.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.PageNumberFooter.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.PageNumberFooter.Style.Visible = true;
            this.PageNumberFooter.Value = "= PageNumber";
            // 
            // reportHeaderSection1
            // 
            this.reportHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.8D);
            this.reportHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.ParticipantName,
            this.SectionName});
            this.reportHeaderSection1.Name = "reportHeaderSection1";
            // 
            // WorkHistoryReport
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.WHDetailSection,
            this.WHFooterSection,
            this.reportHeaderSection1});
            this.Name = "WorkHistoryReport";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "Participant";
            reportParameter1.Text = "Participant\'s Name";
            reportParameter1.Value = "";
            this.ReportParameters.Add(reportParameter1);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.UnitOfMeasure = Telerik.Reporting.Drawing.UnitType.Inch;
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.5D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

            }
		#endregion
		private Telerik.Reporting.DetailSection WHDetailSection;
		private Telerik.Reporting.PageFooterSection WHFooterSection;
        private Telerik.Reporting.TextBox ParticipantName;
        private Telerik.Reporting.TextBox SectionName;
        private Telerik.Reporting.List EmploymentList;
        private Telerik.Reporting.Panel EmplymentList;
        private Telerik.Reporting.TextBox JobTypeLabel;
        private Telerik.Reporting.TextBox EmployerLabel;
        private Telerik.Reporting.TextBox LocationLabel;
        private Telerik.Reporting.TextBox LocationValue;
        private Telerik.Reporting.TextBox ContactLabel;
        private Telerik.Reporting.TextBox ContactNameValue;
        private Telerik.Reporting.TextBox ContactNumberValue;
        private Telerik.Reporting.TextBox DatesLabel;
        private Telerik.Reporting.TextBox BeginDateValue;
        private Telerik.Reporting.TextBox EndDateValue;
        private Telerik.Reporting.TextBox BeginDateLabel;
        private Telerik.Reporting.TextBox EndDateLabel;
        private Telerik.Reporting.TextBox PayRateLabel;
        private Telerik.Reporting.TextBox BeginPayValue;
        private Telerik.Reporting.TextBox EndPayValue;
        private Telerik.Reporting.TextBox BeginPayLabel;
        private Telerik.Reporting.TextBox EndPayLabel;
        private Telerik.Reporting.TextBox HoursValue;
        private Telerik.Reporting.TextBox HoursLabel;
        private Telerik.Reporting.TextBox PositionValue;
        private Telerik.Reporting.TextBox PositionLabel;
        private Telerik.Reporting.TextBox DutiesValue;
        private Telerik.Reporting.TextBox DutiesLabel;
        private Telerik.Reporting.TextBox JobTypeValue;
        private Telerik.Reporting.TextBox EmployerValue;
        private Telerik.Reporting.Shape EndLine;
        private Telerik.Reporting.ReportHeaderSection reportHeaderSection1;
        private Telerik.Reporting.TextBox PageNumberFooter;
        private Telerik.Reporting.TextBox SpaceBox;
    }
}