using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using log4net;
using Dcf.Wwp.Batch.Interfaces;
using TinyCsvParser;

namespace Dcf.Wwp.Batch.Models
{
    public class JWWWP04 : IBatchJob
    {
        #region Properties

        private readonly IDbConfig          _dbConfig;
        private readonly IProgramOptions    _options;
        private readonly ILog               _log;
        private readonly CsvParser<PEPLine> _parser;
        private readonly string             _fileName;

        public  string Name    => GetType().Name;
        public  string Desc    => "Disenroll CF/TJ/TMJ enrolled older than 18 months";
        public  string Sproc   => "wwp.USP_CF_Disenrollment_Update";
        public  int    NumRows { get; set; }

        #endregion

        #region Methods

        public JWWWP04(IDbConfig dbConfig, IProgramOptions options, ILog log, ICsvPathConfig csvPathConfig, CsvParser<PEPLine> parser)
        {
            _dbConfig = dbConfig;
            _options  = options;
            _log      = log;
            _parser   = parser;
            _fileName = csvPathConfig.FilePath;
        }

        public DataTable Run()
        {
            _log.Info($"Running job {Name}");

            var dataTable = new DataTable();

            NumRows = 0;
            string xml = null;

            if (_options.IsRQJ)
            {
                if (!File.Exists(_fileName))
                    throw new FileNotFoundException($"{_fileName}"); // file not found, end processing...

                var lineData = _parser.ReadFromFile(_fileName, Encoding.ASCII).ToList();
                var lines    = lineData.Where(parsedLine => parsedLine.IsValid).Select(parsedLine => parsedLine.Result).ToList();

                lineData.Where(l => !l.IsValid).ToList().ForEach(l => _log.Warn($"{l.Error}"));

                xml = new XElement("ParticipantEnrolledPrograms", lines.Select(i => new XElement("ParticipantEnrolledProgram",
                                                                                                 new XElement("PEPId",              i.PEPId),
                                                                                                 new XElement("EnrolledProgramId",  i.EnrolledProgramId),
                                                                                                 new XElement("CompletionReasonCd", i.CompletionReasonCd)))).ToString();
            }

            try
            {
                using (var cn = new SqlConnection(_dbConfig.ConnectionString))
                {
                    var cmd = cn.CreateCommand();
                    cmd.CommandText = Sproc;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 600;

                    if (xml != null)
                    {
                        var sqlParm = new SqlParameter
                                      {
                                          ParameterName = "@XML",
                                          Direction     = ParameterDirection.Input,
                                          Value         = xml,
                                          DbType        = DbType.Xml
                                      };

                        cmd.Parameters.Add(sqlParm);
                    }

                    if (_options.IsSimulation)
                    {
                        var sqlParm = new SqlParameter
                        {
                            ParameterName = "@Debug",
                            Direction = ParameterDirection.Input,
                            Value = true,
                            DbType = DbType.Boolean
                        };

                        cmd.Parameters.Add(sqlParm);
                    }

                    _log.Debug($"\tcalling {_dbConfig.Catalog}.{Sproc}");

                    if (cmd.Parameters.Count > 0)
                    {
                        for (var idx = 0; idx < cmd.Parameters.Count; idx++)
                        {
                            var p = cmd.Parameters[idx];
                            _log.Debug($"\twith parm {p}: {p.Value.ToString().ToLower()}");
                        }
                    }
                    else
                    {
                        _log.Debug("\twith no parms");
                    }

                    cn.Open();

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        NumRows = da.Fill(dataTable);
                        _log.Debug($"\t{Sproc} returned ({NumRows}) rows in result set");
                    }
                }
            }
            catch (SqlException ex)
            {
                _log.Error(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                throw;
            }

            return (dataTable);
        }
    }

    #endregion
}
