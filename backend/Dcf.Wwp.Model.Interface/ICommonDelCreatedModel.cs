namespace Dcf.Wwp.Model.Interface
{
    /// <summary>
    /// An extension of the Common Delete Model interface that adds the CreatedDate column.
    /// </summary>
    public interface ICommonDelCreatedModel : ICommonDelModel, IIsCreated
    {
        
    }
}