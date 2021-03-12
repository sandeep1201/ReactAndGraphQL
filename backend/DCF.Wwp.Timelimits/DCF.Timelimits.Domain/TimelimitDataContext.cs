// <auto-generated>
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable RedundantOverridenMember
// ReSharper disable UseNameofExpression
// TargetFrameworkVersion = 4.5
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace DCF.Core.Domain
{

    using System.Linq;

    public class TimelimitDataContext : System.Data.Entity.DbContext, ITimelimitDataContext
    {
        public System.Data.Entity.DbSet<ApprovalReason> ApprovalReasons { get; set; } // ApprovalReason
        public System.Data.Entity.DbSet<AuxiliaryPayment> AuxiliaryPayments { get; set; } // AuxiliaryPayment
        public System.Data.Entity.DbSet<ChangeReason> ChangeReasons { get; set; } // ChangeReason
        public System.Data.Entity.DbSet<DeleteReason> DeleteReasons { get; set; } // DeleteReason
        public System.Data.Entity.DbSet<Participant> Participants { get; set; } // Participant
        public System.Data.Entity.DbSet<TimeLimit> TimeLimits { get; set; } // TimeLimit

        static TimelimitDataContext()
        {
            System.Data.Entity.Database.SetInitializer<TimelimitDataContext>(null);
        }

        /// <summary>
        /// Default constructor, should only be used in Local Environment to read from AppConfig
        /// </summary>
        public TimelimitDataContext() : base("Name=TimelimitsDataContext")
        {
            
        }

        public TimelimitDataContext(string server, string database, string userid, string password)
            : base(TimelimitDataContext.CreateConnectionString(server,database,userid,password))
        {
        }

        private static string CreateConnectionString(String server, String database, String userId, String password)
        {
            var sqlConnBuilder = new SqlConnectionStringBuilder();
            sqlConnBuilder.DataSource = server;
            sqlConnBuilder.InitialCatalog = database;
            sqlConnBuilder.MultipleActiveResultSets = true;
            sqlConnBuilder.PersistSecurityInfo = true;
            sqlConnBuilder.UserID = userId; //"WWPDev_App";
            sqlConnBuilder.Password = password; //"Br0cc0l1";
            return sqlConnBuilder.ConnectionString;
        }

        //public TimelimitDataContext(string connectionString)
        //    : base(connectionString)
        //{
        //}

        //public TimelimitDataContext(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
        //    : base(connectionString, model)
        //{
        //}

        //public TimelimitDataContext(System.Data.Common.DbConnection existingConnection, bool contextOwnsConnection)
        //    : base(existingConnection, contextOwnsConnection)
        //{
        //}

        //public TimelimitDataContext(System.Data.Common.DbConnection existingConnection, System.Data.Entity.Infrastructure.DbCompiledModel model, bool contextOwnsConnection)
        //    : base(existingConnection, model, contextOwnsConnection)
        //{
        //}

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public bool IsSqlParameterNull(System.Data.SqlClient.SqlParameter param)
        {
            var sqlValue = param.SqlValue;
            var nullableValue = sqlValue as System.Data.SqlTypes.INullable;
            if (nullableValue != null)
            {
                return nullableValue.IsNull;
            }
            return (sqlValue == null || sqlValue == System.DBNull.Value);
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new ApprovalReasonConfiguration());
            modelBuilder.Configurations.Add(new AuxiliaryPaymentConfiguration());
            modelBuilder.Configurations.Add(new ChangeReasonConfiguration());
            modelBuilder.Configurations.Add(new DeleteReasonConfiguration());
            modelBuilder.Configurations.Add(new ParticipantConfiguration());
            modelBuilder.Configurations.Add(new TimeLimitConfiguration());
        }

        public static System.Data.Entity.DbModelBuilder CreateModel(System.Data.Entity.DbModelBuilder modelBuilder, string schema)
        {
            modelBuilder.Configurations.Add(new ApprovalReasonConfiguration(schema));
            modelBuilder.Configurations.Add(new AuxiliaryPaymentConfiguration(schema));
            modelBuilder.Configurations.Add(new ChangeReasonConfiguration(schema));
            modelBuilder.Configurations.Add(new DeleteReasonConfiguration(schema));
            modelBuilder.Configurations.Add(new ParticipantConfiguration(schema));
            modelBuilder.Configurations.Add(new TimeLimitConfiguration(schema));
            return modelBuilder;
        }

        // Stored Procedures
        public Task<Int32> InsertDb2T0459InW2LimitsAsync(string pinNum, string benefitMm, string historySeqNum, string clockTypeCd, string creTranCd, string fedClockInd, string fedCmpMthNum, string fedMaxMthNum, string historyCd, string otCmpMthNum, string overrideReasonCd, string totCmpMthNum, string totMaxMthNum, string updatedDt, string userId, string wwCmpMthNum, string wwMaxMthNum, string commentTxt, CancellationToken token = default(CancellationToken))
        {
            var pinNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PIN_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNum, Size = 10 };
            if (pinNumParam.Value == null)
            {
                pinNumParam.Value = System.DBNull.Value;
            }

            var benefitMmParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@BENEFIT_MM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = benefitMm, Size = 6 };
            if (benefitMmParam.Value == null)
            {
                benefitMmParam.Value = System.DBNull.Value;
            }

            var historySeqNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@HISTORY_SEQ_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = historySeqNum, Size = 4 };
            if (historySeqNumParam.Value == null)
            {
                historySeqNumParam.Value = System.DBNull.Value;
            }

            var clockTypeCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CLOCK_TYPE_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = clockTypeCd, Size = 4 };
            if (clockTypeCdParam.Value == null)
            {
                clockTypeCdParam.Value = System.DBNull.Value;
            }

            var creTranCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CRE_TRAN_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = creTranCd, Size = 8 };
            if (creTranCdParam.Value == null)
            {
                creTranCdParam.Value = System.DBNull.Value;
            }

            var fedClockIndParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_CLOCK_IND", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedClockInd, Size = 1 };
            if (fedClockIndParam.Value == null)
            {
                fedClockIndParam.Value = System.DBNull.Value;
            }

            var fedCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedCmpMthNum, Size = 4 };
            if (fedCmpMthNumParam.Value == null)
            {
                fedCmpMthNumParam.Value = System.DBNull.Value;
            }

            var fedMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedMaxMthNum, Size = 4 };
            if (fedMaxMthNumParam.Value == null)
            {
                fedMaxMthNumParam.Value = System.DBNull.Value;
            }

            var historyCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@HISTORY_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = historyCd, Size = 4 };
            if (historyCdParam.Value == null)
            {
                historyCdParam.Value = System.DBNull.Value;
            }

            var otCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@OT_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = otCmpMthNum, Size = 4 };
            if (otCmpMthNumParam.Value == null)
            {
                otCmpMthNumParam.Value = System.DBNull.Value;
            }

            var overrideReasonCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@OVERRIDE_REASON_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = overrideReasonCd, Size = 3 };
            if (overrideReasonCdParam.Value == null)
            {
                overrideReasonCdParam.Value = System.DBNull.Value;
            }

            var totCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@TOT_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = totCmpMthNum, Size = 4 };
            if (totCmpMthNumParam.Value == null)
            {
                totCmpMthNumParam.Value = System.DBNull.Value;
            }

            var totMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@TOT_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = totMaxMthNum, Size = 4 };
            if (totMaxMthNumParam.Value == null)
            {
                totMaxMthNumParam.Value = System.DBNull.Value;
            }

            var updatedDtParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@UPDATED_DT", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = updatedDt, Size = 10 };
            if (updatedDtParam.Value == null)
            {
                updatedDtParam.Value = System.DBNull.Value;
            }

            var userIdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@USER_ID", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = userId, Size = 6 };
            if (userIdParam.Value == null)
            {
                userIdParam.Value = System.DBNull.Value;
            }

            var wwCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@WW_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = wwCmpMthNum, Size = 4 };
            if (wwCmpMthNumParam.Value == null)
            {
                wwCmpMthNumParam.Value = System.DBNull.Value;
            }

            var wwMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@WW_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = wwMaxMthNum, Size = 4 };
            if (wwMaxMthNumParam.Value == null)
            {
                wwMaxMthNumParam.Value = System.DBNull.Value;
            }

            var commentTxtParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@COMMENT_TXT", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = commentTxt, Size = 75 };
            if (commentTxtParam.Value == null)
            {
                commentTxtParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            return this.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC @procResult = [wwp].[InsertDB2_T0459_IN_W2_LIMITS] @PIN_NUM, @BENEFIT_MM, @HISTORY_SEQ_NUM, @CLOCK_TYPE_CD, @CRE_TRAN_CD, @FED_CLOCK_IND, @FED_CMP_MTH_NUM, @FED_MAX_MTH_NUM, @HISTORY_CD, @OT_CMP_MTH_NUM, @OVERRIDE_REASON_CD, @TOT_CMP_MTH_NUM, @TOT_MAX_MTH_NUM, @UPDATED_DT, @USER_ID, @WW_CMP_MTH_NUM, @WW_MAX_MTH_NUM, @COMMENT_TXT",token, pinNumParam, benefitMmParam, historySeqNumParam, clockTypeCdParam, creTranCdParam, fedClockIndParam, fedCmpMthNumParam, fedMaxMthNumParam, historyCdParam, otCmpMthNumParam, overrideReasonCdParam, totCmpMthNumParam, totMaxMthNumParam, updatedDtParam, userIdParam, wwCmpMthNumParam, wwMaxMthNumParam, commentTxtParam, procResultParam);
        }


        public System.Collections.Generic.List<AuxiliaryPayment> SpAuxiliaryPayment(string pinNumber)
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
            var procResultData = this.Database.SqlQuery<AuxiliaryPayment>("EXEC @procResult = [wwp].[SP_AuxiliaryPayment] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<AuxiliaryPayment>> SpAuxiliaryPaymentAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            return this.Database.SqlQuery<AuxiliaryPayment>("EXEC [wwp].[SP_AuxiliaryPayment] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);

        }

        public System.Collections.Generic.List<SpClockPlacementSummaryReturnModel> SpClockPlacementSummary(string pinNumber)
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            var procResultData  = this.Database.SqlQuery<SpClockPlacementSummaryReturnModel>("EXEC @[wwp].[SP_Clock_Placement_Summary] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public Task<List<SpClockPlacementSummaryReturnModel>> SpClockPlacementSummaryAsync(String pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            var procResultData = this.Database.SqlQuery<SpClockPlacementSummaryReturnModel>("EXEC @[wwp].[SP_Clock_Placement_Summary] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToListAsync(token);

            return procResultData;
        }

        private DbRawSqlQuery<TElement> ExecuteRawSql<TElement>(String sql, params Object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql,parameters);
        }


        public class SpClockPlacementSummaryReturnModel
        {
            public DateTime PLACEMENT_BEGIN_DATE { get; }
            public DateTime PLACEMENT_END_MONTH { get; }
            public String PLACEMENT_TYPE { get; set; }
        }

        public System.Collections.Generic.List<SpConfidentialCaseReturnModel> SpConfidentialCase(string caseNumber)
        {
            var caseNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CaseNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = caseNumber, Size = 10 };
            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
            var procResultData = this.Database.SqlQuery<SpConfidentialCaseReturnModel>("EXEC @procResult = [wwp].[SP_ConfidentialCase] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<SpConfidentialCaseReturnModel>> SpConfidentialCaseAsync(string caseNumber, CancellationToken token = default(CancellationToken))
        {
            var caseNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CaseNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = caseNumber, Size = 10 };
            if (caseNumberParam.Value == null)
            {
                caseNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            return this.Database.SqlQuery<SpConfidentialCaseReturnModel>("EXEC [wwp].[SP_ConfidentialCase] @CaseNumber, @SchemaName", caseNumberParam, schemaNameParam).ToListAsync(token);

        }

        public System.Collections.Generic.List<SpOtherParticipantReturnModel> SpOtherParticipant(string pinNumber)
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
            var procResultData = this.Database.SqlQuery<SpOtherParticipantReturnModel>("EXEC @procResult = [wwp].[SP_OtherParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();


            return procResultData;
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<SpOtherParticipantReturnModel>> SpOtherParticipantAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            return this.Database.SqlQuery<SpOtherParticipantReturnModel>("EXEC [wwp].[SP_OtherParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);

        }

        public System.Collections.Generic.List<SpTimeLimitParticipantReturnModel> SpTimeLimitParticipant(string pinNumber)
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };
            var procResultData = this.Database.SqlQuery<SpTimeLimitParticipantReturnModel>("EXEC @procResult = [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam, procResultParam).ToList();

            return procResultData;
        }

        public System.Threading.Tasks.Task<System.Collections.Generic.List<SpTimeLimitParticipantReturnModel>> SpTimeLimitParticipantAsync(string pinNumber, CancellationToken token = default(CancellationToken))
        {
            var pinNumberParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PinNumber", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNumber, Size = 10 };
            if (pinNumberParam.Value == null)
            {
                pinNumberParam.Value = System.DBNull.Value;
            }

            var schemaNameParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@SchemaName", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = this.Database.Connection.Database, Size = 20 };
            if (schemaNameParam.Value == null)
            {
                schemaNameParam.Value = System.DBNull.Value;
            }

            return this.Database.SqlQuery<SpTimeLimitParticipantReturnModel>("EXEC [wwp].[SP_TimeLimitParticipant] @PinNumber, @SchemaName", pinNumberParam, schemaNameParam).ToListAsync(token);

        }

        public Task<Int32> UpdateDb2T0459InW2LimitsAsync(String pinNum, String benefitMm, String historySeqNum, String clockTypeCd, String creTranCd, String fedClockInd, String fedCmpMthNum, String fedMaxMthNum, String historyCd, String otCmpMthNum, String overrideReasonCd, String totCmpMthNum, String totMaxMthNum, String updatedDt, String userId, String wwCmpMthNum, String wwMaxMthNum, String commentTxt, CancellationToken token = default(CancellationToken))
        {
            var pinNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@PIN_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = pinNum, Size = 10 };
            if (pinNumParam.Value == null)
            {
                pinNumParam.Value = System.DBNull.Value;
            }

            var benefitMmParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@BENEFIT_MM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = benefitMm, Size = 6 };
            if (benefitMmParam.Value == null)
            {
                benefitMmParam.Value = System.DBNull.Value;
            }

            var historySeqNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@HISTORY_SEQ_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = historySeqNum, Size = 4 };
            if (historySeqNumParam.Value == null)
            {
                historySeqNumParam.Value = System.DBNull.Value;
            }

            var clockTypeCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CLOCK_TYPE_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = clockTypeCd, Size = 4 };
            if (clockTypeCdParam.Value == null)
            {
                clockTypeCdParam.Value = System.DBNull.Value;
            }

            var creTranCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@CRE_TRAN_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = creTranCd, Size = 8 };
            if (creTranCdParam.Value == null)
            {
                creTranCdParam.Value = System.DBNull.Value;
            }

            var fedClockIndParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_CLOCK_IND", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedClockInd, Size = 1 };
            if (fedClockIndParam.Value == null)
            {
                fedClockIndParam.Value = System.DBNull.Value;
            }

            var fedCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedCmpMthNum, Size = 4 };
            if (fedCmpMthNumParam.Value == null)
            {
                fedCmpMthNumParam.Value = System.DBNull.Value;
            }

            var fedMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@FED_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = fedMaxMthNum, Size = 4 };
            if (fedMaxMthNumParam.Value == null)
            {
                fedMaxMthNumParam.Value = System.DBNull.Value;
            }

            var historyCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@HISTORY_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = historyCd, Size = 4 };
            if (historyCdParam.Value == null)
            {
                historyCdParam.Value = System.DBNull.Value;
            }

            var otCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@OT_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = otCmpMthNum, Size = 4 };
            if (otCmpMthNumParam.Value == null)
            {
                otCmpMthNumParam.Value = System.DBNull.Value;
            }

            var overrideReasonCdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@OVERRIDE_REASON_CD", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = overrideReasonCd, Size = 3 };
            if (overrideReasonCdParam.Value == null)
            {
                overrideReasonCdParam.Value = System.DBNull.Value;
            }

            var totCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@TOT_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = totCmpMthNum, Size = 4 };
            if (totCmpMthNumParam.Value == null)
            {
                totCmpMthNumParam.Value = System.DBNull.Value;
            }

            var totMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@TOT_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = totMaxMthNum, Size = 4 };
            if (totMaxMthNumParam.Value == null)
            {
                totMaxMthNumParam.Value = System.DBNull.Value;
            }

            var updatedDtParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@UPDATED_DT", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = updatedDt, Size = 10 };
            if (updatedDtParam.Value == null)
            {
                updatedDtParam.Value = System.DBNull.Value;
            }

            var userIdParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@USER_ID", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = userId, Size = 6 };
            if (userIdParam.Value == null)
            {
                userIdParam.Value = System.DBNull.Value;
            }

            var wwCmpMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@WW_CMP_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = wwCmpMthNum, Size = 4 };
            if (wwCmpMthNumParam.Value == null)
            {
                wwCmpMthNumParam.Value = System.DBNull.Value;
            }

            var wwMaxMthNumParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@WW_MAX_MTH_NUM", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = wwMaxMthNum, Size = 4 };
            if (wwMaxMthNumParam.Value == null)
            {
                wwMaxMthNumParam.Value = System.DBNull.Value;
            }

            var commentTxtParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@COMMENT_TXT", SqlDbType = System.Data.SqlDbType.VarChar, Direction = System.Data.ParameterDirection.Input, Value = commentTxt, Size = 75 };
            if (commentTxtParam.Value == null)
            {
                commentTxtParam.Value = System.DBNull.Value;
            }

            var procResultParam = new System.Data.SqlClient.SqlParameter { ParameterName = "@procResult", SqlDbType = System.Data.SqlDbType.Int, Direction = System.Data.ParameterDirection.Output };

            return this.Database.ExecuteSqlCommandAsync(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC [wwp].[UpdateDB2_T0459_IN_W2_LIMITS] @PIN_NUM, @BENEFIT_MM, @HISTORY_SEQ_NUM, @CLOCK_TYPE_CD, @CRE_TRAN_CD, @FED_CLOCK_IND, @FED_CMP_MTH_NUM, @FED_MAX_MTH_NUM, @HISTORY_CD, @OT_CMP_MTH_NUM, @OVERRIDE_REASON_CD, @TOT_CMP_MTH_NUM, @TOT_MAX_MTH_NUM, @UPDATED_DT, @USER_ID, @WW_CMP_MTH_NUM, @WW_MAX_MTH_NUM, @COMMENT_TXT", token, pinNumParam, benefitMmParam, historySeqNumParam, clockTypeCdParam, creTranCdParam, fedClockIndParam, fedCmpMthNumParam, fedMaxMthNumParam, historyCdParam, otCmpMthNumParam, overrideReasonCdParam, totCmpMthNumParam, totMaxMthNumParam, updatedDtParam, userIdParam, wwCmpMthNumParam, wwMaxMthNumParam, commentTxtParam, procResultParam);
        }

    }
}
