using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace Timesheet.STS.Configuration
{
    static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                {
                    Name = "timesheet-api"
                }
            };
        }
    }
}