using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dcf.Wwp.Data.Sql.Model
{
    /// <remarks>
    /// This entity does not have a 'RowVersion', so it does not inherit from 'BaseEntity'.cs
    /// </remarks>
    public partial class WebServiceMessage
    {
        #region Properties

        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid      MsgId            { get; set; }
        public DateTime  MsgDateTime      { get; set; }
        public string    MsgEndpoint      { get; set; }
        public string    MsgOperation     { get; set; }
        public string    MsgDirection     { get; set; }
        public string    MsgXml           { get; set; }
        public DateTime? MsgLocalDateTime { get; set; } // not sure why this was defined as 'nullable' - all the others are defined as 'required'....

        #endregion

        #region Navigation Properties

        #endregion
    }
}
