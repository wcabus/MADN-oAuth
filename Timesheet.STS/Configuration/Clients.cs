using System.Collections.Generic;
using IdentityServer3.Core.Models;
using Timesheet.Domain;

namespace Timesheet.STS.Configuration
{
    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                // Definition for the mobile app
                new Client
                {
                    ClientName = "Timesheet Mobile App",
                    ClientId = TimesheetConstants.ClientId, //Used to identify this app in the STS
                    Enabled = true,

                    Flow = Flows.Hybrid,

                    // List of scopes which this application can ask to receive the claims from.
                    // Note that this list can only contains scopes configured in the Scopes.cs file.
                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.OfflineAccess.Name, // Needed for requesting refresh tokens
                        TimesheetConstants.ApiScope // Needed to be able to access the API
                    },

                    // Client secret needed to retrieve the first pair of access/refresh tokens.
                    ClientSecrets = new List<Secret>
                    {
                        new Secret("mysupersecretkey".Sha256())
                    },

                    // The unique redirect URI of our mobile app (retrieved by calling WebAuthenticationBroker.GetCurrentApplicationCallbackUri())
                    RedirectUris = new List<string>
                    {
                        "ms-app://s-1-15-2-2229323805-4229103899-2707766196-3752596484-745237967-1254760863-2047913441/"
                    },

                    // The lifetime of the access token, in seconds. Defaults to 3600.
                    AccessTokenLifetime = 300 // 5 minutes
                    
                }
            };
        }
    }
}