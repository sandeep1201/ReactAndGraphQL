namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistancePopulationTypeBridge : ICommonDelModel
    {
        int? RequestForAssistanceId { get; set; }
        int? PopulationTypeId       { get; set; }

        IRequestForAssistance RequestForAssistance { get; set; }
        IPopulationType       PopulationType       { get; set; }
    }
}
