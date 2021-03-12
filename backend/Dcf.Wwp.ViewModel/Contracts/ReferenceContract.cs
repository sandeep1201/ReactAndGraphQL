using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Extensions;


namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class ReferenceContract : IFieldData
    {
        // This forces us to use the Create factory method.
        protected internal ReferenceContract()
        {
        }

        [DataMember(Name = "id")]
        public int Id { get; protected set; }

        [DataMember(Name = "name")]
        public string Name { get; protected set; }

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
            return new ReferenceContract { Id = id, Name = name.SafeTrim() };
        }

        public static ReferenceContract CreateContract(int id, string name)
        {
            return new ReferenceContract { Id = id, Name = name.SafeTrim() };
        }

    }
}
