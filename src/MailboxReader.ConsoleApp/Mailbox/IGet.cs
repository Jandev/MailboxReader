using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace MailboxReader.ConsoleApp.Mailbox
{
    internal interface IGet
    {
        Task<IEnumerable<Email>> ListInboxMessages(string identifier);
    }

    internal class GetByEmail : IGet
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GetByEmail(
            GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IEnumerable<Email>> ListInboxMessages(string identifier)
        {
            User user = FindByEmail(identifier).Result;
            IMailFolderMessagesCollectionPage messages = await _graphServiceClient.Users[user.Id].MailFolders.Inbox
                .Messages.Request().Top(10).GetAsync();

            var items = new List<Email>();
            if (messages?.Count > 0)
            {
                foreach (var message in messages)
                {
                    items.Add(new Email
                    {
                        Subject = message.Subject,
                        Id = message.Id
                    });
                }
            }

            return items;
        }

        private async Task<User> FindByEmail(string email)
        {
            var queryOptions = new List<QueryOption>
            {
                new QueryOption("$filter", $@"mail eq '{email}'")
            };

            var userResult = await _graphServiceClient.Users.Request(queryOptions).GetAsync();
            if (userResult.Count == 1)
            {
                return userResult[0];
            }

            throw new ApplicationException($"Unable to find a user with the alias {email}");
        }
    }

    internal class GetByIdentifier : IGet
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GetByIdentifier(
            GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task<IEnumerable<Email>> ListInboxMessages(string identifier)
        {
            var messages = await _graphServiceClient.Users[identifier].MailFolders.Inbox.Messages.Request().Top(10)
                .GetAsync();

            var items = new List<Email>();
            if (messages?.Count > 0)
            {
                foreach (var message in messages)
                {
                    items.Add(new Email
                    {
                        Subject = message.Subject,
                        Id = message.Id
                    });
                }
            }

            return items;
        }
    }
}