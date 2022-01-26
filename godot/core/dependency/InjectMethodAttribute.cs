using System;

namespace Lavos.Core.Dependency
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class InjectMethodAttribute : Attribute
    {
        public InjectMethodAttribute()
        { }
    }
}