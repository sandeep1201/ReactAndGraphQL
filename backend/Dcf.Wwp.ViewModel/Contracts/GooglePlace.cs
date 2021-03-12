using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public class GooglePlace : IGooglePlace
    {
        public string Name    { get; set; }
        public string PlaceId { get; set; }
    }
}
