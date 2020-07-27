using System;
namespace Auditor.Helper
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class AuditAttribute : Attribute
    {
        public AuditAttribute()
        {
        }

    }
}
