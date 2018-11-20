using System;

namespace Hermes.Way.Api.Models
{
    public sealed class TaWayPointsResult : IWayPointsResult
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}
