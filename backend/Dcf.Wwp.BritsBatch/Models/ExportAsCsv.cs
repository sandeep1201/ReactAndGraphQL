using System;
using System.Data;
using System.IO;
using System.Text;
using Dcf.Wwp.BritsBatch.Infrastructure;
using Dcf.Wwp.BritsBatch.Interfaces;
using log4net;

namespace Dcf.Wwp.BritsBatch.Models
{
    /// <summary>
    /// Exports a Results DataTable to a Comma Separated Values file
    /// </summary>
    public class ExportAsCsv : IExportable
    {
        #region Properties

        private readonly string   _jobName;
        private readonly string   _outPath;
        private readonly DateTime _runTime;
        private readonly string   _subGuid;
        private readonly ILog     _log;
        private readonly bool     _isSimulation;
        private          byte[]   _byteArray; // the data ;)

        public string FileName    { get; private set; }
        public string ContentType => "text/csv";

        #endregion

        #region Methods

        public ExportAsCsv(IBatchJobOptions options, ILog log)
        {
            _jobName      = options.JobName;
            _runTime      = options.RunTime;
            _subGuid      = options.SubGuid;
            _outPath      = options.OutPath;
            _isSimulation = options.IsSimulation;
            _log          = log;
        }

        public void Export(DataTable dataTable)
        {
            FileName = $"{AppDomain.CurrentDomain.BaseDirectory}{_outPath}\\{_jobName}\\{_jobName}_Results_{_runTime:MM-dd-yyyy-hh-mm-sstt}_{_subGuid}.csv";
            var shortName = $"\\{_outPath}\\{_jobName}\\{_jobName}_Results_{_runTime:MM-dd-yyyy-hh-mm-sstt}_{_subGuid}.csv";

            if (dataTable == null)
            {
                _log.Error($"\tArgument {nameof(dataTable)} DataTable cannot be null");

                throw new ArgumentNullException($"Argument {nameof(dataTable)} DataTable cannot be null");
            }

            var dirName = Path.GetDirectoryName(FileName);

            using (var ms = new MemoryStream())
            {
                using (var fileWriter = new DelimitedFileWriter(ms, ",", Encoding.ASCII))
                {
                    var iNumRows = dataTable.Rows.Count;
                    var iNumCols = dataTable.Columns.Count;

                    _log.Info($"Generating output for {_jobName} in csv format");
                    _log.Debug($"\treceived ({iNumRows}) rows from {_jobName}");

                    var beginDataRow = $"BEGIN DATA ({iNumRows} rows) ********{(_isSimulation ? " SIMULATION " : "".PadRight(12, '*'))}********";
                    var endDataRow = "END DATA ".PadRight(beginDataRow.Length, '*');

                    fileWriter.WriteRow(new Row { beginDataRow });

                    var headerRow = new Row();

                    for (var colIdx = 0; colIdx < iNumCols; colIdx++)
                    {
                        var columnName = dataTable.Columns[colIdx].ColumnName;
                        headerRow.Add(columnName);
                    }

                    fileWriter.WriteRow(headerRow);

                    _log.Info("");
                    _log.Info(beginDataRow);
                    _log.Info(headerRow.LineText);

                    if (iNumRows > 0)
                    {
                        var dataView = dataTable.DefaultView;

                        for (var rowIdx = 0; rowIdx < iNumRows; rowIdx++)
                        {
                            var dataRow = dataView[rowIdx];
                            var fileRow = new Row();

                            for (var colIdx = 0; colIdx < iNumCols; colIdx++)
                            {
                                if (dataRow != null)
                                {
                                    var dataCol   = dataRow[colIdx];
                                    var dataType  = dataRow[colIdx].GetType().Name.ToLower();
                                    var dataValue = dataType.Equals("datetime") ? ((DateTime) dataCol).ToString("d") : dataCol.ToString();
                                    dataValue = string.IsNullOrEmpty(dataValue) ? "null" : dataValue;

                                    fileRow.Add(dataValue);
                                }
                                else
                                {
                                    fileRow.Add(string.Empty);
                                }
                            }

                            fileWriter.WriteRow(fileRow);
                            _log.Info(fileRow.LineText.Trim(','));
                        }
                    }
                    else
                    {
                        fileWriter.WriteRow(new Row { "No data to process." });
                        _log.Info("No data to process.");
                    }

                    fileWriter.WriteRow(new Row { "END DATA ".PadRight(11, ' ').PadRight(25, '*') });
                    _log.Info(endDataRow);
                    _log.Info("");
                }

                _byteArray = ms.ToArray();

                if (!Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName ?? throw new InvalidOperationException());
                }

                _log.Info($"Writing output to {shortName}");
                _log.Debug($"\tPath: {Directory.GetParent(FileName)}");
                _log.Debug($"\tFile: {Path.GetFileName(FileName)}");

                try
                {
                    File.WriteAllBytes(FileName, _byteArray);
                }
                catch (Exception ex)
                {
                    _log.Error(ex.Message);

                    throw;
                }
            }
        }

        #endregion
    }
}
