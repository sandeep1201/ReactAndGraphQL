namespace Dcf.Wwp.Model.Interface
{
    public interface IRequestForAssistanceChild : ICommonModelFinal
    {
        IChild Child { get; set; }

        // This parent objects should not be used for cloning as it will cause recursive
        // calls (bad).  We do need it though since a whole new object graph could be
        // created and we don't yet have an ID.
        IRequestForAssistance RequestForAssistance { get; set; }
    }
}
