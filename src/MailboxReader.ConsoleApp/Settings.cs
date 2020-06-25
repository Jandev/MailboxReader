namespace MailboxReader.ConsoleApp
{
    internal class AppRegistrationSettings
    {
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string TenantId { get; set; }
        public string RedirectUri { get; set; }
        public string Domain { get; set; }
    }

    internal class QuerySettings
    {
        public string UserEmailAddress { get; set; }
        public string UserIdentifier { get; set; }
        public bool FindUser { get; set; }

        public string ActiveIdentifier
        {
            get
            {
                if (FindUser)
                {
                    return UserEmailAddress;
                }

                return UserIdentifier;
            }
        }
    }
}