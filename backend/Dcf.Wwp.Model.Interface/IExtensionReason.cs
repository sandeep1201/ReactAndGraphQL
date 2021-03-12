using System;
using System.Collections.Generic;

namespace Dcf.Wwp.Model.Interface
{
    public interface IExtensionReason : ICommonDelCreatedModel
    {
        string Code { get; set; }
        String Name { get; set; }
        ICollection<ITimeLimitExtension> TimeLimitExtensions { get; set; }

        ICollection<ITimeLimitType> TimeLimitTypes { get; set; }
    }

    public interface IApprovalReason : IExtensionReason
    {
    }

    public interface IDenialReason : IExtensionReason
    {
    }
}