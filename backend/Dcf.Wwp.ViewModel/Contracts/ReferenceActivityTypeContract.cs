using System.Runtime.Serialization;

namespace Dcf.Wwp.Api.Library.Contracts
{
    [DataContract]
    public class ReferenceActivityTypeContract : IFieldData
    {
        #region Properties

        [DataMember(Name = "id")]
        public int? Id { get; protected set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "code")]
        public string Code { get; set; }

        //[DataMember(Name = "enrolledProgamId")]
        //public int? EnrolledProgramId { get; protected set; }

        //[DataMember(Name = "activityTypeId")]
        //public int? ActivityTypeId { get; protected set; }

        [DataMember(Name = "canSelfDirect")]
        public bool? CanSelfDirect { get; protected set; }

        #endregion

        #region Methods

        public static ReferenceActivityTypeContract Create(int? id, string code, string name, bool? isSelfDirected)
        {
            var r = new ReferenceActivityTypeContract
                    {
                        Id            = id,
                        Name          = name,
                        CanSelfDirect = isSelfDirected,
                        Code  = code
                    };

            return (r);
        }

        #endregion
    }
}
