using System;
namespace Hermes.Way.Api.Services
{
    public sealed class TrWayPointsResult : IWayPointsResult
    {
        public object Result { get; set; }
        public Exception Exception { get; set; }
    }
}
