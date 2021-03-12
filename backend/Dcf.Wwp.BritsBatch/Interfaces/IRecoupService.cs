using System.Xml.Linq;
using Dcf.Wwp.BritsBatch.Models;

namespace Dcf.Wwp.BritsBatch.Interfaces
{
    public interface IRecoupService
    {
        #region Properties

        #endregion

        #region Methods

        XDocument          GetRecoupResponse(string  getRecoupJson,  bool isSimulation);
        PostRecoupResponse PostRecoupResponse(string postRecoupJson, bool isSimulation);

        #endregion
    }
}
