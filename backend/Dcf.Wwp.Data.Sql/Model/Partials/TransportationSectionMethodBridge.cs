using System;
using Dcf.Wwp.Model.Interface;


namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class TransportationSectionMethodBridge : BaseCommonModel, ITransportationSectionMethodBridge, IEquatable<TransportationSectionMethodBridge>
    {
        ITransportationType ITransportationSectionMethodBridge.TransportationType
        {
            get => TransportationType;
            set => TransportationType = (TransportationType) value;
        }

        ITransportationSection ITransportationSectionMethodBridge.TransportationSection
        {
            get => TransportationSection;
            set => TransportationSection = (TransportationSection) value;
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new TransportationSectionMethodBridge()
                        {
                            TransportationSectionId = TransportationSectionId,
                            TransporationTypeId     = TransporationTypeId
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
                return false;

            var obj = other as TransportationSectionMethodBridge;
            return obj != null && Equals(obj);
        }

        public bool Equals(TransportationSectionMethodBridge other)
        {
            // Check whether the compared object is null.
            if (ReferenceEquals(other, null)) return false;

            // Check whether the compared object references the same data.
            if (ReferenceEquals(this, other)) return true;

            // Check whether the products' properties are equal.           
            if (!AdvEqual(TransportationSectionId, other.TransportationSectionId))
                return false;

            // Check whether the products' properties are equal.           
            if (!AdvEqual(TransporationTypeId, other.TransporationTypeId))
                return false;

            if (!AdvEqual(TransportationType, other.TransportationType))
                return false;


            return true;
        }

        #endregion IEquatable<T>
    }
}
