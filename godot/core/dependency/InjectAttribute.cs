using System;

namespace Lavos.Core.Dependency
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute()
        { }
    }
}