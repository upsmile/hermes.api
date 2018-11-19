using Unity;
using Unity.Extension;
namespace Hermes.Way.Api.Services
{
    public sealed class WayPointsServiceExtention : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterSingleton<TaWayPointsService>();            
        }
    }
}
