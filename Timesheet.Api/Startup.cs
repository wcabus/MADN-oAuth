using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Web.Http;
using FluentValidation.WebApi;
using IdentityModel;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
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
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://timesheetsts.azurewebsites.net",
                ValidationMode = ValidationMode.Local,
                RequiredScopes = new[] { TimesheetConstants.ApiScope },
                NameClaimType = "name",
                RoleClaimType = "role"
            });

            var config = new HttpConfiguration();

            //config.SuppressDefaultHostAuthentication();
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new AuthorizeAttribute());

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            FluentValidationModelValidatorProvider.Configure(config);

            app.UseWebApi(config);
        }
    }
}
