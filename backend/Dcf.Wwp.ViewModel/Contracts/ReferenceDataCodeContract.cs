using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Extensions;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class ReferenceDataCodeContract : IFieldData
    {
        #region Properties

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "optionCd")]
        public string OptionCd { get; set; }

        [DataMember(Name = "optionId")]
        public int OptionId { get; set; }

        [DataMember(Name = "isIncome")]
        public bool IsIncome { get; set; }

        [DataMember(Name = "isAsset")]
        public bool IsAsset { get; set; }

        [DataMember(Name = "isVehicle")]
        public bool IsVehicle { get; set; }

        [DataMember(Name = "isVehicleValue")]
        public bool IsVehicleValue { get; set; }

        [DataMember(Name = "isSystemUseOnly")]
        public bool IsSystemUseOnly { get; set; }

        #endregion

        #region Methods

        // scott v. Don't think we really need a ctor or to make it static for that matter...
        public static IFieldData Create(int id, string name, string code, string optionCd = null)
        {
            return (new ReferenceDataCodeContract { Id = id, Name = name.SafeTrim(), Code = code, OptionCd = optionCd });
        }

        public static IFieldData Create(int id, string name, string code, bool isSystemUseOnly)
        {
            return (new ReferenceDataCodeContract { Id = id, Name = name.SafeTrim(), Code = code, IsSystemUseOnly = isSystemUseOnly});
        }

        public static IFieldData Create(int id, string name, int optionId)
        {
            return (new ReferenceDataCodeContract { Id = id, Name = name.SafeTrim(), OptionId = optionId });
        }

        public static IFieldData Create(int id, string name, string code, bool isIncome, bool isAsset, bool isVehicle, bool isVehicleValue)
        {
            return (new ReferenceDataCodeContract { Id = id, Name = name.SafeTrim(), Code = code, IsIncome = isIncome, IsAsset = isAsset, IsVehicle = isVehicle, IsVehicleValue = isVehicleValue });
        }


        #endregion
    }
}
