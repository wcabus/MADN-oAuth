﻿using System.Collections.Generic;
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
                new Client
                {
                    ClientName = "Timesheet Mobile App",
                    ClientId = TimesheetConstants.ClientId,
                    Enabled = true,

                    Flow = Flows.Hybrid,

                    AllowedScopes = new List<string>
                    {
                        StandardScopes.OpenId.Name,
                        StandardScopes.Profile.Name,
                        StandardScopes.Email.Name,
                        StandardScopes.OfflineAccess.Name,
                        TimesheetConstants.ApiScope
                    },

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("mysupersecretkey".Sha256())
                    },

                    RedirectUris = new List<string>
                    {
                        "ms-app://s-1-15-2-2229323805-4229103899-2707766196-3752596484-745237967-1254760863-2047913441/"
                    },

                    AccessTokenLifetime = 60,
                    
                }
            };
        }
    }
}