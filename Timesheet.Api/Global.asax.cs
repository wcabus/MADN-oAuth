using System.Web.Http;
using FluentValidation.WebApi;

namespace Timesheet.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            FluentValidationModelValidatorProvider.Configure(GlobalConfiguration.Configuration);
        }
    }
}
