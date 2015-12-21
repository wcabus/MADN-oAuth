using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;

namespace Timesheet.STS.Configuration
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                // Declare a user
                new InMemoryUser
                {
                    Username = "wcuah73",
                    Password = "secret",
                    Subject = "1", // Subject = unique identifier

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Wesley"),
                        new Claim(Constants.ClaimTypes.FamilyName, "Cabus"),
                        new Claim(Constants.ClaimTypes.Email, "wesley.cabus@realdolmen.com"),
                        new Claim(Constants.ClaimTypes.Name, "WCUAH73"),
                        new Claim(Constants.ClaimTypes.Role, "")
                    }
                }
            };
        } 
    }
}
