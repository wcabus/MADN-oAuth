using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using Microsoft.Owin;
using Owin;
using Timesheet.STS.Configuration;

[assembly: OwinStartup(typeof(Timesheet.STS.Startup))]

namespace Timesheet.STS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(new List<InMemoryUser>())
            };

            app.UseIdentityServer(options);
        }
    }
}
