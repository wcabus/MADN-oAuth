using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using Timesheet.Domain;

namespace Timesheet.STS.Configuration
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "wcuah73",
                    Password = "secret",
                    Subject = "1",

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
