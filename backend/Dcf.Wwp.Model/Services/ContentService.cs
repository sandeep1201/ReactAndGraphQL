using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using DCF.Common.Exceptions;
using DCF.Core;
using Z.EntityFramework.Plus;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace Dcf.Wwp.Model.Services
{
    public class ContentService : IContentService
    {
        private readonly WwpEntities _db;
        private readonly IAuthUser _authUser;

        public ContentService(WwpEntities db, IAuthUser authUser)
        {
            this._db = db;
            this._authUser = authUser;
        }

        #region Queries
        public IContentPage GetContentPage(String pageSlug)
        {
            var slug = pageSlug.ToLower().Trim();
                return this._doGetContentPage(x => x.Slug == slug).FirstOrDefault();
        }

        public IContentPage GetContentPage(Int32 Id)
        {
                return this._doGetContentPage(x => x.Id == Id).FirstOrDefault();
        }

        public IContentModule GetContentModule(Int32 id)
        {
                return _db.ContentModules.FirstOrDefault(x => x.Id == id);
        }

        public IContentModuleMeta GetContentModuleMeta(Int32 id)
        {
                return _db.ContentModuleMetas.FirstOrDefault(x => x.Id == id);
        }

        #endregion

        #region Commands

        public IContentPage UpsertContentPage(IContentPage model)
        {

                try
                {
                    WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = true;
                    using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = IsolationLevel.ReadUncommitted }))
                    {
                        // clean Id's so they can attach correctly 
                        foreach (var module in model.ContentModules)
                        {
                            module.ContentPageId = model.Id;
                            foreach (var meta in module.ContentModuleMetas)
                            {
                                meta.ContentModuleId = module.Id;
                            }
                        }

                        // Attach Graph
                        foreach (var module in model.ContentModules)
                        {
                            this._attachModelToContext( module);
                            foreach (var meta in module.ContentModuleMetas)
                            {
                                this._attachModelToContext( meta);
                            }
                        }

                        
                        
                        //// detach all properties so we can save
                        //foreach (var module in model.ContentModules)
                        //{
                        //    db.Entry(module).State = EntityState.Detached;
                        //    foreach (var meta in module.ContentModuleMetas)
                        //    {
                        //        db.Entry(meta).State = EntityState.Detached;
                        //    }
                        //}

                        model = this._attachModelToContext(model);

                        var entities = _db.ChangeTracker.Entries<ContentPage>().ToList();

                        _db.SaveChanges();
                        scope.Complete();

                    }
                }
                finally
                {
                    WwpEnttitesTransientFaultDbConfiguration.SuspendExecutionStrategy = false;
                }
            return model;
        }



        public IEnumerable<IContentModule> UpsertContentModules(IEnumerable<IContentModule> models)
        {

                var moduleProxies = models.Select(model => this._attachModelToContext(model));

                _db.SaveChanges();
                return moduleProxies;
        }

        public IEnumerable<IContentModuleMeta> UpsertContentModuleMetas(IEnumerable<IContentModuleMeta> models)
        {
                var moduleMetaProxies = models.Select(model => this._attachModelToContext( model));

                _db.SaveChanges();
                return moduleMetaProxies;
        }

        #endregion

        #region Private Methods

        private T _attachModelToContext<T>(T model) where T : class, IHasId
        {
            try
            {
                var entry = _db.Entry<T>(model);

                entry.State = EntityState.Added;

                if (model.Id != default(Int32))
                {
                    entry.State = EntityState.Modified;
                }
                return entry.Entity;
            }
            catch (Exception e)
            {
                throw new DCFApplicationException($"Failed to attach entity of type: {typeof(T)} with Id : {model?.Id}.", e);
            }
        }

        private IQueryable<ContentPage> _doGetContentPage(Expression<Func<ContentPage, Boolean>> filter, Boolean includedDeleted = false)
        {
            Debug.Assert(filter != null, nameof(filter) + " != null");
            IQueryable<ContentPage> query = this._db.ContentPages;

            if (includedDeleted)
            {
                query = query.Include(x => x.ContentModules).Include(x => x.ContentModules.SelectMany(y => y.ContentModuleMetas));
            }
            else
            {
                query = query.IncludeFilter(x => x.ContentModules.Where(y => !y.IsDeleted)).IncludeFilter(x => x.ContentModules.SelectMany(y => y.ContentModuleMetas).Where(z => !z.IsDeleted));
            }

            query = query.Where(filter);
            return query;
        }
        #endregion

        #region IDisposable

        /// <inheritdoc />
        public void Dispose()
        {
            this._db?.Dispose();
        }

        #endregion
    }
}
