using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class AuxiliaryPayment : BaseEntity, ICommonDelCreatedModel
    {
        private Boolean _isDeleted = false;

        [NotMapped]
        public Boolean IsDeleted
        {
            get { return this._isDeleted; }
            set { this._isDeleted = value; }
        }

        [NotMapped]
        public DateTime? CreatedDate
        {
            get { return CreatedDateFromCARES ?? ModifiedDate; }
            set { CreatedDateFromCARES = value; }
        }
    }
}
