using System;

namespace DCF.Core.Configuration
{
    public interface ISettingInfo
    {
        String Name { get; set; }
        Int64? UserId { get; set; }
        String Value { get; set; }
    }
}