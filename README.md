# MailboxReader
Sample project which is able to read the mailbox of a user using Azure Active Directory and Office 365 using Microsoft Graph API

With this tool you're able to list the e-mails of a configured mailbox. Of course, it's necessary to have the appropriate permissions from Azure Active Directory & in Office 365.

## Run the project

In order to run the project you need the following settings
```json
{
  "AppRegistration:ApplicationId": "<The Application Id of the App Registration in Azure Active Directory>",
  "AppRegistration:ApplicationSecret": "<The Secret created in the App Registration>",
  "AppRegistration:TenantId": "<The unique identifier of the tenant>",
  "AppRegistration:RedirectUri": "<The redirect uri configured in the App Registration>",
  "AppRegistration:Domain": "<The domainname of your tenant most of the time something like [name].onmicrosoft.com>",
  "Query:UserEmailAddress": "<the configured mailbox, like person@mail.com>",
  "Query:UserIdentifier": "<unique identifier of the user you want to read the mail from>",
  "Query:FindUser": "<when using the e-mail address and find user, set to `true`, if finding mailbox using identifier set to `false`"
}
```

## Configuring Azure Active Directory

1. Create a new App Registration in Azure Active Directory
2. For the `Redirect URI` use something like `https://localhost:8080/`
3. Add a new secret in the App Registration and make sure to copy the value, so you can put it in the configuration later on
4. Add the following `API permissions`
    * `Mail.Read`
    * `User.Read`
    * `User.Read.All`

Having configured this, your application is now able to query all mailboxes within your tenant, so hurry up to the next chapter.

## Configuring Office 365 / Exchange

These steps are taken from the [Microsoft Graph documentation](https://docs.microsoft.com/en-us/graph/auth-limit-mailbox-access#configure-applicationaccesspolicy).

1. Connect to Exchange Online PowerShell
2. Create an application access policy.
```powershell
New-ApplicationAccessPolicy -AppId <ApplicationId of the App Registration> -PolicyScopeGroupId <GroupId where policy applies to> -AccessRight RestrictAccess -Description "<A nice description>"
```
3. Test the access policy
```powershell
Test-ApplicationAccessPolicy -Identity <user in the specified GroupId> -AppId <ApplicationId of the App Registration>
```
Do note, the synchronisation of the access restriction can take a long time. The docs state 30 minutes, but I noticed it can take longer.

# Credits
Got a lot of inspiration & implementation from [the Microsoft Graph .NET Core samples](https://github.com/microsoftgraph/dotnetcore-console-sample)