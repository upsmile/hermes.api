using System.Web.Http;
using Hermes.Way.Api.Services;
using Serilog;
using Unity;

namespace Hermes.Way.Api.Models
{
    public abstract class WayControllerBase:ApiController
    {
        protected readonly ILogger Logger;
        protected readonly IUnityContainer Unity;

        protected WayControllerBase()
        {
            Unity = UnityResolver.Unity;
            Logger = Unity.Resolve<ILogger>();
        }
    }
}