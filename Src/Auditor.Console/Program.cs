using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Auditor.Console.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Auditor.Console
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configBuilder) =>
                {
                    configBuilder.SetBasePath(Directory.GetCurrentDirectory());
                    configBuilder.AddJsonFile("appsettings.json", optional: false);
                    configBuilder.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
                    configBuilder.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    configLogging.AddConsole();
                    configLogging.AddSerilog();
                    configLogging.AddDebug();
                    //var logPath = hostContext.Configuration.GetSection("LogPath").Value;
                    //var logFlag = int.TryParse(hostContext.Configuration.GetSection("LogSize").Value, out int logSize);
                    //logSize = logFlag ? logSize : 30000000;

                    Log.Logger = new LoggerConfiguration()
                                     .ReadFrom.Configuration(hostContext.Configuration)
                                     .MinimumLevel.Information()
                                     .Enrich.FromLogContext()
                                     .CreateLogger();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    string connectionString = hostContext.Configuration.GetConnectionString("AuditorDbConnection");
                    var migrationsAssembly = typeof(AuditorConsoleContext).GetTypeInfo().Assembly.GetName().Name;

                    services
                     .AddDbContext<AuditorConsoleContext>(options => options
                                  .UseSqlServer(connectionString,
                                  optionsBuilder => { optionsBuilder.CommandTimeout((int)TimeSpan.FromMinutes(1).TotalSeconds); optionsBuilder.MigrationsAssembly(migrationsAssembly); }));

                    services.AddSingleton(hostContext.Configuration);

                    services.AddHostedService<ConsoleHostedService>();
                });

            await hostBuilder.RunConsoleAsync();
        }
    }
}
