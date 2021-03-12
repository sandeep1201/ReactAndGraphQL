using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Common;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dcf.Wwp.Api.Library.Contracts.Content
{
    [DataContract]
    public class PageContract : BaseModelContract
    {
        [DataMember]
        public String Title { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public Int32 SortOrder { get; set; }

        [DataMember]
        public String Slug { get; set; }

        [DataMember]
        public List<ModuleContract> Modules { get; set; } = new List<ModuleContract>();

        public static PageContract Create(IContentPage contentPage)
        {
            var contract = new PageContract()
            {
                Title = contentPage.Title,
                Description = contentPage.Description,
                SortOrder = contentPage.SortOrder,
                Slug = contentPage.Slug
            };
            BaseModelContract.SetBaseProperties(contract,contentPage);
            contract.Modules.AddRange(contentPage.ContentModules.Select(ModuleContract.Create));
            return contract;
        }
    }

    [DataContract]
    public class ModuleContract : BaseModelContract
    {
        [DataMember]
        public String Name { get; set; }

        [DataMember]
        public String Title { get; set; }

        [DataMember]
        public Boolean ShowTitle { get; set; }

        [DataMember]
        public String Description { get; set; }

        [DataMember]
        public Boolean ShowDescription { get; set; }

        [DataMember]
        public ModuleStatus Status { get; set; }

        [DataMember]
        public Int32 SortOrder { get; set; }

        public Int32? ContentPageId { get; set; }

        [DataMember]
        public List<MetaContract> ModuleMetas { get; set; } = new List<MetaContract>();

        public static ModuleContract Create(IContentModule module)
        {
            var contract = new ModuleContract()
            {
                Name = module.Name,
                Title = module.Title,
                ShowTitle = module.ShowTitle,
                Description = module.Description,
                ShowDescription = module.ShowDescription,
                Status = (ModuleStatus) module.Status,
                SortOrder = module.SortOrder,
                ContentPageId = module.ContentPageId,
            };

            BaseModelContract.SetBaseProperties(contract, module);
            contract.ModuleMetas.AddRange(module.ContentModuleMetas.Select(MetaContract.Create));
            return contract;
        }
    }




    [DataContract]
    public class MetaContract : BaseModelContract
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public dynamic Data { get; set; }

        [DataMember]
        public Int32? ContentModuleId { get; set; }

        public static MetaContract Create(IContentModuleMeta moduleMeta)
        {
            var contract = new MetaContract()
            {
                Name = moduleMeta.Name,
            };

            BaseModelContract.SetBaseProperties(contract, moduleMeta);
            try
            {
                contract.Data = JObject.Parse(moduleMeta.Data);
            }
            catch (JsonReaderException e)
            {
                contract.Data = $"{{value: {moduleMeta.Data} }}";
            }
            catch (Exception e)
            {
                // TODO: Log this, but don't error out
                contract.Data = new ErrorInfo("Error deserializing Module Meta data", $"Id: {moduleMeta.Id}, ModuleId: {moduleMeta.ContentModuleId}, Content: {moduleMeta.Data}");
            }
            contract.ContentModuleId = moduleMeta.ContentModuleId;

            return contract;
        }
    }
}
