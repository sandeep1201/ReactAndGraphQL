using Dcf.Wwp.Batch.Interfaces;
using TinyCsvParser.Mapping;

namespace Dcf.Wwp.Batch.Models.Parsers
{
    public class TinyCsvPEPMapping : CsvMapping<PEPLine>, IMappable
    {
        #region Properties

        #endregion

        #region Methods

        public TinyCsvPEPMapping()
        {
            MapProperty(0, i => i.PEPId);
            MapProperty(1, i => i.CompletionReasonCd);
            MapProperty(2, i => i.EnrolledProgramId);
        }

        #endregion
    }
}
