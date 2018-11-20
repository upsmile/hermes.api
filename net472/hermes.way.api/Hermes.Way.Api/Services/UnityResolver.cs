using Unity;

namespace Hermes.Way.Api.Services
{
    public static class UnityResolver
    {
        public static readonly IUnityContainer Unity;

        static UnityResolver()
        {
            Unity = new UnityContainer();
            
        }

        public static void Resolve()
        {
            Unity.AddNewExtension<WayPointUnistyExtention>();
            Unity.AddNewExtension<LoggerContainerExtention>();
        }
    }
}