using System;

namespace Dcf.Wwp.Model.Interface
{
    public interface ILoginRole : ICommonDelModel
    {
        int WorkerLoginId { get; set; }
        int RoleId { get; set; }

        IRole Role { get; set; }
    }
}