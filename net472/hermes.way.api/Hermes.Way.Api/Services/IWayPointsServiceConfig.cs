using System;

namespace Hermes.Way.Api.Services
{
    /// <summary>
    ///  настройки и конфигурации
    /// </summary>
    public interface IWayPointsServiceConfig
    {
       /// <summary>
       /// тип подключаемого сервиса(1-грузовой, 2-ТА)
       /// </summary>
        int ServiceType { get; set; }

        double Id { get; set; }

        DateTime Date { get; set; }
    }
}
