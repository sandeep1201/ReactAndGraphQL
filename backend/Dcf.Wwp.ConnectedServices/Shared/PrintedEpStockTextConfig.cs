using System.Collections.Generic;
using Dcf.Wwp.ConnectedServices.Shared;

namespace Dcf.Wwp.Api
{
    public class pepText
    {
        public string name      { get; set; }
        public string detail    { get; set; }
        public string paragraph { get; set; }
    }

    public class pepStockText
    {
        public string        name { get; set; }
        public List<pepText> text { get; set; }
    }

    public class pepPlacementDescription
    {
        public string name   { get; set; }
        public string detail { get; set; }
    }

    public class PrintedEPStockTextConfig : IPrintedEPStockTextConfig
    {
        public List<pepStockText>            english               { get; set; }
        public List<pepPlacementDescription> Placement_Description { get; set; }
        public List<pepStockText>            spanish               { get; set; }
    }
}
