using System.Collections.Generic;
using Dcf.Wwp.Api;

namespace Dcf.Wwp.ConnectedServices.Shared
{
    public interface IPrintedEPStockTextConfig
    {
        List<pepStockText>            english               { get; set; }
        List<pepPlacementDescription> Placement_Description { get; set; }
        List<pepStockText>            spanish               { get; set; }
    }

    public interface IpepStockText
    {
        string        name { get; set; }
        List<pepText> text { get; set; }
    }

    public interface IpepText
    {
        string name      { get; set; }
        string detail    { get; set; }
        string paragraph { get; set; }
    }

    public interface IpepPlacementDescription
    {
        string name   { get; set; }
        string detail { get; set; }
    }
}
