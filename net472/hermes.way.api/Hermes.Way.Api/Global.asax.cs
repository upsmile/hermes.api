using System.Web.Http;
using System.Web.Routing;
using Hermes.Way.Api.Services;
using NSwag.AspNet.Owin;

namespace Hermes.Way.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            UnityResolver.Resolve();
            RouteTable.Routes.MapOwinPath("swagger", app =>
            {
                app.UseSwaggerUi3(typeof(WebApiApplication).Assembly, settings =>
                {
                    settings.GeneratorSettings.Title =  typeof(WebApiApplication).Assembly.GetName().Name;
                    settings.GeneratorSettings.Version =
                        typeof(WebApiApplication).Assembly.GetName().Version.ToString();
                    settings.GeneratorSettings.Description = "API полученияя точек маршрутов транспортных средств";
                    settings.MiddlewareBasePath = "/swagger";
                });
            });
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
