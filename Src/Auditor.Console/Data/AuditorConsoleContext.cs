using System;
using Auditor.Console.Entities;
using Auditor.Data;
using Auditor.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Auditor.Console.Data
{
    public class AuditorConsoleContext : AuditContext
    {
        [Audit]
        public DbSet<Person> Person { get; set; }

        public AuditorConsoleContext(DbContextOptions<AuditorConsoleContext> options) : base(options)
        {

        }

        public override string GetUserName()
        {
            return "TestUser";
        }
    }
}
