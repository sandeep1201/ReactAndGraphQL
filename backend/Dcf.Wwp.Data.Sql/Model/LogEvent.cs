using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Dcf.Wwp.Data.Sql.Model
{
    public partial class LogEvent
    {
        #region Properties

        public int            Id              { get; set; }
        public string         Message         { get; set; }
        public string         MessageTemplate { get; set; }
        public byte?          Level           { get; set; }
        public DateTimeOffset TimeStamp       { get; set; }
        public string         Exception       { get; set; }
        public string         Properties      { get; set; }

        public XElement PropertiesXML
        {
            get { return (XElement.Parse(Properties)); }
            set { Properties = value.ToString(); }
        }


        [Column("LogEvent")]
        public string Details { get; set; } //TODO: Config file - prop name cannot be same as enclosing type (ie, as class name)

        #endregion

        #region Navigation Properties

        #endregion
    }
}
