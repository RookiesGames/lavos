using System;

namespace Vortico.Core.Dependency
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute()
        { }
    }
}