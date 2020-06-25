using System.Threading.Tasks;
using MailboxReader.ConsoleApp.Infrastructure;
using MailboxReader.ConsoleApp.Mailbox;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
    using Microsoft.Extensions.Configuration.UserSecrets;

namespace MailboxReader.ConsoleApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var services = new ServiceCollection()
                .AddLogging(l => l.AddConsole())
                .Configure<LoggerFilterOptions>(c => c.MinLevel = LogLevel.Trace)
                .AddOptions();
            
            ConfigureServices(services);
            
            var serviceProvider = services.BuildServiceProvider();
            await serviceProvider.GetService<App>().Run(args);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // build config
            var configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())                
                .AddJsonFile("appsettings.json", optional: false)
                .AddEnvironmentVariables()
                .AddUserSecrets<AppRegistrationSettings>()
                .AddUserSecrets<QuerySettings>()
                .Build();
            
            services.Configure<AppRegistrationSettings>(configuration.GetSection("AppRegistration"));
            services.Configure<QuerySettings>(configuration.GetSection("Query"));

            services.AddSingleton<AuthorizationProvider>();
            services.AddSingleton<GraphServiceClient>(s =>
            {
                var authenticationProvider = s.GetService<AuthorizationProvider>().Create();
                var graphServiceClient = new GraphServiceClient(authenticationProvider);
                return graphServiceClient;
            });
            
            services.AddTransient<IGet>(s =>
            {
                if (s.GetService<IOptions<QuerySettings>>().Value.FindUser)
                {
                    return new GetByEmail(s.GetService<GraphServiceClient>());
                }
                else
                {
                    return new GetByIdentifier(s.GetService<GraphServiceClient>());
                }
            });
            
            services.AddTransient<App>();
        }
    }
}