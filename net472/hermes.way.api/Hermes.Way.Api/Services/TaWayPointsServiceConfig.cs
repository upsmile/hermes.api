using System;

namespace Hermes.Way.Api.Services
{
    public sealed class TaWayPointsServiceConfig : IWayPointsServiceConfig
    {
        public int ServiceType { get; set; }
        public double Id { get; set; }
        public DateTime Date { get; set; }
    }
}
