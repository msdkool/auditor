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
                        select p.Name;
            
            return props.ToHashSet();
        }
    }
}
