using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Timesheet.STS.Configuration
{
    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientName = "Timesheet Mobile App",
                    ClientId = "timesheet-mobile-app",
                    Enabled = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    Flow = Flows.Implicit,
                    AllowedScopes = new List<string>
                    {
                        "timesheet-api"
                    }
                }
            };
        }
    }
}