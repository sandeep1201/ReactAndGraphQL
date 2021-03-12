using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.ConnectedServices.GoogleApi
{
    public class GooglePlace : IGooglePlace
    {
        public string Name    { get; set; }
        public string PlaceId { get; set; }
    }
}
