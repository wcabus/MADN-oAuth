using System.Collections.Generic;
using IdentityServer3.Core;
using IdentityServer3.Core.Models;
using Timesheet.Domain;

namespace Timesheet.STS.Configuration
{
    static class Scopes
    {
        /// <summary>
        /// This method returns the scopes supported by this STS, for any client.
        /// </summary>
        /// <returns></returns>
        public static List<Scope> Get()
        {
            var scopes = new List<Scope>
            {
                StandardScopes.OpenId, // Needed for any flow using OpenID Connect
                StandardScopes.Profile, // Returns first name, family name, ... claims
                StandardScopes.Email,
                StandardScopes.OfflineAccess, // This scope is used to get refresh tokens

                // Declaration of a new resource scope for our API.
                // There are to types of scopes: 
                // - Identity: a scope which tells more about the identity of a user
                // - Resource: a scope which grants access to a resource
                new Scope
                {
                    Name = TimesheetConstants.ApiScope, // Actually, the identifier of our API
                    DisplayName = "Timesheet API", // Name used to display on the consent page
                    Type = ScopeType.Resource,
                    // Claims to be returned in this scope
                    Claims = new List<ScopeClaim>
                    {
                        // Name claim, needed for Web API to make User.Identity.IsAuthenticated work.
                        new ScopeClaim(Constants.ClaimTypes.Name, true) // true: include this claim in the token without the need to call the userinfo endpoint.
                    }
                }
            };

            return scopes;
        }
    }
}