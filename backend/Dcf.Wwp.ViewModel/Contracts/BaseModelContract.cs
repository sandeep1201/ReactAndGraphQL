using System;
using System.Runtime.Serialization;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public abstract class BaseModelContract
    {
        [DataMember(Name = "id")]
        public virtual Int32 Id { get; set; }

        [DataMember(Name = "createdDate")]
        public virtual DateTimeOffset? CreatedDate { get; set; }

        [DataMember(Name = "modifiedBy")]
        public virtual String ModifiedBy { get; set; }

        [DataMember(Name = "modifiedDate")]
        public virtual DateTimeOffset? ModifiedDate { get; set; }

        [DataMember(Name = "isDeleted")]
        public virtual Boolean IsDeleted { get; set; }

        [DataMember(Name = "rowVersion")]
        public virtual byte[] RowVersion { get; set; }

        public static void SetBaseProperties(BaseModelContract instance, ICommonDelModel model)
        {
            instance.Id = model.Id;
            instance.ModifiedBy = model.ModifiedBy;
            instance.ModifiedDate = model.ModifiedDate;
            instance.IsDeleted = model.IsDeleted;
            instance.RowVersion = model.RowVersion;
        }

        public static void SetBaseProperties(BaseModelContract instance, ICommonDelCreatedModel model)
        {
            BaseModelContract.SetBaseProperties(instance, (ICommonDelModel)model);
            instance.CreatedDate = model.CreatedDate;
 
        }
    }
}