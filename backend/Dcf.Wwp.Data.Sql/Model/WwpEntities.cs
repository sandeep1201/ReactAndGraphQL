using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DCF.Common;
using DCF.Common.Dates;
using DCF.Common.Extensions;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class WwpEntities
    {
        private static string _cs;

        internal WwpEntities(DbConnection connection) : base(connection, false)
        {
            var z = 0;
        }

        public WwpEntities(IDatabaseConfiguration config) : base(CreateEntityConnectionString(config.Server, config.Catalog, config.UserId, config.Password, config.MaxPoolSize, config.Timeout))
        {
            Database.CommandTimeout = config.Timeout;

            Database.SetInitializer<WwpEntities>(null);

            if (!string.IsNullOrEmpty(Database.Connection.ConnectionString))
            {
                _cs = Database.Connection.ConnectionString;
            }
        }

        public static string CreateEntityConnectionString(string server, string database, string userId, string password, int maxPoolSize = 100, int timeout = 60)
        {
            // <add name="WwpEntities" connectionString="metadata=res://*/Model.WwpModel.csdl|res://*/Model.WwpModel.ssdl|res://*/Model.WwpModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=DbWMAD0D2613,2025;initial catalog=WWPDEV;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
            const string appName                  = "EntityFramework";
            var          providerConnectionString = CreateSqlConnectionString(server, database, userId, password, appName, maxPoolSize);

            return providerConnectionString;
        }

        public static string CreateSqlConnectionString(string server, string database, string userId, string password, string appName, int maxPoolSize)
        {
            var sqlBuilder = new SqlConnectionStringBuilder
                             {
                                 DataSource               = server,
                                 InitialCatalog           = database,
                                 MultipleActiveResultSets = true,
                                 PersistSecurityInfo      = true,
                                 UserID                   = userId,
                                 Password                 = password,
                                 ApplicationName          = appName,
                                 MaxPoolSize              = maxPoolSize
                             };

            _cs = sqlBuilder.ConnectionString;

            return sqlBuilder.ConnectionString;
        }

        public static bool SuspendExecutionStrategy
        {
            get => (bool?) CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            set => CallContext.LogicalSetData("SuspendExecutionStrategy", value);
        }

        #region General Purpose Sproc Calls

        public virtual DataTable ExecStoredProcUsingAdo(string storedProcName, Dictionary<string, object> parms = null)
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(_cs))
            {
                var cmd = cn.CreateCommand();
                cmd.CommandText    = storedProcName;
                cmd.CommandType    = CommandType.StoredProcedure;
                cmd.CommandTimeout = 600;

                if (null != parms)
                {
                    foreach (var kvp in parms)
                    {
                        var parameter = new SqlParameter
                                        {
                                            ParameterName = "@" + kvp.Key,
                                            Direction     = ParameterDirection.Input,
                                            Value         = kvp.Value
                                        };

                        cmd.Parameters.Add(parameter);
                    }
                }

                cn.Open();

                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return (dt);
        }

        public virtual IList<T> ExecStoredProc<T>(string storedProcName, Dictionary<string, object> parms)
        {
            List<T> spResults;

            var idx       = 0;
            var sqlParams = new object[parms.Count];
            var sql       = new StringBuilder("EXEC " + storedProcName + " ");

            foreach (var kvp in parms)
            {
                //_log.DebugFormat("Parm: {0} = {1}", kvp.Key, kvp.Value);

                sql.Append("@" + kvp.Key + ", ");

                var p = new SqlParameter
                        {
                            ParameterName = "@" + kvp.Key,
                            Direction     = ParameterDirection.Input,
                            Value         = kvp.Value
                        };

                sqlParams[idx] = p;
                idx++;
            }

            spResults = Database.SqlQuery<T>(sql.ToString().Trim().TrimEnd(','), sqlParams).ToList();

            return (spResults);
        }

        public virtual void ExecStoredProc(string storedProcName, Dictionary<string, object> parms)
        {
            var idx       = 0;
            var sqlParams = new object[parms.Count];
            var sql       = new StringBuilder("EXEC " + storedProcName + " ");

            foreach (var kvp in parms)
            {
                //_log.DebugFormat("Parm: {0} = {1}", kvp.Key, kvp.Value);

                sql.Append("@" + kvp.Key + ", ");

                var p = new SqlParameter
                        {
                            ParameterName = "@" + kvp.Key,
                            Direction     = ParameterDirection.Input,
                            Value         = kvp.Value
                        };

                sqlParams[idx] = p;
                idx++;
            }

            Database.ExecuteSqlCommand(sql.ToString().Trim().TrimEnd(','), sqlParams);
        }

        public virtual int GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms)
        {
            var idx       = 0;
            var sqlParams = new object[parms.Count];
            var sql       = new StringBuilder("EXEC " + storedProcName + " ");

            foreach (var kvp in parms)
            {
                //_log.DebugFormat("Parm: {0} = {1}", kvp.Key, kvp.Value);

                sql.Append("@" + kvp.Key + ", ");

                var p = new SqlParameter
                        {
                            ParameterName = "@" + kvp.Key,
                            Direction     = ParameterDirection.Input,
                            Value         = kvp.Value
                        };

                sqlParams[idx] = p;
                idx++;
            }

            var rs = Database.ExecuteSqlCommand(sql.ToString().Trim().TrimEnd(','), sqlParams);

            return (rs);
        }

        public virtual IList<T> ExecStoredProc<T>(string storedProcName)
        {
            IList<T> spResults;

            var sql = new StringBuilder("EXEC " + storedProcName + " ");

            spResults = Database.SqlQuery<T>(sql.ToString().Trim().TrimEnd(',')).ToList();

            return (spResults);
        }

        #endregion

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //	if (_DbConnection != null)
        //	{
        //		optionsBuilder.UseSqlCe(_DbConnection);
        //	}
        //	else
        //	{

        //		string connectionString = "data source=DbWMAD0D2613,2025;initial catalog=WWPDEV;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";
        //		optionsBuilder.UseSqlServer(connectionString);
        //	}
        //}

        #region Timelimits StoredProcedures

        // Stored Procedures - These are manually mapped manually so we can use Async calls and because they do not prvoide return type info to the EDMX
        public virtual Task<int> InsertDb2T0459InW2LimitsAsync(string pinNum, string benefitMm, string historySeqNum, string clockTypeCd, string creTranCd, string fedClockInd, string fedCmpMthNum, string fedMaxMthNum, string historyCd, string otCmpMthNum, string overrideReasonCd, string totCmpMthNum, string totMaxMthNum, string updatedDt, string userId, string wwCmpMthNum, string wwMaxMthNum, string commentTxt, CancellationToken token = default(CancellationToken))
        {
            var pinNumParam = new SqlParameter { ParameterName = "PIN_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNum, Size = 10 };

            if (pinNumParam.Value == null)
            {
                pinNumParam.Value = DBNull.Value;
            }

            var benefitMmParam = new SqlParameter { ParameterName = "BENEFIT_MM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = benefitMm, Size = 6 };

            if (benefitMmParam.Value == null)
            {
                benefitMmParam.Value = DBNull.Value;
            }

            var historySeqNumParam = new SqlParameter { ParameterName = "HISTORY_SEQ_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = historySeqNum, Size = 4 };

            if (historySeqNumParam.Value == null)
            {
                historySeqNumParam.Value = DBNull.Value;
            }

            var clockTypeCdParam = new SqlParameter { ParameterName = "CLOCK_TYPE_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = clockTypeCd, Size = 4 };

            if (clockTypeCdParam.Value == null)
            {
                clockTypeCdParam.Value = DBNull.Value;
            }

            var creTranCdParam = new SqlParameter { ParameterName = "CRE_TRAN_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = creTranCd, Size = 8 };

            if (creTranCdParam.Value == null)
            {
                creTranCdParam.Value = DBNull.Value;
            }

            var fedClockIndParam = new SqlParameter { ParameterName = "FED_CLOCK_IND", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedClockInd, Size = 1 };

            if (fedClockIndParam.Value == null)
            {
                fedClockIndParam.Value = DBNull.Value;
            }

            var fedCmpMthNumParam = new SqlParameter { ParameterName = "FED_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedCmpMthNum, Size = 4 };

            if (fedCmpMthNumParam.Value == null)
            {
                fedCmpMthNumParam.Value = DBNull.Value;
            }

            var fedMaxMthNumParam = new SqlParameter { ParameterName = "FED_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedMaxMthNum, Size = 4 };

            if (fedMaxMthNumParam.Value == null)
            {
                fedMaxMthNumParam.Value = DBNull.Value;
            }

            var historyCdParam = new SqlParameter { ParameterName = "HISTORY_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = historyCd, Size = 4 };

            if (historyCdParam.Value == null)
            {
                historyCdParam.Value = DBNull.Value;
            }

            var otCmpMthNumParam = new SqlParameter { ParameterName = "OT_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = otCmpMthNum, Size = 4 };

            if (otCmpMthNumParam.Value == null)
            {
                otCmpMthNumParam.Value = DBNull.Value;
            }

            var overrideReasonCdParam = new SqlParameter { ParameterName = "OVERRIDE_REASON_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = overrideReasonCd, Size = 3 };

            if (overrideReasonCdParam.Value == null)
            {
                overrideReasonCdParam.Value = DBNull.Value;
            }

            var totCmpMthNumParam = new SqlParameter { ParameterName = "TOT_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = totCmpMthNum, Size = 4 };

            if (totCmpMthNumParam.Value == null)
            {
                totCmpMthNumParam.Value = DBNull.Value;
            }

            var totMaxMthNumParam = new SqlParameter { ParameterName = "TOT_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = totMaxMthNum, Size = 4 };

            if (totMaxMthNumParam.Value == null)
            {
                totMaxMthNumParam.Value = DBNull.Value;
            }

            var updatedDtParam = new SqlParameter { ParameterName = "UPDATED_DT", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = updatedDt, Size = 10 };

            if (updatedDtParam.Value == null)
            {
                updatedDtParam.Value = DBNull.Value;
            }

            var userIdParam = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = userId, Size = 6 };

            if (userIdParam.Value == null)
            {
                userIdParam.Value = DBNull.Value;
            }

            var wwCmpMthNumParam = new SqlParameter { ParameterName = "WW_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = wwCmpMthNum, Size = 4 };

            if (wwCmpMthNumParam.Value == null)
            {
                wwCmpMthNumParam.Value = DBNull.Value;
            }

            var wwMaxMthNumParam = new SqlParameter { ParameterName = "WW_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = wwMaxMthNum, Size = 4 };

            if (wwMaxMthNumParam.Value == null)
            {
                wwMaxMthNumParam.Value = DBNull.Value;
            }

            var commentTxtParam = new SqlParameter { ParameterName = "COMMENT_TXT", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = commentTxt, Size = 75 };

            if (commentTxtParam.Value == null)
            {
                commentTxtParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };

            return Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [wwp].[InsertDB2_T0459_IN_W2_LIMITS] @PIN_NUM, @BENEFIT_MM, @HISTORY_SEQ_NUM, @CLOCK_TYPE_CD, @CRE_TRAN_CD, @FED_CLOCK_IND, @FED_CMP_MTH_NUM, @FED_MAX_MTH_NUM, @HISTORY_CD, @OT_CMP_MTH_NUM, @OVERRIDE_REASON_CD, @TOT_CMP_MTH_NUM, @TOT_MAX_MTH_NUM, @UPDATED_DT, @USER_ID, @WW_CMP_MTH_NUM, @WW_MAX_MTH_NUM, @COMMENT_TXT", token, pinNumParam, benefitMmParam, historySeqNumParam, clockTypeCdParam, creTranCdParam, fedClockIndParam, fedCmpMthNumParam, fedMaxMthNumParam, historyCdParam, otCmpMthNumParam, overrideReasonCdParam, totCmpMthNumParam, totMaxMthNumParam, updatedDtParam, userIdParam, wwCmpMthNumParam, wwMaxMthNumParam, commentTxtParam, procResultParam);
        }

        public virtual List<AuxiliaryPayment> SpAuxiliaryPayment(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData  = Database.SqlQuery<AuxiliaryPayment>("EXEC @procResult = [wwp].[SP_AuxiliaryPayment] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public virtual Task<List<AuxiliaryPayment>> SpAuxiliaryPaymentAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<AuxiliaryPayment>("EXEC [wwp].[SP_AuxiliaryPayment] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual Task<Participant> SpRefreshParticipantAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<Participant>("EXEC [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).FirstOrDefaultAsync(token);

            //var result = await this.Database.SqlQuery<SP_TimeLimitParticipant_Result>("EXEC [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).FirstOrDefaultAsync(token);
            //var participant = this.MapTimeLimitParticpant_Result_To_Participant(result);
            //return participant;
        }

        public Participant SpRefreshParticipant(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var participant = Database.SqlQuery<Participant>("EXEC [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).FirstOrDefault();
            //var participant = this.MapTimeLimitParticpant_Result_To_Participant(result);
            return participant;
        }

        private Participant MapTimeLimitParticpant_Result_To_Participant(SP_TimeLimitParticipant_Result result)
        {
            var participant = Participants.Create();
            participant.Id                       = result.Id;
            participant.PinNumber                = result.PinNumber;
            participant.FirstName                = result.FirstName;
            participant.MiddleInitialName        = result.MiddleInitialName;
            participant.LastName                 = result.LastName;
            participant.SuffixName               = result.SuffixName;
            participant.DateOfBirth              = result.DateOfBirth;
            participant.DateOfDeath              = result.DateOfDeath;
            participant.GenderIndicator          = result.GenderIndicator;
            participant.AliasResponse            = result.AliasResponse;
            participant.LanguageCode             = result.LanguageCode;
            participant.MaxHistorySequenceNumber = result.MaxHistorySequenceNumber;
            participant.RaceCode                 = result.RaceCode;
            participant.USCitizenSwitch          = result.USCitizenSwitch;
            participant.AmericanIndianIndicator  = result.AmericanIndianIndicator;
            participant.AsianIndicator           = result.AsianIndicator;
            participant.BlackIndicator           = result.BlackIndicator;
            participant.HispanicIndicator        = result.HispanicIndicator;
            participant.PacificIslanderIndicator = result.PacificIslanderIndicator;
            participant.WhiteIndicator           = result.WhiteIndicator;
            participant.MCI_ID                   = result.MCI_ID;
            participant.TribalMemberIndicator    = result.TribalMemberIndicator;
            participant.ConversionProjectDetails = result.ConversionProjectDetails;
            participant.ConversionDate           = result.ConversionDate;
            participant.IsDeleted                = result.IsDeleted;
            participant.CreatedDate              = result.CreatedDate;
            participant.ModifiedBy               = result.ModifiedBy;
            participant.ModifiedDate             = result.ModifiedDate;
            participant.RowVersion               = result.RowVersion;
            participant.TimeLimitStatus          = result.TimeLimitStatus;

            return participant;
        }

        public virtual Task<List<SpAlienStatusResult>> SpAlienStatusAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SpAlienStatusResult>("EXEC [wwp].[SP_TimeLimit_AlienStatusCodes] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual List<SpAlienStatusResult> SpAlienStatus(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SpAlienStatusResult>("EXEC [wwp].[SP_TimeLimit_AlienStatusCodes] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToList();
        }

        public virtual Task<int> SpBatchParticipantAsync(DateTime month, CancellationToken token = default(CancellationToken))
        {
            var newDateParam = new SqlParameter { ParameterName = "NewDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = month.StartOf(DateTimeUnit.Month).ToString("yyyy-MM-dd"), Size = 10 };
            var endDateParam = new SqlParameter { ParameterName = "EndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = month.EndOf(DateTimeUnit.Month).ToString("yyyy-MM-dd"), Size   = 10 };

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var tmp             = Database.ExecuteSqlCommandAsync("EXEC @procResult = [wwp].[SP_BatchTimeLimitParticipant] @NewDate, @EndDate, @SchemaName", token, newDateParam, endDateParam, schemaNameParam, procResultParam);

            return (tmp);
        }

        public virtual int SpBatchParticipant(DateTime month)
        {
            var newDateParam = new SqlParameter { ParameterName = "NewDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = month.StartOf(DateTimeUnit.Month).ToString("yyyy-MM-dd"), Size = 10 };
            var endDateParam = new SqlParameter { ParameterName = "EndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = month.EndOf(DateTimeUnit.Month).ToString("yyyy-MM-dd"), Size   = 10 };

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            return Database.ExecuteSqlCommand("EXEC @procResult = [wwp].[SP_BatchTimeLimitParticipant] @NewDate, @EndDate, @SchemaName", newDateParam, endDateParam, schemaNameParam, procResultParam);
        }

        public virtual List<SpTimelimitPlacementSummaryReturnModel> SpTimelimitPlacementSummary(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultData = Database.SqlQuery<SpTimelimitPlacementSummaryReturnModel>("EXEC [wwp].[SP_TimeLimit_PlacementSummary] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToList();

            return procResultData;
        }

        public virtual async Task<List<SpTimelimitPlacementSummaryReturnModel>> SpTimelimitPlacementSummaryAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultData = await Database.SqlQuery<SpTimelimitPlacementSummaryReturnModel>("EXEC [wwp].[SP_TimeLimit_PlacementSummary] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);

            return procResultData;
        }

        public virtual List<SpConfidentialCaseReturnModel> SpConfidentialCase(string caseNumber)
        {
            var caseNumberParam = new SqlParameter { ParameterName = "CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber, Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData  = Database.SqlQuery<SpConfidentialCaseReturnModel>("EXEC @procResult = [wwp].[SP_ConfidentialCase] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public virtual Task<List<SpConfidentialCaseReturnModel>> SpConfidentialCaseAsync(string caseNumber, CancellationToken token = default(CancellationToken))
        {
            var caseNumberParam = new SqlParameter { ParameterName = "CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber, Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SpConfidentialCaseReturnModel>("EXEC [wwp].[SP_ConfidentialCase] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual List<SpOtherParticipantReturnModel> SpOtherParticipant(string pinNumber, DateTime beginDate, DateTime endDate)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var beginDateParam = new SqlParameter { ParameterName = "BeginDate", SqlDbType = SqlDbType.Date, Value = beginDate };
            var endDateParam   = new SqlParameter { ParameterName = "EndDate", SqlDbType   = SqlDbType.Date, Value = endDate };

            return Database.SqlQuery<SpOtherParticipantReturnModel>("EXEC [wwp].[SP_OtherParticipant] @PinNumber, @BeginDate, @EndDate, @SchemaName", pinNumberParam, beginDateParam, endDateParam, schemaNameParam).ToList();
        }

        public virtual Task<List<SpOtherParticipantReturnModel>> SpOtherParticipantAsync(string pinNumber, DateTime beginDate, DateTime endDate, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var beginDateParam = new SqlParameter { ParameterName = "BeginDate", SqlDbType = SqlDbType.Date, Value = beginDate };
            var endDateParam   = new SqlParameter { ParameterName = "EndDate", SqlDbType   = SqlDbType.Date, Value = endDate };

            return Database.SqlQuery<SpOtherParticipantReturnModel>("EXEC [wwp].[SP_OtherParticipant] @PinNumber, @BeginDate, @EndDate, @SchemaName", pinNumberParam, beginDateParam, endDateParam, schemaNameParam).ToListAsync(token);
        }

        public virtual Task<List<SP_ParticpantsChildrenReturnType>> SPParticpantsChildrenFromCARESAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SP_ParticpantsChildrenReturnType>("EXEC [wwp].[SP_ParticpantsChildrenFromCARES] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual List<SP_ParticpantsChildrenReturnType> SPParticpantsChildrenFromCARES(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SP_ParticpantsChildrenReturnType>("EXEC [wwp].[SP_ParticpantsChildrenFromCARES] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToList();
        }

        public virtual List<Participant> SpTimeLimitParticipant(string pinNumber)
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var procResultParam = new SqlParameter { ParameterName = "procResult", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Output };
            var procResultData  = Database.SqlQuery<Participant>("EXEC @procResult = [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public virtual Task<List<Participant>> SpTimeLimitParticipantAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber, Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<Participant>("EXEC [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual Task<List<SPW2PaymentInfoResult>> SpW2PaymentInfoAsync(string caseNumber, CancellationToken token = default(CancellationToken))
        {
            var caseNumberParam = new SqlParameter { ParameterName = "CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber, Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SPW2PaymentInfoResult>("EXEC [wwp].[SP_W2PaymentInfo] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam).ToListAsync(token);
        }

        public virtual List<SPW2PaymentInfoResult> SpW2PaymentInfo(string caseNumber)
        {
            var caseNumberParam = new SqlParameter { ParameterName = "CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber, Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var schemaNameParam = new SqlParameter { ParameterName = "SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            return Database.SqlQuery<SPW2PaymentInfoResult>("EXEC [wwp].[SP_W2PaymentInfo] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam).ToList();
        }

        //private DbRawSqlQuery<T> _doSqlQuery<T>(String sql, params Object[] paramObjects)
        //{
        //    return this.Database.SqlQuery<T>(sql, paramObjects);
        //}

        public virtual Task<int> UpdateDb2T0459InW2LimitsAsync(string pinNum, string benefitMm, string historySeqNum, string clockTypeCd, string creTranCd, string fedClockInd, string fedCmpMthNum, string fedMaxMthNum, string historyCd, string otCmpMthNum, string overrideReasonCd, string totCmpMthNum, string totMaxMthNum, string updatedDt, string userId, string wwCmpMthNum, string wwMaxMthNum, string commentTxt, CancellationToken token = default(CancellationToken))
        {
            var pinNumParam = new SqlParameter { ParameterName = "PIN_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNum, Size = 10 };

            if (pinNumParam.Value == null)
            {
                pinNumParam.Value = DBNull.Value;
            }

            var benefitMmParam = new SqlParameter { ParameterName = "BENEFIT_MM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = benefitMm, Size = 6 };

            if (benefitMmParam.Value == null)
            {
                benefitMmParam.Value = DBNull.Value;
            }

            var historySeqNumParam = new SqlParameter { ParameterName = "HISTORY_SEQ_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = historySeqNum, Size = 4 };

            if (historySeqNumParam.Value == null)
            {
                historySeqNumParam.Value = DBNull.Value;
            }

            var clockTypeCdParam = new SqlParameter { ParameterName = "CLOCK_TYPE_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = clockTypeCd, Size = 4 };

            if (clockTypeCdParam.Value == null)
            {
                clockTypeCdParam.Value = DBNull.Value;
            }

            var creTranCdParam = new SqlParameter { ParameterName = "CRE_TRAN_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = creTranCd, Size = 8 };

            if (creTranCdParam.Value == null)
            {
                creTranCdParam.Value = DBNull.Value;
            }

            var fedClockIndParam = new SqlParameter { ParameterName = "FED_CLOCK_IND", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedClockInd, Size = 1 };

            if (fedClockIndParam.Value == null)
            {
                fedClockIndParam.Value = DBNull.Value;
            }

            var fedCmpMthNumParam = new SqlParameter { ParameterName = "FED_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedCmpMthNum, Size = 4 };

            if (fedCmpMthNumParam.Value == null)
            {
                fedCmpMthNumParam.Value = DBNull.Value;
            }

            var fedMaxMthNumParam = new SqlParameter { ParameterName = "FED_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = fedMaxMthNum, Size = 4 };

            if (fedMaxMthNumParam.Value == null)
            {
                fedMaxMthNumParam.Value = DBNull.Value;
            }

            var historyCdParam = new SqlParameter { ParameterName = "HISTORY_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = historyCd, Size = 4 };

            if (historyCdParam.Value == null)
            {
                historyCdParam.Value = DBNull.Value;
            }

            var otCmpMthNumParam = new SqlParameter { ParameterName = "OT_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = otCmpMthNum, Size = 4 };

            if (otCmpMthNumParam.Value == null)
            {
                otCmpMthNumParam.Value = DBNull.Value;
            }

            var overrideReasonCdParam = new SqlParameter { ParameterName = "OVERRIDE_REASON_CD", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = overrideReasonCd, Size = 3 };

            if (overrideReasonCdParam.Value == null)
            {
                overrideReasonCdParam.Value = DBNull.Value;
            }

            var totCmpMthNumParam = new SqlParameter { ParameterName = "TOT_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = totCmpMthNum, Size = 4 };

            if (totCmpMthNumParam.Value == null)
            {
                totCmpMthNumParam.Value = DBNull.Value;
            }

            var totMaxMthNumParam = new SqlParameter { ParameterName = "TOT_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = totMaxMthNum, Size = 4 };

            if (totMaxMthNumParam.Value == null)
            {
                totMaxMthNumParam.Value = DBNull.Value;
            }

            var updatedDtParam = new SqlParameter { ParameterName = "UPDATED_DT", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = updatedDt, Size = 10 };

            if (updatedDtParam.Value == null)
            {
                updatedDtParam.Value = DBNull.Value;
            }

            var userIdParam = new SqlParameter { ParameterName = "USER_ID", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = userId, Size = 6 };

            if (userIdParam.Value == null)
            {
                userIdParam.Value = DBNull.Value;
            }

            var wwCmpMthNumParam = new SqlParameter { ParameterName = "WW_CMP_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = wwCmpMthNum, Size = 4 };

            if (wwCmpMthNumParam.Value == null)
            {
                wwCmpMthNumParam.Value = DBNull.Value;
            }

            var wwMaxMthNumParam = new SqlParameter { ParameterName = "WW_MAX_MTH_NUM", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = wwMaxMthNum, Size = 4 };

            if (wwMaxMthNumParam.Value == null)
            {
                wwMaxMthNumParam.Value = DBNull.Value;
            }

            var commentTxtParam = new SqlParameter { ParameterName = "COMMENT_TXT", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = commentTxt, Size = 75 };

            if (commentTxtParam.Value == null)
            {
                commentTxtParam.Value = DBNull.Value;
            }

            return Database.ExecuteSqlCommandAsync(TransactionalBehavior.DoNotEnsureTransaction, "EXEC [wwp].[UpdateDB2_T0459_IN_W2_LIMITS] @PIN_NUM, @BENEFIT_MM, @HISTORY_SEQ_NUM, @CLOCK_TYPE_CD, @CRE_TRAN_CD, @FED_CLOCK_IND, @FED_CMP_MTH_NUM, @FED_MAX_MTH_NUM, @HISTORY_CD, @OT_CMP_MTH_NUM, @OVERRIDE_REASON_CD, @TOT_CMP_MTH_NUM, @TOT_MAX_MTH_NUM, @UPDATED_DT, @USER_ID, @WW_CMP_MTH_NUM, @WW_MAX_MTH_NUM, @COMMENT_TXT", token, pinNumParam, benefitMmParam, historySeqNumParam, clockTypeCdParam, creTranCdParam, fedClockIndParam, fedCmpMthNumParam, fedMaxMthNumParam, historyCdParam, otCmpMthNumParam, overrideReasonCdParam, totCmpMthNumParam, totMaxMthNumParam, updatedDtParam, userIdParam, wwCmpMthNumParam, wwMaxMthNumParam, commentTxtParam);
        }

        public virtual void SpTimeLimitPlacementClosure(decimal caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode)
        {
            var schemaNameParam = new SqlParameter { ParameterName = "@SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var caseNumberParam = new SqlParameter { ParameterName = "@CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber.ToString(), Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var databaseDateParam = new SqlParameter { ParameterName = "@DatabaseDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = databaseDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (databaseDateParam.Value == null)
            {
                databaseDateParam.Value = DBNull.Value;
            }

            var inputUserIdParam = new SqlParameter { ParameterName = "@InputUserId", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = inputUserId, Size = 6 };

            if (inputUserIdParam.Value == null)
            {
                inputUserIdParam.Value = DBNull.Value;
            }

            var existingEpisodeBeginDateParam = new SqlParameter { ParameterName = "@ExistingEpisodeBeginDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingEpisodeBeginDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingEpisodeBeginDateParam.Value == null)
            {
                existingEpisodeBeginDateParam.Value = DBNull.Value;
            }

            var pinNumberParam = new SqlParameter { ParameterName = "@PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber.ToString(), Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var existingFepIdParam = new SqlParameter { ParameterName = "@ExistingFepId", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingFepId, Size = 6 };

            if (existingFepIdParam.Value == null)
            {
                existingFepIdParam.Value = DBNull.Value;
            }

            var existingEpisodeEndDateParam = new SqlParameter { ParameterName = "@ExistingEpisodeEndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingEpisodeEndDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingEpisodeEndDateParam.Value == null)
            {
                existingEpisodeEndDateParam.Value = DBNull.Value;
            }

            var existingPlacementCodeParam = new SqlParameter { ParameterName = "@ExistingPlacementCode", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingPlacementCode, Size = 3 };

            if (existingPlacementCodeParam.Value == null)
            {
                existingPlacementCodeParam.Value = DBNull.Value;
            }

            var existingPlacementBeginDateParam = new SqlParameter { ParameterName = "@ExistingPlacementBeginDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingPlacementBeginDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingPlacementBeginDateParam.Value == null)
            {
                existingPlacementBeginDateParam.Value = DBNull.Value;
            }

            var newPinNumberParam = new SqlParameter { ParameterName = "@NewPinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber.ToString(), Size = 10 };

            if (newPinNumberParam.Value == null)
            {
                newPinNumberParam.Value = DBNull.Value;
            }

            var newFepIdNumberParam = new SqlParameter { ParameterName = "@NewFepIdNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newFepIdNumber, Size = 6 };

            if (newFepIdNumberParam.Value == null)
            {
                newFepIdNumberParam.Value = DBNull.Value;
            }

            var newEpisodeEndDateParam = new SqlParameter { ParameterName = "@NewEpisodeEndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newEpisodeEndDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (newEpisodeEndDateParam.Value == null)
            {
                newEpisodeEndDateParam.Value = DBNull.Value;
            }

            var newPlacementCodeParam = new SqlParameter { ParameterName = "@NewPlacementCode", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newPlacementCode, Size = 3 };

            if (newPlacementCodeParam.Value == null)
            {
                newPlacementCodeParam.Value = DBNull.Value;
            }

            Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC [wwp].[SP_TimeLimit_PlacementClosure] @SchemaName, @CaseNumber, @DatabaseDate, @InputUserId, @ExistingEpisodeBeginDate, @PinNumber, @ExistingFepId, @ExistingEpisodeEndDate, @ExistingPlacementCode, @ExistingPlacementBeginDate, @NewPinNumber, @NewFepIdNumber, @NewEpisodeEndDate, @NewPlacementCode", schemaNameParam, caseNumberParam, databaseDateParam, inputUserIdParam, existingEpisodeBeginDateParam, pinNumberParam, existingFepIdParam, existingEpisodeEndDateParam, existingPlacementCodeParam, existingPlacementBeginDateParam, newPinNumberParam, newFepIdNumberParam, newEpisodeEndDateParam, newPlacementCodeParam);
        }

        public Task SpTimeLimitPlacementClosureAsync(decimal caseNumber, DateTime databaseDate, string inputUserId, DateTime existingEpisodeBeginDate, decimal pinNumber, string existingFepId, DateTime existingEpisodeEndDate, string existingPlacementCode, DateTime existingPlacementBeginDate, string newFepIdNumber, DateTime newEpisodeEndDate, string newPlacementCode, CancellationToken token = default(CancellationToken))
        {
            //var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = schemaName, Size = 20 };
            //if (schemaNameParam.Value == null)
            //    schemaNameParam.Value = System.DBNull.Value;

            var schemaNameParam = new SqlParameter { ParameterName = "@SchemaName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = Database.Connection.Database, Size = 20 };

            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = DBNull.Value;
            }

            var caseNumberParam = new SqlParameter { ParameterName = "@CaseNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = caseNumber.ToString(), Size = 10 };

            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = DBNull.Value;
            }

            var databaseDateParam = new SqlParameter { ParameterName = "@DatabaseDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = databaseDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (databaseDateParam.Value == null)
            {
                databaseDateParam.Value = DBNull.Value;
            }

            var inputUserIdParam = new SqlParameter { ParameterName = "@InputUserId", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = inputUserId, Size = 6 };

            if (inputUserIdParam.Value == null)
            {
                inputUserIdParam.Value = DBNull.Value;
            }

            var existingEpisodeBeginDateParam = new SqlParameter { ParameterName = "@ExistingEpisodeBeginDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingEpisodeBeginDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingEpisodeBeginDateParam.Value == null)
            {
                existingEpisodeBeginDateParam.Value = DBNull.Value;
            }

            var pinNumberParam = new SqlParameter { ParameterName = "@PinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber.ToString(), Size = 10 };

            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = DBNull.Value;
            }

            var existingFepIdParam = new SqlParameter { ParameterName = "@ExistingFepId", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingFepId, Size = 6 };

            if (existingFepIdParam.Value == null)
            {
                existingFepIdParam.Value = DBNull.Value;
            }

            var existingEpisodeEndDateParam = new SqlParameter { ParameterName = "@ExistingEpisodeEndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingEpisodeEndDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingEpisodeEndDateParam.Value == null)
            {
                existingEpisodeEndDateParam.Value = DBNull.Value;
            }

            var existingPlacementCodeParam = new SqlParameter { ParameterName = "@ExistingPlacementCode", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingPlacementCode, Size = 3 };

            if (existingPlacementCodeParam.Value == null)
            {
                existingPlacementCodeParam.Value = DBNull.Value;
            }

            var existingPlacementBeginDateParam = new SqlParameter { ParameterName = "@ExistingPlacementBeginDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = existingPlacementBeginDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (existingPlacementBeginDateParam.Value == null)
            {
                existingPlacementBeginDateParam.Value = DBNull.Value;
            }

            var newPinNumberParam = new SqlParameter { ParameterName = "@NewPinNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = pinNumber.ToString(), Size = 10 };

            if (newPinNumberParam.Value == null)
            {
                newPinNumberParam.Value = DBNull.Value;
            }

            var newFepIdNumberParam = new SqlParameter { ParameterName = "@NewFepIdNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newFepIdNumber, Size = 6 };

            if (newFepIdNumberParam.Value == null)
            {
                newFepIdNumberParam.Value = DBNull.Value;
            }

            var newEpisodeEndDateParam = new SqlParameter { ParameterName = "@NewEpisodeEndDate", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newEpisodeEndDate.ToString("yyyy-MM-dd"), Size = 10 };

            if (newEpisodeEndDateParam.Value == null)
            {
                newEpisodeEndDateParam.Value = DBNull.Value;
            }

            var newPlacementCodeParam = new SqlParameter { ParameterName = "@NewPlacementCode", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = newPlacementCode, Size = 3 };

            if (newPlacementCodeParam.Value == null)
            {
                newPlacementCodeParam.Value = DBNull.Value;
            }

            return Database.ExecuteSqlCommandAsync("EXEC [wwp].[SP_TimeLimit_PlacementClosure] @SchemaName, @CaseNumber, @DatabaseDate, @InputUserId, @ExistingEpisodeBeginDate, @PinNumber, @ExistingFepId, @ExistingEpisodeEndDate, @ExistingPlacementCode, @ExistingPlacementBeginDate, @NewPinNumber, @NewFepIdNumber, @NewEpisodeEndDate, @NewPlacementCode", token, schemaNameParam, caseNumberParam, databaseDateParam, inputUserIdParam, existingEpisodeBeginDateParam, pinNumberParam, existingFepIdParam, existingEpisodeEndDateParam, existingPlacementCodeParam, existingPlacementBeginDateParam, newPinNumberParam, newFepIdNumberParam, newEpisodeEndDateParam, newPlacementCodeParam); //.ContinueWith(x => { return Task.CompletedTask; }).Unwrap();
        }

        #endregion

        #region CDC

        public List<T> SPReadCDCHistory<T>(string tableName, string identityNumber) where T : new()
        {
            var tableNameParameter = new SqlParameter { ParameterName = "TableName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = tableName };

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableNameParameter.Value = DBNull.Value;
            }

            var identityNumberParameter = new SqlParameter { ParameterName = "IdentityNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = identityNumber };

            if (string.IsNullOrWhiteSpace(identityNumber))
            {
                identityNumberParameter.Value = DBNull.Value;
            }

            var results = ((IObjectContextAdapter) this).ObjectContext.ExecuteStoreQuery<T>("EXEC [wwp].[SP_ReadCDCHistory] @TableName, @IdentityNumber", tableNameParameter, identityNumberParameter);

            var tableNameParameter2 = new SqlParameter { ParameterName = "TableName", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = tableName };

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableNameParameter.Value = DBNull.Value;
            }

            var identityNumberParameter2 = new SqlParameter { ParameterName = "IdentityNumber", SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input, Value = identityNumber };

            if (string.IsNullOrWhiteSpace(identityNumber))
            {
                identityNumberParameter.Value = DBNull.Value;
            }

            return results.ToList();
        }

        public string SPReadCDCHistory(string storedProcedureName, string tableName, string pin, int? id)
        {
            var tableNameParameter = new SqlParameter { ParameterName = "TableName", SqlDbType = SqlDbType.NVarChar, Direction = ParameterDirection.Input, Value = tableName };

            if (string.IsNullOrWhiteSpace(tableName))
            {
                tableNameParameter.Value = DBNull.Value;
            }

            var pinParameter = new SqlParameter { ParameterName = "PinNumber", SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input, Value = pin };

            if (string.IsNullOrWhiteSpace(pin))
            {
                pinParameter.Value = DBNull.Value;
            }

            var idParameter = new SqlParameter { ParameterName = "Id", SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input, Value = id };

            if (id == null)
            {
                idParameter.Value = DBNull.Value;
            }

            var xmlString = "";

            if (tableName == "BarrierDetail" || tableName == "EmploymentInformation")
            {
                var idResults = Database.SqlQuery<string>("EXEC " + storedProcedureName + " @TableName, @Id", tableNameParameter, idParameter).ToList();

                foreach (var result in idResults)
                {
                    xmlString += result;
                }
            }
            else
            {
                var pinResults = Database.SqlQuery<string>("EXEC " + storedProcedureName + " @TableName, @PinNumber", tableNameParameter, pinParameter).ToList();

                foreach (var result in pinResults)
                {
                    xmlString += result;
                }
            }

            return xmlString;
        }

        #endregion
    }
}
