namespace Ergate.Common.Extensions.Priority
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PriorityAttribute : Attribute
    {
        public PriorityAttribute(int value)
        {
            this.Value = value;
        }

        public int Value { get; private set; }
    }
}