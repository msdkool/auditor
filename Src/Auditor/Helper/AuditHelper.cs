using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Auditor.Entities;
using Auditor.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Auditor.Helper
{
    public class AuditHelper
    {
        private static bool IsReference(EntityEntry entry, string memberName)
        {
            var reference = entry.Member(memberName) as ReferenceEntry;
            return reference != null;
        }

        public static AuditTrail LoadEntiyProperty(EntityEntry entry, AuditTrail auditTrail)
        {
            var allProps = entry.Entity.GetType().GetTypeInfo().DeclaredProperties;
            var props = allProps
                // Ignore NotMapped props to avoid the following error 'The property could not be found. Ensure that the property exists and has been included in the model.'
                .Where(p => p.GetCustomAttributes(typeof(NotMappedAttribute)).Count() == 0)
                .Where(p => p.PropertyType == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(p.PropertyType));

            foreach (var prop in props)
            {
                if (!prop.GetGetMethod().IsVirtual && !IsReference(entry, prop.Name))
                {

                    var currentValue = entry.Property(prop.Name).CurrentValue;
                    var originalValue = entry.Property(prop.Name).OriginalValue;

                    currentValue = currentValue ?? string.Empty;
                    originalValue = originalValue ?? string.Empty;

                    var auditTrailDetail = new AuditTrailDetail();

                    TypeHelper.IsCoreType(prop.GetType());
                    {
                        auditTrailDetail.ColumnName = prop.Name;
                        auditTrailDetail.NewRecord = currentValue.ToString();

                        if (entry.State == EntityState.Added)
                        {
                            auditTrail.Action = ChangeType.Insert.ToString();
                        }
                        else if (entry.Property(prop.Name).IsModified)
                        {
                            auditTrailDetail.OldRecord = originalValue.ToString();
                            auditTrail.Action = ChangeType.Update.ToString();
                        }
                        else if (entry.State == EntityState.Deleted)
                        {
                            auditTrailDetail.OldRecord = originalValue.ToString();
                            auditTrail.Action = ChangeType.Delete.ToString();
                        }

                        auditTrail.AuditTrailDetail.Add(auditTrailDetail);
                    }
                }
            }

            return auditTrail;
        }

    }
}
