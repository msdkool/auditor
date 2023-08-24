using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Auditor.Data;

namespace Auditor.Helper
{
    public class AttributesHelper
    {
        public static HashSet<string> GetAuditedEntities<T>(T dbContext)
        {
            var props = from p in dbContext.GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(AuditAttribute), true)
                        where attr.Length == 1
                        select p;
            // Gets the name of a generic type an element inside of IQueryable<T>
            return props.Select(p => p.PropertyType.GetGenericArguments()[0].Name).ToHashSet();
        }
    }
}
