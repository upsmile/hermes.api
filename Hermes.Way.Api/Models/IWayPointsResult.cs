using System;

namespace Hermes.Way.Api.Services
{
    /// <summary>
    ///  результат
    /// </summary>
    public interface IWayPointsResult
    {
       object Result { get; set; }
       Exception Exception { get; set; }
    }
}
