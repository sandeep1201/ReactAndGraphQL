using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Dcf.Wwp.Batch.Infrastructure;
using log4net;
using Dcf.Wwp.Batch.Interfaces;
using Fclp.Internals.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP06 : IBatchJob
    {
        #region Properties

        private readonly IDbConfig  _dbConfig;
        private readonly ILog       _log;
        public           string     Name    => GetType().Name;
        public           string     Desc    => "Report of Workers w/Participants and no activity in past month";
        public           string     Sproc   => "";
        public           int        NumRows { get; private set; }
        private readonly DataTable  _dataTable = new DataTable();
        private readonly ISmtpEmail _smtpEmail;
        private readonly string     _jobName;
        private readonly string     _outPath;
        private readonly DateTime   _runTime;
        private readonly string     _subGuid;

        #endregion

        #region Methods

        public JWWWP06(IDbConfig dbConfig, ILog log, ISmtpEmail smtpEmail, IBatchJobOptions options)
        {
            _dbConfig  = dbConfig;
            _log       = log;
            _smtpEmail = smtpEmail;
            _jobName   = options.JobName;
            _runTime   = options.RunTime;
            _subGuid   = options.SubGuid;
            _outPath   = options.OutPath;
        }

        public DataTable Run()
        {
            _log.Info($"Running job {Name}");
            _log.Debug($"\t------------------------------------------------------------------------------------");
            _dataTable.Columns.Add("UserName",        typeof(string));
            _dataTable.Columns.Add("AgencyName",      typeof(string));
            _dataTable.Columns.Add("ParticipantName", typeof(string));
            _dataTable.Columns.Add("Program",         typeof(string));
            _dataTable.Columns.Add("PinNumber",       typeof(decimal));
            _dataTable.Columns.Add("LastLogin",       typeof(DateTime));


            var sqlSelectWkr = @"SELECT LastLogin, RTRIM(LTRIM(W.FirstName + ' ' + W.LastName)) UserName, O.AgencyName,
                                        wwp.ProperCase(RTRIM(LTRIM(RTRIM(P.FirstName) + ' ' + RTRIM(P.LastName)))) ParticipantName, P.PinNumber, EP.ShortName Program
                                 FROM wwp.Worker W
                                    INNER JOIN wwp.Organization O
                                        ON W.OrganizationId = O.Id
                                   LEFT OUTER JOIN wwp.ParticipantEnrolledProgram PEP
                                        ON PEP.WorkerId = W.Id
                                   INNER JOIN wwp.Participant P
                                        ON PEP.ParticipantId = P.Id
                                   INNER JOIN wwp.EnrolledProgram EP
                                        ON PEP.EnrolledProgramId = EP.Id AND PEP.EnrolledProgramStatusCodeId = 2
                                 WHERE LastLogin <= DATEADD(MONTH, -1, GETDATE())
                                 ORDER BY 'LastLogin', 'UserName'";

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    // read Worker, Organization, participant and Enrolled Program tables
                    var dtWkr           = ExecSql(cn, sqlSelectWkr);
                    var wkrList         = dtWkr.ConvertDataTable<Worker>(_dataTable).ToList();
                    var outputPackage   = new ExcelPackage();
                    var outputWorksheet = outputPackage.Workbook.Worksheets.Add("JWWWP06");
                    var wkrHeaders      = new List<string>() { "UserName", "AgencyName", "LastLogin", "No. of Participants" };
                    var partHeaders     = new List<string>() { "ParticipantName", "Program", "PinNumber" };
                    var row             = 1;
                    var i               = 0;

                    var reportHeader = outputWorksheet.Cells["A1:D1"];
                    reportHeader.Value           = $"Report as of {DateTime.Today:D}";
                    reportHeader.Merge           = true;
                    reportHeader.Style.Font.Bold = true;
                    reportHeader.Style.Font.Size = 16;
                    reportHeader.Style.Font.Color.SetColor(red: 68, green: 84, blue: 106, alpha: 0);
                    reportHeader.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                    reportHeader.Style.Border.Bottom.Color.SetColor(red: 68, green: 114, blue: 196, alpha: 0);
                    row += 1;

                    var wkrGroup = wkrList.ToLookup(w => w.UserName);
                    wkrGroup.ForEach(w =>
                                     {
                                         var wkr = w.FirstOrDefault();
                                         if (wkr == null) return;
                                         i = 0;
                                         wkrHeaders.ForEach(h =>
                                                            {
                                                                var userHeader = outputWorksheet.Cells[row, i + 1];
                                                                userHeader.Value           = h;
                                                                userHeader.Style.Font.Bold = true;
                                                                userHeader.Style.Font.Size = 14;
                                                                userHeader.Style.Font.Color.SetColor(red: 68, green: 84, blue: 106, alpha: 0);
                                                                userHeader.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                                                                userHeader.Style.Border.Bottom.Color.SetColor(red: 162, green: 184, blue: 225, alpha: 0);
                                                                i += 1;
                                                            });

                                         row += 1;

                                         var wkrNameCell = outputWorksheet.Cells[row, 1];
                                         wkrNameCell.Value = wkr.UserName;

                                         var wkrAgencyCell = outputWorksheet.Cells[row, 2];
                                         wkrAgencyCell.Value = wkr.AgencyName;

                                         var wkrLoginCell = outputWorksheet.Cells[row, 3];
                                         wkrLoginCell.Value = wkr.LastLogin.ToString("MM/dd/yyyy HH:mm:ss");

                                         var part       = w.Where(r => r.UserName == wkr.UserName).ToList();
                                         var partCtCell = outputWorksheet.Cells[row, 4];
                                         partCtCell.Value = part.Count;

                                         row += 1;

                                         i = 1;
                                         if (part.Count == 0) return;
                                         partHeaders.ForEach(h =>
                                                             {
                                                                 var partHeader = outputWorksheet.Cells[row, i + 1];
                                                                 partHeader.Value           = h;
                                                                 partHeader.Style.Font.Bold = true;
                                                                 partHeader.Style.Font.Size = 13;
                                                                 partHeader.Style.Font.Color.SetColor(red: 68, green: 84, blue: 106, alpha: 0);
                                                                 partHeader.Style.Border.Bottom.Style = ExcelBorderStyle.Thick;
                                                                 partHeader.Style.Border.Bottom.Color.SetColor(red: 142, green: 169, blue: 219, alpha: 0);
                                                                 i += 1;
                                                             });

                                         row += 1;
                                         part.ForEach(p =>
                                                      {
                                                          var partNameCell = outputWorksheet.Cells[row, 2];
                                                          partNameCell.Value = p.ParticipantName;

                                                          var progCell = outputWorksheet.Cells[row, 3];
                                                          progCell.Value = p.Program;

                                                          var pinCell = outputWorksheet.Cells[row, 4];
                                                          pinCell.Value = p.PinNumber;

                                                          row += 1;
                                                      });

                                         row += 1;
                                     });
                    outputWorksheet.Cells.AutoFitColumns();

                    var fileName = $"{_jobName}_Results_{_runTime:MM-dd-yyyy-hh-mm-sstt}_{_subGuid}.xlsx";
                    var fullName = $"{AppDomain.CurrentDomain.BaseDirectory}{_outPath}\\{_jobName}\\{fileName}";
                    var dirName  = Path.GetDirectoryName(fullName);
                    if (!Directory.Exists(dirName)) Directory.CreateDirectory(dirName);
                    var stream  = new FileInfo(fullName).Create();
                    var subject = "Non Active Workers w/Assigned Participant";

                    outputPackage.SaveAs(stream);
                    _smtpEmail.SendEmail(_jobName, stream, fileName, null, _dbConfig.Catalog, subject, $"Report for {subject} as of {DateTime.Today:MM/dd/yyyy}\n\nThanks,\n WWP BITS Support");
                    stream.Flush();
                }
            }
            catch (SqlException ex)
            {
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                throw;
            }


            return _dataTable;
        }

        private DataTable ExecSql(SqlConnection cn, string sql)
        {
            var dataTable = new DataTable();

            using (var cmd = cn.CreateCommand())
            {
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;


                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dataTable);
                }
            }

            return (dataTable);
        }
    }

    public class Worker
    {
        public DateTime LastLogin       { get; set; }
        public string   UserName        { get; set; }
        public string   AgencyName      { get; set; }
        public string   Program         { get; set; }
        public string   ParticipantName { get; set; }
        public decimal  PinNumber       { get; set; }
    }

    #endregion
}
