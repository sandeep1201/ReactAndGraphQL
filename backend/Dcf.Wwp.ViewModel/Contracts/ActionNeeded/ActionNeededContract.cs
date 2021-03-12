using System;
using System.Collections.Generic;
using Dcf.Wwp.Api.Library.Extensions;

namespace Dcf.Wwp.Api.Library.Contracts.ActionNeeded
{
    public class ActionNeededContract
    {
        public ActionNeededContract()
        {
            Tasks = new List<ActionNeededTaskContract>();
        }

        public int Id { get; set; }
        public int PageId { get; set; }
        public string PageName { get; set; }
        public bool IsNoActionNeeded { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte[] RowVersion { get; set; }

        public List<ActionNeededTaskContract> Tasks { get; private set; }

        public static ActionNeededContract Create(int id, int pageId, string pageName, bool isNoActionNeeded, string modifiedBy, DateTime? modifiedDate, byte[] rowVersion)
        {
            return new ActionNeededContract { Id = id, PageId = pageId, PageName = pageName.SafeTrim(), IsNoActionNeeded = isNoActionNeeded, ModifiedDate = modifiedDate, ModifiedBy = modifiedBy, RowVersion = rowVersion};
        }
    }
}