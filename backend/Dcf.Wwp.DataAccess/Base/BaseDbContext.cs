using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface.Model;

namespace Dcf.Wwp.DataAccess.Base
{
    public class BaseDbContext : DbContext, IDbContext
    {
        #region Properties

        protected readonly ILog   _log = LogManager.GetLogger(typeof(BaseDbContext));
        private   readonly string _cs;

        #endregion

        #region Methods

        public BaseDbContext(string connString) : base(connString)
        {
            _cs = connString;
            Database.SetInitializer<BaseDbContext>(null); // using an existing database, no initialization/migrations req'd
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            _log.Debug("BaseDbContext.OnModelCreating()");

            // Fluent API Entity configurations
            try
            {
                modelBuilder.HasDefaultSchema("wwp");
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
                modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }

            base.OnModelCreating(modelBuilder);
        }

        public int Commit()
        {
            var x = 0;

            try
            {
                x = SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    _log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }

            return (x);
        }

        public CommitStatus CommitWithStatus()
        {
            var commitStatus = new CommitStatus();

            try
            {
                SaveChanges();
                commitStatus.Status = "SUCCESS";
            }
            catch (Exception e)
            {
                commitStatus.Status    = "ERROR";
                commitStatus.Exception = e;
            }

            return commitStatus;
        }

        public async Task<int> CommitAsync()
        {
            var x = 0;

            try
            {
                x = await SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationError in dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors))
                {
                    _log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }
            }

            return (x);
        }

        public void Rollback()
        {
            ChangeTracker.Entries().ForEach(entry =>
                                            {
                                                if (entry.Entity.GetType().BaseType?.Name == nameof(ParticipantPlacement))
                                                {
                                                    switch (entry.State)
                                                    {
                                                        case EntityState.Modified:
                                                            entry.State = EntityState.Unchanged;
                                                            break;
                                                        case EntityState.Added:
                                                            entry.State = EntityState.Detached;
                                                            break;
                                                        case EntityState.Deleted:
                                                            entry.State = EntityState.Unchanged;
                                                            break;
                                                    }
                                                }
                                            });
        }

        public void SetEntityModified<T>(int id) where T : class
        {
            ChangeTracker.Entries<T>().ForEach(i =>
                                               {
                                                   if (i.OriginalValues.GetValue<int>("Id") == id)
                                                       i.OriginalValues.SetValues(i.GetDatabaseValues());
                                                   else if (i.OriginalValues.GetValue<int>("Id") > id)
                                                       i.State = EntityState.Detached;
                                               });
        }

        private T CreateWithValues<T>(DbPropertyValues values) where T : new()
        {
            var entity = new T();
            var type   = typeof(T);

            values.PropertyNames.ForEach(name =>
                                         {
                                             var property = type.GetProperty(name);
                                             property?.SetValue(entity, values.GetValue<object>(name));
                                         });

            return entity;
        }

        public virtual DataTable ExecStoredProcUsingAdo(string storedProcName, Dictionary<string, object> parms = null)
        {
            var dt = new DataTable();

            try
            {
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
                            _log.DebugFormat("Parm: {0} = {1}", kvp.Key, kvp.Value);

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
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message, ex);
                throw;
            }

            return (dt);
        }

        public virtual IList<T> ExecStoredProc<T>(string storedProcName)
        {
            IList<T> spResults;

            var sql = new StringBuilder("EXEC " + storedProcName + " ");

            try
            {
                spResults = Database.SqlQuery<T>(sql.ToString().Trim().TrimEnd(',')).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }

            return (spResults);
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

            try
            {
                spResults = Database.SqlQuery<T>(sql.ToString().Trim().TrimEnd(','), sqlParams).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }

            return (spResults);
        }

        public virtual int GetStoredProcReturnValue(string storedProcName, Dictionary<string, object> parms)
        {
            var idx       = 0;
            var sqlParams = new object[parms.Count];
            var sql       = new StringBuilder("EXEC " + storedProcName + " ");
            int returnValue;

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

            try
            {
                returnValue = Database.SqlQuery<int>(sql.ToString().Trim().TrimEnd(','), sqlParams).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }

            return (returnValue);
        }

        public virtual IList<T> ExecFunction<T>(string functionName, Dictionary<string, object> parms, bool isTableValuedFunction)
        {
            List<T> spResults;

            var idx       = 0;
            var sqlParams = new object[parms.Count];
            var sql       = new StringBuilder($"SELECT{(isTableValuedFunction ? " * FROM" : "")} {functionName} (");

            foreach (var param in parms)
            {
                sql.Append("@" + param.Key + ", ");

                var p = new SqlParameter
                        {
                            ParameterName = "@" + param.Key,
                            Direction     = ParameterDirection.Input,
                            Value         = param.Value
                        };

                sqlParams[idx] = p;
                idx++;
            }

            try
            {
                spResults = Database.SqlQuery<T>($"{sql.ToString().Trim().TrimEnd(',')})", sqlParams).ToList();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                throw;
            }

            return (spResults);
        }

        #endregion
    }
}
