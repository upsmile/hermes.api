using Hermes.Way.Api.Services;
using System.Web.Http;
using Unity;

namespace Hermes.Way.Api
{

    public static class UnityResolver
    {
        public static IUnityContainer unity;

        static UnityResolver()
        {
            unity = new UnityContainer();
            
        }

        public static void Resolve()
        {
            unity.AddNewExtension<WayPointUnistyExtention>();
        }
    }
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.Clear();
            GlobalConfiguration.Configuration.Formatters.Add(new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            UnityResolver.Resolve();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
