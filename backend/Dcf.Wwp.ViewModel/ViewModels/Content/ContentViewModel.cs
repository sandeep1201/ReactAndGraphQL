using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Dcf.Wwp.Api.Library.Contracts.Content;
using Dcf.Wwp.Data.Sql;
using Dcf.Wwp.Data.Sql.Model;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;
using Dcf.Wwp.Model.Services;
using DCF.Common.Exceptions;
using DCF.Common.Logging;
using Newtonsoft.Json;

namespace Dcf.Wwp.Api.Library.ViewModels.Content
{
    public class ContentViewModel : BaseViewModel
    {
        private readonly IContentService _contentService;
        private readonly JsonSerializerSettings _serializerSettings;

        public ContentViewModel(IContentService _contentService, JsonSerializerSettings serializerSettings, IRepository repository, IAuthUser authUser) : base(repository, authUser)
        {
            this._contentService = _contentService;
            this._serializerSettings = serializerSettings;
        }

        public PageContract GetPageBySlug(String pageSlug)
        {
            var contentPage = this._contentService.GetContentPage(pageSlug);
            if (contentPage == null)
            {
                // if it doesn't exist we will create one for this slug!

                contentPage = new ContentPage
                {
                    Slug = pageSlug,
                    ModifiedBy = this.AuthUser?.Username,
                    Title = "New Page",
                    ModifiedDate = DateTime.Now,
                    Description = ""
                };
                this._contentService.UpsertContentPage(contentPage);
                //throw new EntityNotFoundException(typeof(ContentPage), pageSlug);
            }

            var contract = PageContract.Create(contentPage);
            return contract;
        }

        public PageContract GetPageById(Int32 id)
        {
            var contentPage = this._contentService.GetContentPage(id);
            if (contentPage == null)
            {
                throw new EntityNotFoundException(typeof(ContentPage), id);
            }

            var contract = PageContract.Create(contentPage);
            return contract;
        }

        public ModuleContract GetModuleContractById(Int32 id)
        {
            var contentModule = this._contentService.GetContentModule(id);
            if (contentModule == null)
            {
                throw new EntityNotFoundException(typeof(ContentModule), id);
            }

            var contract = ModuleContract.Create(contentModule);
            return contract;
        }

        public MetaContract GetMetaContractById(Int32 id)
        {
            var contentModuleMeta = this._contentService.GetContentModuleMeta(id);
            if (contentModuleMeta == null)
            {
                throw new EntityNotFoundException(typeof(ContentModuleMeta), id);
            }

            var contract = MetaContract.Create(contentModuleMeta);
            return contract;
        }


        public UpsertResponse<PageContract> UpsertData(PageContract contract)
        {
            var response = new UpsertResponse<PageContract>();
            IContentPage model;

            if (contract.Id == default(Int32))
            {
                model = new ContentPage();
            }
            else
            {
                model = this._contentService.GetContentPage(contract.Id);
            }

            try
            {
                this._mapPageContractToContentPageModel(contract, model);

                this._contentService.UpsertContentPage(model);
                response.UpdatedModel = PageContract.Create(model);

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                this.Logger.ErrorException("Error performing PageContract UpsertData in ContentViewModel for {@pageId}, {@data}", ex, contract.Id, contract);
                throw;
            }

            return response;
        }

        public UpsertResponse<List<ModuleContract>> UpsertData(IEnumerable<ModuleContract> contracts)
        {
            var response = new UpsertResponse<List<ModuleContract>>();
            response.UpdatedModel = new List<ModuleContract>();
            var models = new List<IContentModule>();

            try
            {
                foreach (var contract in contracts)
                {
                    IContentModule model;

                    if (contract.Id == default(Int32))
                    {
                        model = new ContentModule();
                    }
                    else
                    {
                        model = this._contentService.GetContentModule(contract.Id);
                    }

                    this._mapModuleContractToContentModuleModel(contract, model);
                    models.Add(model);

                }
                models = new List<IContentModule>(this._contentService.UpsertContentModules(models));
                response.UpdatedModel.AddRange(models.Select(ModuleContract.Create));

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                this.Logger.ErrorException("Error performing ModuleContracts UpsertData in ContentViewModel {@data}", ex, contracts);
                throw;
            }

            return response;
        }

        public UpsertResponse<List<MetaContract>> UpsertData(IEnumerable<MetaContract> contracts)
        {
            var response = new UpsertResponse<List<MetaContract>>();
            response.UpdatedModel = new List<MetaContract>();
            var models = new List<IContentModuleMeta>();

            try
            {
                foreach (var contract in contracts)
                {
                    IContentModuleMeta model;

                    if (contract.Id == default(Int32))
                    {
                        model = new ContentModuleMeta();
                    }
                    else
                    {
                        model = this._contentService.GetContentModuleMeta(contract.Id);
                    }

                    this._mapMetaContractToContentModuleMetaModel(contract, model);
                    models.Add(model);

                }
                models = new List<IContentModuleMeta>(this._contentService.UpsertContentModuleMetas(models));
                response.UpdatedModel.AddRange(models.Select(MetaContract.Create));

            }
            catch (Exception ex)
            {
                response.ErrorMessage = ex.Message;
                this.Logger.ErrorException("Error performing MetaContract UpsertData in ContentViewModel {@data}", ex, contracts);
                throw;
            }

            return response;
        }


        #region Private Methods

        #region Mapping Methods

        private void _mapPageContractToContentPageModel(PageContract contract, IContentPage model)
        {
            MapBaseContractToBaseModel(contract, model);
            model.Title = contract.Title;
            model.Description = contract.Description;
            model.SortOrder = contract.SortOrder;
            model.Slug = contract.Slug;

            List<ContentModule> contentModules = new List<ContentModule>(model.ContentModules.Cast<ContentModule>());
            foreach (var moduleContract in contract.Modules)
            {
                ContentModule moduleModel = null;
                // Should Exist on IContentPage and should be tracked...
                if (moduleContract.Id > 0)
                {
                    moduleModel = (ContentModule)model.ContentModules.First(x => x.Id == moduleContract.Id);
                }
                else
                {
                    moduleModel = new ContentModule();
                    contentModules.Add(moduleModel);
                }

                this._mapModuleContractToContentModuleModel(moduleContract, moduleModel);
                
            }
            model.ContentModules = contentModules.Cast<IContentModule>().ToList();
        }

        private void _mapModuleContractToContentModuleModel(ModuleContract contract, IContentModule model)
        {
            MapBaseContractToBaseModel(contract, model);
            model.Name = contract.Name;
            model.Title = contract.Title;
            model.ShowTitle = contract.ShowTitle;
            model.Description = contract.Description;
            model.ShowDescription = contract.ShowDescription;
            model.Status = contract.Status;
            model.SortOrder = contract.SortOrder;
            model.ContentPageId = contract.ContentPageId == 0 ? null : contract.ContentPageId;

            var contentModuleMetas = model.ContentModuleMetas.ToList();
            foreach (var metaContract in contract.ModuleMetas)
            {
                ContentModuleMeta metaModel = null;

                // Should Exist on IContentPage and should be tracked...
                if (metaContract.Id > 0)
                {
                    metaModel = (ContentModuleMeta)model.ContentModuleMetas.First(x => x.Id == contract.Id);
                }
                else
                {
                    metaModel = new ContentModuleMeta();
                    contentModuleMetas.Add(metaModel);

                }

                this._mapMetaContractToContentModuleMetaModel(metaContract, metaModel);
            }

            model.ContentModuleMetas = contentModuleMetas;

        }

        private void _mapMetaContractToContentModuleMetaModel(MetaContract contract, IContentModuleMeta model)
        {
            MapBaseContractToBaseModel(contract, model);
            model.Name = contract.Name;
            model.ContentModuleId = contract.ContentModuleId == 0 ? (Int32?)null : contract.ContentModuleId;
            model.Data = Newtonsoft.Json.JsonConvert.SerializeObject(contract.Data, _serializerSettings);

        }
        #endregion

        #endregion
    }
}
