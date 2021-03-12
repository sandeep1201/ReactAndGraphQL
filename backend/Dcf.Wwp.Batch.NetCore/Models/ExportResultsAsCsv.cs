using System;
using System.Data;
using System.IO;
using System.Text;
using Dcf.Wwp.Batch.Infrastructure;
using Dcf.Wwp.Batch.Interfaces;

namespace Dcf.Wwp.Batch.Models
{
    /// <summary>
    /// Exports a Results DataTable to a Comma Separated Values file
    /// </summary>
    public class ExportResultsAsCsv : IExportable
    {
        #region Properties

        private byte[] _fileData   { get; set; }
        public  string FileName    { get; private set; }
        public  string ContentType { get; private set; }

        private string _jobName;
        private string _logDir;
        private string _outDir;

        #endregion

        #region Methods

        public ExportResultsAsCsv()
        {
            ContentType = "text/csv";
        }

        public ExportResultsAsCsv(string jobName, string logDir, string outDir)
        {
            _jobName = jobName;
            _logDir  = logDir;
            _outDir  = outDir;

            ContentType = "text/csv";
        }

        public void WriteOutput(DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException($"Argument {nameof(dataTable)} DataTable cannot be null");
            }

            using (var ms = new MemoryStream())
            {
                using (var fileWriter = new DelimitedFileWriter(ms, ",", ASCIIEncoding.ASCII))
                {
                    var iNumRows = dataTable.Rows.Count;
                    var iNumCols = dataTable.Columns.Count;

                    // quick hack because the result set is not sorted in the sproc.

                    dataTable.DefaultView.Sort = "PinNumber ASC" ;
                    var dataView = dataTable.DefaultView;

                    if (iNumRows > 0)
                    {
                        var headerRow = new Row();

                        for (var i = 0; i < iNumCols; i++)
                        {
                            var columnName = dataTable.Columns[i].ColumnName;
                            headerRow.Add(columnName);
                        }

                        fileWriter.WriteRow(headerRow);

                        for (var i = 0; i < iNumRows; i++)
                        {
                            var dataRow = dataView[i];
                            var fileRow = new Row();

                            for (var j = 0; j < iNumCols; j++)
                            {
                                if (dataRow != null)
                                {
                                    var d = dataRow[j];
                                    var t = dataRow[j].GetType().Name.ToLower();
                                    var v = t.Equals("datetime") ? ((DateTime) d).ToString("d") : d.ToString();
                                    fileRow.Add(v);
                                }
                                else
                                {
                                    fileRow.Add(string.Empty);
                                }
                            }

                            fileWriter.WriteRow(fileRow);
                        }
                    }
                }

                _fileData = ms.ToArray();

                //TODO move this to an ancestor? (it's common code)

                var jn = _jobName;
                var ld = _outDir;

                var fn = $"{ld}\\{jn}_results_{DateTime.Now:MM-dd-yyyy-hh-mm-ss-FFFFFFF}.csv";

                if (!Directory.Exists(ld))
                {
                    Directory.CreateDirectory(ld);
                    var z = 0;
                }

                File.WriteAllBytes(fn, _fileData);

                return;
            }
        }

        #endregion
    }
}

// "Work alone. Not on a committee. " ~ Steve Wozniak ~ lol
