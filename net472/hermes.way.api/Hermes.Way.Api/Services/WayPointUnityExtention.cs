using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;
using Unity.Extension;

namespace Hermes.Way.Api.Services
{
    public class WayPointUnistyExtention : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Container.RegisterInstance(new TaWayPointsServiceConfig());
            Container.RegisterInstance( new TrWayPointsConfig());
            Container.RegisterSingleton<TaWayPointsService>("agents_service");
            Container.RegisterSingleton<TrWayPointsService>("transport_services");
        }
    }
}