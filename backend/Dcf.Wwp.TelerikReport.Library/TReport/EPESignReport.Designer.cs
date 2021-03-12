namespace Dcf.Wwp.TelerikReport.Library.TReport
{
    partial class EPESignReport
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EPESignReport));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.EPSignDetails = new Telerik.Reporting.DetailSection();
            this.SignHeader = new Telerik.Reporting.TextBox();
            this.Acknowledgement = new Telerik.Reporting.HtmlTextBox();
            this.AckCheck = new Telerik.Reporting.CheckBox();
            this.FNHeader = new Telerik.Reporting.TextBox();
            this.MIHeader = new Telerik.Reporting.TextBox();
            this.LNHeader = new Telerik.Reporting.TextBox();
            this.FNValue = new Telerik.Reporting.TextBox();
            this.MIValue = new Telerik.Reporting.TextBox();
            this.LNValue = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // EPSignDetails
            // 
            this.EPSignDetails.CanShrink = false;
            this.EPSignDetails.Height = Telerik.Reporting.Drawing.Unit.Inch(4.1D);
            this.EPSignDetails.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.Acknowledgement,
            this.AckCheck,
            this.FNHeader,
            this.MIHeader,
            this.LNHeader,
            this.FNValue,
            this.MIValue,
            this.LNValue,
            this.SignHeader});
            this.EPSignDetails.KeepTogether = true;
            this.EPSignDetails.Name = "EPSignDetails";
            // 
            // SignHeader
            // 
            this.SignHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Mm(20.35D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.SignHeader.Name = "SignHeader";
            this.SignHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.199D), Telerik.Reporting.Drawing.Unit.Inch(0.4D));
            this.SignHeader.Style.Font.Bold = true;
            this.SignHeader.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
            this.SignHeader.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.SignHeader.Value = "Electronic Signature Acknowledgement";
            // 
            // Acknowledgement
            // 
            this.Acknowledgement.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.801D), Telerik.Reporting.Drawing.Unit.Inch(0.8D));
            this.Acknowledgement.Name = "Acknowledgement";
            this.Acknowledgement.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.199D), Telerik.Reporting.Drawing.Unit.Inch(2.2D));
            this.Acknowledgement.Value = resources.GetString("Acknowledgement.Value");
            // 
            // AckCheck
            // 
            this.AckCheck.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.801D), Telerik.Reporting.Drawing.Unit.Inch(3.2D));
            this.AckCheck.Name = "AckCheck";
            this.AckCheck.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.199D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.AckCheck.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.AckCheck.Text = "  By checking this box and typing my name below, I am electronically signing my E" +
    "mployability Plan.";
            this.AckCheck.Value = "= True";
            // 
            // FNHeader
            // 
            this.FNHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.801D), Telerik.Reporting.Drawing.Unit.Inch(3.6D));
            this.FNHeader.Name = "FNHeader";
            this.FNHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.799D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.FNHeader.Style.Font.Bold = true;
            this.FNHeader.Value = "First Name";
            // 
            // MIHeader
            // 
            this.MIHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(3.6D));
            this.MIHeader.Name = "MIHeader";
            this.MIHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.899D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.MIHeader.Style.Font.Bold = true;
            this.MIHeader.Value = "Middle Initial";
            // 
            // LNHeader
            // 
            this.LNHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.301D), Telerik.Reporting.Drawing.Unit.Inch(3.6D));
            this.LNHeader.Name = "LNHeader";
            this.LNHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.198D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.LNHeader.Style.Font.Bold = true;
            this.LNHeader.Value = "Last Name";
            // 
            // FNValue
            // 
            this.FNValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.801D), Telerik.Reporting.Drawing.Unit.Inch(3.9D));
            this.FNValue.Name = "FNValue";
            this.FNValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.099D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.FNValue.Value = "= Parameters.FirstName.Value";
            // 
            // MIValue
            // 
            this.MIValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(3.9D));
            this.MIValue.Name = "MIValue";
            this.MIValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.MIValue.Value = "= Parameters.MI.Value";
            // 
            // LNValue
            // 
            this.LNValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.301D), Telerik.Reporting.Drawing.Unit.Inch(3.9D));
            this.LNValue.Name = "LNValue";
            this.LNValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.299D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
            this.LNValue.Value = "= Parameters.LastName.Value";
            // 
            // EPESignReport
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.EPSignDetails});
            this.Name = "EPESignReport";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Mm(10D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "FirstName";
            reportParameter1.Text = "FirstName";
            reportParameter1.Value = "FirstName";
            reportParameter2.Name = "MI";
            reportParameter2.Text = "MI";
            reportParameter2.Value = "MI";
            reportParameter3.Name = "LastName";
            reportParameter3.Text = "LastName";
            reportParameter3.Value = "LastName";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(8.5D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection EPSignDetails;
        private Telerik.Reporting.TextBox SignHeader;
        private Telerik.Reporting.HtmlTextBox Acknowledgement;
        private Telerik.Reporting.CheckBox AckCheck;
        private Telerik.Reporting.TextBox FNHeader;
        private Telerik.Reporting.TextBox MIHeader;
        private Telerik.Reporting.TextBox LNHeader;
        private Telerik.Reporting.TextBox FNValue;
        private Telerik.Reporting.TextBox MIValue;
        private Telerik.Reporting.TextBox LNValue;
    }
}