using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Extensions;


namespace Dcf.Wwp.Api.Library.Contracts
{
    /// <summary>
    /// This is a marker interface to assist with multiple types being used with the Field
    /// Data controller, specifically the GetData method call.
    /// </summary>
    public interface IFieldData
    {
    }

    [DataContract]
    public class ReferenceDataContract : IFieldData
    {
        // This forces us to use the Create factory method.
        protected internal ReferenceDataContract()
        {
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "disablesOthers")]
        public bool? RequireDetailsFlag { get; set; }


        /// <summary>
        /// Return a ReferenceDataContract but as the generic interface IFieldData so it
        /// can be returned to the FieldData.GetData method in a way that is needed for the
        /// controller to return different types (i.e. WageType...)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IFieldData Create(int id, string name)
        {
            return new ReferenceDataContract { Id = id, Name = name.SafeTrim() };
        }

        public static ReferenceDataContract CreateContract(int id, string name, bool? requireDetailsFlag)
        {
            return new ReferenceDataContract { Id = id, Name = name.SafeTrim(), RequireDetailsFlag = requireDetailsFlag };
        }
    }


    [DataContract]
    public class NestedReferenceDataContract : IFieldData
    {
        // This forces us to use the Create factory method.
        protected internal NestedReferenceDataContract()
        {
        }

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "subTypes")]
        public List<ReferenceDataContract> SubTypes { get; set; }

        public static NestedReferenceDataContract CreateContract(int id, string name)
        {
            return new NestedReferenceDataContract { Id = id, Name = name.SafeTrim(), SubTypes = new List<ReferenceDataContract>() };
        }
    }
}
