namespace Dcf.Wwp.Model.Interface.Repository
{
    public interface IMilitaryTrainingSectionRepository
    {
        IMilitaryTrainingSection NewMilitaryTrainingSection(IParticipant parentParticipant, string user);
    }
}
