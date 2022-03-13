using System;

namespace Lavos.Dependency
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class InjectMethodAttribute : Attribute
    {
        public InjectMethodAttribute()
        { }
    }
}