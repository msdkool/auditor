using System;
using System.Threading;
using System.Threading.Tasks;
using Auditor.Console.Data;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Auditor.Console
{
    public class ConsoleHostedService : IHostedService, IDisposable
    {
        private readonly ILogger<ConsoleHostedService> _logger;
        private AuditorConsoleContext _auditorConsoleContext;

        public ConsoleHostedService(AuditorConsoleContext auditorConsoleContext, ILogger<ConsoleHostedService> logger)
        {
            _logger = logger;
            _auditorConsoleContext = auditorConsoleContext;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _auditorConsoleContext.Person.AddAsync(new Entities.Person
            {
                Name = "John Smith",
                Age = 20,
                Address = "My address at home"
            });

            _auditorConsoleContext.SaveChanges();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
