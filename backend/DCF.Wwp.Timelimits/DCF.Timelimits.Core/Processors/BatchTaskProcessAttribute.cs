using System;

namespace DCF.Timelimits.Core.Processors
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class BatchTaskProcessAttribute : Attribute
    {
        public UInt32 Priority { get; set; }
    }
}