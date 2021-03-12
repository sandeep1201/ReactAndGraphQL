using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Dcf.Wwp.Api.Library.Extensions;


namespace Dcf.Wwp.Api.Library.Contracts
{
    public class ReferenceMultiExclusiveDataContract : ReferenceMultiDataContract
    {
        #region Properties

        [DataMember(Name = "disabledIds")]
        public int[] DisabledIds { get; private set; }

        #endregion

        #region Methods

        /// <remarks>
        /// Used for mutually exclusive options in multi drop downs (used forPopulation types)
        /// </remarks>
        public static IFieldData Create(int id, string name, bool DisablesOthersFlag, IEnumerable<int> disabledIds) 
        {
            return new ReferenceMultiExclusiveDataContract
                   {
                       Id             = id,
                       Name           = name.SafeTrim(),
                       DisablesOthers = DisablesOthersFlag,
                       DisabledIds    = disabledIds.ToArray()
                   };
        }

        #endregion
    }
}
