using System.Web.Http;
using FluentValidation.WebApi;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Timesheet.Api.Startup))]

namespace Timesheet.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:44333",
                ValidationMode = ValidationMode.ValidationEndpoint,
                RequiredScopes = new[] { "timesheet-api" }
            });

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.Filters.Add(new AuthorizeAttribute());

            FluentValidationModelValidatorProvider.Configure(config);

            app.UseWebApi(config);
        }
    }
}
