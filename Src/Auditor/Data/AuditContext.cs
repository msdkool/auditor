using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Auditor.Entities;
using Auditor.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Auditor.Data
{
    public abstract class AuditContext : DbContext
    {

        public AuditContext(DbContextOptions<AuditContext> options)
        : base(options)
        {
        }

        protected AuditContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<AuditTrail> AuditTrail { get; set; }
        public DbSet<AuditTrailDetail> AuditTrailDetail { get; set; }

        
        public override int SaveChanges()
        {
            var entityList = AttributesHelper.GetAuditedEntities(this);
            ChangeTracker.DetectChanges();

            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => entityList.Contains(p.Entity.GetType().Name) && (p.State == EntityState.Modified
                                    || p.State == EntityState.Added
                                    || p.State == EntityState.Deleted
                                    || p.State == EntityState.Modified
                                    || p.State == EntityState.Detached))
                                    .Select(x => x).ToList();

            var now = DateTime.UtcNow;

            foreach (var entity in modifiedEntities)
            {
                StartAudit(entity);
            }

            var validationErrors = ChangeTracker
                    .Entries<IValidatableObject>()
                    .SelectMany(e => e.Entity.Validate(null))
                    .Where(r => r != ValidationResult.Success);

            if (validationErrors.Any())
            {
                // Possibly throw an exception here
                foreach (var err in validationErrors)
                {
                    Trace.TraceError(err.ErrorMessage);
                }

            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entityList = AttributesHelper.GetAuditedEntities(this);
            ChangeTracker.DetectChanges();

            var modifiedEntities = ChangeTracker.Entries()
                .Where(p => entityList.Contains(p.Entity.GetType().Name) && (p.State == EntityState.Modified
                                    || p.State == EntityState.Added
                                    || p.State == EntityState.Deleted
                                    || p.State == EntityState.Modified
                                    || p.State == EntityState.Detached))
                                    .Select(x => x).ToList();

            var now = DateTime.UtcNow;

            foreach (var entity in modifiedEntities)
            {
                StartAudit(entity);
            }

            var validationErrors = ChangeTracker
                    .Entries<IValidatableObject>()
                    .SelectMany(e => e.Entity.Validate(null))
                    .Where(r => r != ValidationResult.Success);

            if (validationErrors.Any())
            {
                // Possibly throw an exception here
                foreach (var err in validationErrors)
                {
                    Trace.TraceError(err.ErrorMessage);
                }

            }

            return await base.SaveChangesAsync();
        }

        public virtual string GetUserName()
        {
            IHttpContextAccessor _accessor = new HttpContextAccessor();
            return _accessor.HttpContext?.User?.Identity?.Name;
        } 

        private void StartAudit(EntityEntry entry)
        {
            var auditTrail = new AuditTrail();
            auditTrail.ActionBy = GetUserName();
            auditTrail.ActionEntity = entry.Entity.GetType().Name;
            auditTrail.AuditDate = DateTime.Now;

            var detail = AuditHelper.LoadEntiyProperty(entry, auditTrail);
            this.AuditTrail.Add(detail);
        }

        protected virtual string GetColumnName<T>(T entity)
        {
            var keyName = this.Model.FindEntityType(entity.GetType()).GetProperties()
                .Select(x => x.Relational().ColumnName).Single();

            return keyName;
        }
    }
}
