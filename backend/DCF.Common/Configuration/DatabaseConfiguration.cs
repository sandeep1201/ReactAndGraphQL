using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace DCF.Common.Configuration
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        #region Properties

        public        string Server           { get; set; }
        public        string Catalog          { get; set; }
        public        string UserId           { get; set; }
        public        string Password         { get; set; }
        public        int    MaxPoolSize      { get; set; }
        public        int    Timeout          { get; set; }
        public static string ConnectionString { get; set; }

        #endregion

        #region Methods

        public DatabaseConfiguration ()
        {
#if (USE_ENV_VARS2)
            var envSet = "2";       // for WWPTRN setup
#else
            var envSet = string.Empty;
#endif
            Server      = Environment.GetEnvironmentVariable("WWP_DB_SERVER" + envSet);
            Catalog     = Environment.GetEnvironmentVariable("WWP_DB_NAME"   + envSet);
            UserId      = Environment.GetEnvironmentVariable("WWP_DB_USER"   + envSet);
            Password    = Environment.GetEnvironmentVariable("WWP_DB_PASS"   + envSet);
            MaxPoolSize = 1000;
            Timeout     = 600;

            var csb = new SqlConnectionStringBuilder
                      {
                          DataSource               = Server,
                          InitialCatalog           = Catalog,
                          UserID                   = UserId,
                          Password                 = Password,
                          MultipleActiveResultSets = true
                      };

            ConnectionString = csb.ConnectionString;
        }

        #endregion
    }

    public class DcfPollyDbExecutionStrategy : IDbExecutionStrategy
    {
        private readonly ISyncPolicy  _policy;
        private readonly IAsyncPolicy _asyncPolicy;

        public DcfPollyDbExecutionStrategy(ISyncPolicy policy, IAsyncPolicy asyncPolicy)
        {
            _policy      = policy;
            _asyncPolicy = asyncPolicy;
        }

        public void Execute(Action operation)
        {
            _policy.Execute(operation);
        }

        public TResult Execute<TResult>(Func<TResult> operation) => _policy.Execute(operation);

        public Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
        {
            Func<CancellationToken, Task> wrappedOperation = c =>
                                                             {
                                                                 if (c.IsCancellationRequested)
                                                                 {
                                                                     throw new OperationCanceledException();
                                                                 }

                                                                 return operation();
                                                             };

            return _asyncPolicy.ExecuteAsync(wrappedOperation, cancellationToken);
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            Func<CancellationToken, Task<TResult>> wrappedOperation = c =>
                                                                      {
                                                                          if (c.IsCancellationRequested)
                                                                          {
                                                                              throw new OperationCanceledException();
                                                                          }

                                                                          return operation();
                                                                      };

            return _asyncPolicy.ExecuteAsync(wrappedOperation, cancellationToken);
        }

        public bool RetriesOnFailure { get; } = true;
    }

    public class DcfDbExecutionStrategy : DbExecutionStrategy
    {
        public DcfDbExecutionStrategy(int maxRetryCount, TimeSpan maxDelay) : base(maxRetryCount, maxDelay)
        {
        }

        protected override bool ShouldRetryOn(Exception exception) => exception is SqlException || exception is DataException;
    }
}
