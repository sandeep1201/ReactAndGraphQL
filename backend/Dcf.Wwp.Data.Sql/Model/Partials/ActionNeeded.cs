using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    [MetadataType(typeof(ModelExtension))]
    public partial class ActionNeeded : BaseCommonModel, IActionNeeded, IEquatable<ActionNeeded>
    {
        IActionNeededPage IActionNeeded.ActionNeededPage
        {
            get => ActionNeededPage;
            set => ActionNeededPage = (ActionNeededPage) value;
        }

        ICollection<IActionNeededTask> IActionNeeded.ActionNeededTasks
        {
            // Limit these to only the non-deleted items.
            get { return ActionNeededTasks.Where(x => !x.IsDeleted).Cast<IActionNeededTask>().ToList(); }
            set => ActionNeededTasks = value.Cast<ActionNeededTask>().ToList();
        }

        #region ICloneable

        public object Clone()
        {
            var clone = new ActionNeeded
                        {
                            Id                 = Id,
                            ParticipantId      = ParticipantId,
                            ActionNeededPageId = ActionNeededPageId,
                            IsNoActionNeeded   = IsNoActionNeeded,
                            IsDeleted          = IsDeleted
                        };

            return clone;
        }

        #endregion ICloneable

        #region IEquatable<T>

        public override bool Equals(object other)
        {
            if (other == null)
            {
                return false;
            }

            var obj = other as ActionNeeded;

            return obj != null && Equals(obj);
        }

        public bool Equals(ActionNeeded other)
        {
            //Check whether the compared object is null.
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            //Check whether the compared object references the same data.
            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //Check whether the products' properties are equal.

            if (!AdvEqual(Id, other.Id))
            {
                return false;
            }

            if (!AdvEqual(ParticipantId, other.ParticipantId))
            {
                return false;
            }

            if (!AdvEqual(ActionNeededPageId, other.ActionNeededPageId))
            {
                return false;
            }

            if (!AdvEqual(IsNoActionNeeded, other.IsNoActionNeeded))
            {
                return false;
            }

            if (!AdvEqual(IsDeleted, other.IsDeleted))
            {
                return false;
            }

            return true;
        }

        #endregion IEquatable<T>
    }
}
