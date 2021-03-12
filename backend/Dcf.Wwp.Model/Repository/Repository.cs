using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using EnumsNET;
using DCF.Common.Configuration;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using DCF.Common;
using DCF.Common.Logging;
using DCF.Common.Exceptions;

namespace Dcf.Wwp.Model.Repository
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class Repository : IRepository
    {
        #region Properties

        private          ILog                   _logger = LogProvider.GetLogger(typeof(Repository));
        private readonly IDatabaseConfiguration _dbConfig;
        private readonly IAuthUser              _authUser;
        private          Database               _sqlDb { get; }

        public string Server   => _dbConfig?.Server;
        public string Database => _dbConfig?.Catalog;
        public string UserId   => _dbConfig?.UserId;
        public string Pass     => _dbConfig?.Password;

        public CancellationToken Token { get; } = CancellationToken.None;

        #endregion

        #region Methods

        // Constructor is used by TimeLimits
        public Repository(IDatabaseConfiguration config)
        {
            this._dbConfig = config;
            this._db       = new WwpEntities(config);
        }

        /// Used only for UNIT Testing 
        // Constructor is used by TimeLimits
        internal Repository(WwpEntities context)
        {
            _dbConfig = new DatabaseConfiguration();
            _db       = context;
            _sqlDb    = _db.Database;
        }

        public Repository(IDatabaseConfiguration config, IAuthUser authUser)
        {
            _dbConfig = config;
            _authUser = authUser;
            _db       = new WwpEntities(config);
            _sqlDb    = _db.Database;
        }

        /// <summary>
        /// Used only for UNIT Testing 
        /// </summary>
        internal Repository(WwpEntities context, IAuthUser authUser)
        {
            _dbConfig = new DatabaseConfiguration();
            _db       = context;
            _authUser = authUser;
            _sqlDb    = _db.Database;
        }

        public bool Attach<T>(T model) where T : class
        {
            if (_db.Entry(model).State == EntityState.Detached)
            {
                _db.Set(model.GetType()).Attach(model);
                return true;
            }

            return false;
        }

        public bool Dettach<T>(T model) where T : class
        {
            if (!_db.Entry(model).State.HasFlag(EntityState.Detached))
            {
                _db.Entry(model).State = EntityState.Detached;
                return true;
            }

            return false;
        }

        public DbEntityEntry<T> GetEntityEntry<T>(T model) where T : class
        {
            return _db.Entry(model);
        }

        /// <summary>
        /// Makes a clone copy of the model object so it can later be checked
        /// for changes.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public void StartChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel
        {
            // We might get called with a null model.  If so, just return.
            if (model == null) return;

            // Calling ObjectContext.GetObjectType helps when DynamicProxies are being used.
            // https://msdn.microsoft.com/en-us/data/jj592886.aspx
            string key = $"{ObjectContext.GetObjectType(model.GetType())}-{model.Id}";

            if (_originalValues.ContainsKey(key))
            {
                throw new InvalidOperationException("Cannot call StartChangeTracking for an object with the same type and Id.");
            }

            _originalValues.Add(key, model.Clone());
        }

        public void ResetChangeTracking<T>(T model) where T : class, ICloneable, ICommonModel
        {
            // We might get called with a null model.  If so, just return.
            if (model == null) return;

            // Calling ObjectContext.GetObjectType helps when DynamicProxies are being used.
            // https://msdn.microsoft.com/en-us/data/jj592886.aspx
            string key = $"{ObjectContext.GetObjectType(model.GetType())}-{model.Id}";

            if (_originalValues.ContainsKey(key))
            {
                _originalValues.Remove(key);
            }
        }

        //public T GetClone<T>(T model) where T : class, ICloneable, ICommonModel
        //{
        //    // We might get called with a null model.  If so, just return.
        //    if (model == null) return null;

        //    // Calling ObjectContext.GetObjectType helps when DynamicProxies are being used.
        //    // https://msdn.microsoft.com/en-us/data/jj592886.aspx
        //    string key = $"{ObjectContext.GetObjectType(model.GetType())}-{model.Id}";

        //    if (_originalValues.ContainsKey(key))
        //    {
        //        _originalValues.Remove(key);
        //    }

        //}

        /// <summary>
        /// Indicates if a model object has changed since StartChangeTracking was called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool HasChanged<T>(T model) where T : class, ICloneable, ICommonModel //, IDataEquality
        {
            var originalDatabaseObject = GetClonedOriginal(model);

            // New/null objects are always reported as changed.
            if (originalDatabaseObject == null)
            {
                return true;
            }

            return !model.Equals(originalDatabaseObject);
        }

        public T GetClonedOriginal<T>(T model) where T : class, ICloneable, ICommonModel
        {
            // New objects aren't cloned, so return null;
            if (model.Id == default(int))
                return null;

            // Calling ObjectContext.GetObjectType helps when DynamicProxies are being used.
            // https://msdn.microsoft.com/en-us/data/jj592886.aspx
            string key = $"{ObjectContext.GetObjectType(model.GetType())}-{model.Id}";

            if (!_originalValues.ContainsKey(key))
                throw new InvalidOperationException("Change tracking for the object has not been started.");

            return (T) _originalValues[key];
        }

        /// <summary>
        /// Saves if the model has changed and returns an indicator of if it has saved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool SaveIfChanged<T>(T model, string user) where T : class, ICloneable, ICommonModel
        {
            // If the model is null, there's nothing to save.
            if (model == null)
                return false;

            if (!HasChanged(model))
                return false;

            // It has changed, so update the last modified info and save it.
            model.ModifiedBy   = user;
            model.ModifiedDate = DateTime.Now;

            // If the model is complex, then set the modified on the complex properties.
            var complex = model as IComplexModel;

            if (complex != null)
            {
                var cloned = GetClonedOriginal(model);
                complex.SetModifiedOnComplexProperties(cloned, user, model.ModifiedDate.Value);
            }

            Save();

            return true;
        }

        /// <summary>
        /// Saves if the model has changed and returns an indicator of if it has saved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <param name="modifiedDate"></param>
        /// <returns></returns>
        public bool SaveIfChanged<T>(T model, string user, DateTime modifiedDate) where T : class, ICloneable, ICommonModel
        {
            // If the model is null, there's nothing to save.
            if (model == null)
                return false;

            if (!HasChanged(model))
                return false;

            // It has changed, so update the last modified info and save it.
            model.ModifiedBy   = user;
            model.ModifiedDate = modifiedDate;

            // If the model is complex, then set the modified on the complex properties.
            var complex = model as IComplexModel;

            if (complex != null)
            {
                var cloned = GetClonedOriginal(model);
                complex.SetModifiedOnComplexProperties(cloned, user, model.ModifiedDate.Value);
            }

            Save();

            return true;
        }

        public void SaveRangeIfChanged<T>(IEnumerable<T> models, string user) where T : class
        {
            if (models != null)
            {
                foreach (var model in models)
                {
                    var dbEntity = _db.Entry(model);

                    if (dbEntity != null)
                    {
                        var hasChanged = false;

                        if (dbEntity.State.HasAnyFlags(EntityState.Detached))
                        {
                            _db.Set<T>().Attach(model);
                        }

                        if (dbEntity.State.HasAnyFlags(EntityState.Modified))
                        {
                            foreach (var propertyName in dbEntity.OriginalValues.PropertyNames)
                            {
                                if (dbEntity.CurrentValues[propertyName] != dbEntity.OriginalValues[propertyName])
                                {
                                    hasChanged = true;
                                    break;
                                }
                            }
                        }

                        // detach from context if not changed
                        if (!hasChanged)
                        {
                            dbEntity.State = EntityState.Detached;
                        }
                    }
                }
            }

            Save();
        }

        /// <summary>
        /// Checks the model object which must be obtained from the Repository against
        /// the user's row version.  It also applies this check only when the model
        /// object has changed.  Therefore, it is safe to call this method not knowing
        /// if the user has changed anything (this handles the Save & Continue call from
        /// the web page when there aren't any changes).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="usersRowVersion"></param>
        /// <returns></returns>
        public bool IsRowVersionStillCurrent<T>(T model, byte[] usersRowVersion)
            where T : class, ICloneable, ICommonModel
        {
            // If the model is null, there is no concurrency issue.
            if (model == null)
                return true;

            // If the model object is new, there is no concurrency issue.
            if (model.Id == default(int))
                return true;

            if (HasChanged(model))
            {
                // Since the model object has changed and needs to be saved, we
                // need to verify the row version in the database (which is the model
                // variable) hasn't changed from what the user originally had.
                // NOTE: you can't just use != for arrays.

                if (model.RowVersion != null && usersRowVersion == null)
                {
                    return true; // let DB handle the row version check and concurrency
                }

                if (model.RowVersion?.SequenceEqual(usersRowVersion) == false)
                    return false;
            }

            // If we get to here, there either wasn't a change or the row
            // versions match... so it's still current.
            return true;
        }

        public void ResetContext()
        {
            if (string.IsNullOrEmpty(Server) || string.IsNullOrEmpty(Database))
                throw new InvalidOperationException(
                                                    "Cannot reset the context when the connection properties have not been set.");

            _db.Dispose();

            _db = new WwpEntities(_dbConfig);
        }

        public void Save(bool refreshOnConcurrencyException = false)
        {
            try
            {
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.ForEach(entry =>
                                   {
                                       if (refreshOnConcurrencyException)
                                           entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                                   });

                Save();
            }
            catch (DBConcurrencyException ex)
            {
                this._logger.ErrorException("Concurreny Error Saving changes. ", ex);
                throw;
            }
            catch (DbEntityValidationException dbEx)
            {
                var l = new List<DbValidationError>();

                var efErrors = dbEx.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors);

                foreach (var validationError in efErrors)
                {
                    l.Add(validationError);
                    Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                }

                throw;
            }
        }

        public Task<Int32> SaveAsync(CancellationToken token = default(CancellationToken))
        {
            var changed = _db.ChangeTracker.Entries().Where(t => t.State == EntityState.Modified).ToList();
            var added   = _db.ChangeTracker.Entries().Where(t => t.State == EntityState.Added).ToList();
            return this._db.SaveChangesAsync(token);
        }

        public DateTime AddBusinessDays(DateTime? startDate, int? noDays = 10)
        {
            var d = _authUser.CDODate ?? DateTime.Today;

            var r = (DateTime) _db.USP_GetComputedBusniessDays(startDate, noDays).FirstOrDefault();

            return (r);
        }

        public ISP_GetCARESCaseNumber_Result GetCARESCaseNumber(string pin)
        {
            return _db.SP_GetCARESCaseNumber(pin, Database).FirstOrDefault();
        }

        #endregion	IRepository Methods

        #region Private Fields

        private WwpEntities _db;

        //private readonly ILogger<Repository> _logger;

        private readonly Dictionary<string, object> _originalValues = new Dictionary<string, object>();

        #endregion	Private Fields

        #region IDisposable Support

        private bool _disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _db?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion	IDisposable Support

        #region demonRegion

        //public IQueryable<T> GetAsQueryable<T>() where T : class
        //{
        //    var dbSet = _db.Set<T>();
        //    var q     = dbSet.AsQueryable();

        //    return (q);
        //}

        //public IQueryable<T> GetAsQueryableAsNoTracking<T>() where T : class
        //{
        //    var dbSet = _db.Set<T>();
        //    var q     = dbSet.AsNoTracking();

        //    return (q);
        //}

        #endregion
    }
}
