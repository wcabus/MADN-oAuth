using System;
using System.Security.Cryptography.X509Certificates;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin;
using Owin;
using Serilog;
using Timesheet.STS.Configuration;

[assembly: OwinStartup(typeof(Timesheet.STS.Startup))]

namespace Timesheet.STS
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Simple logging setup. Logs to the debugger console and/or trace log (if enabled in web.config)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Trace()
                .CreateLogger();

            // Configure IdentityServer to use in memory clients, scopes and users.
            // Also assigns a certificate used to sign the tokens with.
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(Users.Get()),

                SigningCertificate = LoadCertificate()
            };

            // Start the IdentityServer middleware using the options defined above
            app.UseIdentityServer(options);
        }

        /// <summary>
        /// Loads the certificate from the bin folder.
        /// </summary>
        /// <remarks>
        /// The certificate file needs to be set as Content and Copy If Newer/Copy Always for this to work.
        /// </remarks>
        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                $@"{AppDomain.CurrentDomain.BaseDirectory}\bin\Certificate.pfx", "test");
        }
    }
}
