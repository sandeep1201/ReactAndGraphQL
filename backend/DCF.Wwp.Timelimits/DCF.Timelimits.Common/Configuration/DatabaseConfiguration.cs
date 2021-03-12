using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace DCF.Common.Configuration
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public String Server { get; set; } = Environment.GetEnvironmentVariable("WWP_DB_SERVER");
        public String Catalog { get; set; } = Environment.GetEnvironmentVariable("WWP_DB_NAME");
        public String UserId { get; set; } = Environment.GetEnvironmentVariable("WWP_DB_USER");
        public String Password { get; set; } = Environment.GetEnvironmentVariable("WWP_DB_PASS");
        public Int32 MaxPoolSize { get; set; } = 1000;
        public Int32 Timeout { get; set; } = 600;
        public Func<IDbExecutionStrategy> ExecutionStrategyFactory { get; set; } = () => new DcfDbExecutionStrategy(5,TimeSpan.FromSeconds(2));

        public String GetConnectionString()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = this.Server;
            builder.InitialCatalog = this.Catalog;
            builder.UserID = this.UserId;
            builder.Password = this.Password;
            builder.Pooling = true;

            return builder.ConnectionString;
        }
    }

    public class DcfPollyDbExecutionStrategy : IDbExecutionStrategy
    {
        private ISyncPolicy _policy;
        private IAsyncPolicy _asyncPolicy;

        public DcfPollyDbExecutionStrategy(ISyncPolicy policy, IAsyncPolicy asyncPolicy)
        {
            this._policy = policy;
            this._asyncPolicy = asyncPolicy;
        }

        public void Execute(Action operation)
        {
            this._policy.Execute(operation);
        }

        public TResult Execute<TResult>(Func<TResult> operation)
        {
            return this._policy.Execute(operation);
        }

        public Task ExecuteAsync(Func<Task> operation, CancellationToken cancellationToken)
        {
            Func<CancellationToken, Task> wrappedOperation = (c) =>
            {
                if (c.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                return operation();
            };
            return this._asyncPolicy.ExecuteAsync(wrappedOperation, cancellationToken);
        }

        public Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> operation, CancellationToken cancellationToken)
        {
            Func<CancellationToken, Task<TResult>> wrappedOperation = (c) =>
            {
                if (c.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }
                return operation();
            };
            return this._asyncPolicy.ExecuteAsync(wrappedOperation, cancellationToken);
        }

        public Boolean RetriesOnFailure { get; } = true;
    }



    //internal class DcfDbExecutionStrategy : DbExecutionStrategy
    public class DcfDbExecutionStrategy : DbExecutionStrategy
    {

        public DcfDbExecutionStrategy()
        {
            
        }
        public DcfDbExecutionStrategy(int maxRetryCount, TimeSpan maxDelay) : base(maxRetryCount, maxDelay)
        {
        }

        protected override Boolean ShouldRetryOn(Exception exception)
        {
            return exception is SqlException || exception is DataException;
        }
    }


}