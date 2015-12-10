using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Timesheet.Domain;

namespace Timesheet.STS.Configuration
{
    static class Scopes
    {
        public static List<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                StandardScopes.OpenId,
                StandardScopes.Profile,
                StandardScopes.Email,
                
                new Scope
                {
                    Name = TimesheetConstants.ApiScope,
                    DisplayName = "Timesheet API",
                    Type = ScopeType.Resource,
                    Claims = new List<ScopeClaim>
                    {
                        new ScopeClaim(Constants.ClaimTypes.Name, true)
                    }
                }
            };

            return scopes;
        }
    }
}