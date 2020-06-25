using System;
using System.Threading.Tasks;
using MailboxReader.ConsoleApp.Mailbox;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MailboxReader.ConsoleApp
{
    internal class App
    {
        private readonly IGet _get;
        private readonly QuerySettings _querySettings;
        private readonly ILogger<App> _logger;

        public App(
            IGet get,
            IOptions<QuerySettings> querySettings,
            ILogger<App> logger)
        {
            _get = get;
            _querySettings = querySettings.Value;
            _logger = logger;
        }
        public async Task Run(string[] args)
        {
            var messages = await _get.ListInboxMessages(_querySettings.ActiveIdentifier);
            foreach (var message in messages)
            {
                _logger.LogInformation($"Found email id `{message.Id}`{Environment.NewLine}with subject `{message.Subject}`.");
            }

            Console.ReadLine();
        }
    }
}