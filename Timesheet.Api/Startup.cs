using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Http;
using FluentValidation.WebApi;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Timesheet.Domain;

[assembly: OwinStartup(typeof(Timesheet.Api.Startup))]

namespace Timesheet.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Disables default mapping of incoming claims
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            // Configures authentication using "Bearer" tokens with IdentityServer.
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                // The issuer of the tokens
                Authority = "https://timesheetsts.azurewebsites.net",
                ValidationMode = ValidationMode.Local,
                RequiredScopes = new[] { TimesheetConstants.ApiScope }, // When accessing our API, a token must have the "timesheet-api" scope
                NameClaimType = "name", // Rename the default Name/Role claimtypes from their SOAP versions to IdentityServer.
                RoleClaimType = "role"
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes(); // Use attribute routing

            config.SuppressDefaultHostAuthentication(); // If IIS has set a User on our request, remove it again
            config.Filters.Add(new AuthorizeAttribute()); // Users must be logged on for each request
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType)); // And look for a Bearer token to authenticate a user

            config.Formatters.Remove(config.Formatters.XmlFormatter); // Remove the XML formatter, we're only supporting JSON
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver() // Set up camelCase names
            };

            // Validation for our POST models
            FluentValidationModelValidatorProvider.Configure(config);

            // Start the Web API middleware
            app.UseWebApi(config);
        }
    }
}
