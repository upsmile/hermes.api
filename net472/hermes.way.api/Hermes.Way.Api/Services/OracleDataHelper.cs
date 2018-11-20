using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Distributor.DataClasses;
using DISTRDEVDataContext;
using Hermes.Way.Api.Models;
using Newtonsoft.Json;

namespace Hermes.Way.Api.Services
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public sealed class OracleDataHelper
    {
        private static readonly Func<VShopAddressTer, bool> IsCoordinatesIsNull = t => !t.Latitude.HasValue && !t.Longtitude.HasValue;

        private static object CreateDataConnection()
        {
            try
            {
                return new DISTRDEVDataContext.DISTRDEVDataContext(ConfigurationManager.ConnectionStrings["distributor"].ConnectionString);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        public static List<double> GetDeliveryVehiclesPoint(DateTime docDate, double carId)
        {
            var result = new List<double>();
            var dataConnection = CreateDataConnection();
            if (dataConnection.GetType() == typeof(Exception))
                throw new Exception(typeof(DistrDeliveryPointsDict).ToString());

            var distrdevDataContext = (DISTRDEVDataContext.DISTRDEVDataContext)dataConnection;
            distrdevDataContext.Connection.Open();
            var car = distrdevDataContext.VDicCars.FirstOrDefault(x => x.Id.Equals(carId));
            if (car != null)
            {
                var begindate = docDate.ToBeginDay();
                var enddate = docDate.ToEndDay();
                var codes = distrdevDataContext.VDeliverypointsbydocs.Where(items =>
                        items.CarId == car.Id &
                        (items.DocDate >= (DateTime?) begindate && 
                         items.DocDate < (DateTime?) enddate) &&
                        items.LatitudeDegrees != null &&
                        items.LatitudeMinutes!= null && 
                        items.LatitudeSeconds != null && 
                        items.LongitudeDegrees != null &&
                        items.LongitudeMinutes != null && 
                        items.LongitudeSeconds != null)
                    .Select(items => new { items.Code})
                    .Distinct()
                    .ToList()
                    .Select(pc => pc.Code)
                    .ToList();
                result.AddRange(codes.Where(x => x.HasValue).Select(x => x.Value));
            }
            distrdevDataContext.Connection.Close();
            return result;
        }

        public static string GetShopAdressInfo(double shopcode)
        {
            var str = string.Empty;
            try
            {
                var dataConnection = (DISTRDEVDataContext.DISTRDEVDataContext)CreateDataConnection();
                dataConnection.Connection.Open();
                var vshopAddressTer = dataConnection.VShopAddressTers.FirstOrDefault(x => x.ShopCode.Equals(shopcode));
                if (vshopAddressTer != null)
                    str = JsonConvert.SerializeObject(new List<string>
                    {
                        $"Код РТТ: {vshopAddressTer.ShopCode}",
                        IsCoordinatesIsNull(vshopAddressTer).ToCoordinatesResult(),
                        vshopAddressTer.ContracorName                        
                    });
                dataConnection.Connection.Close();
            }
            catch (Exception ex)
            {
                str = JsonConvert.SerializeObject(ex);
            }
            return str;
        }


        public event AgentPointsEvent BeginLoadData;

        public event AgentPointsEvent EndLoadData;

        private void OnBeginLoadData(AgentPointsArgument arg)
        {
            var beginLoadData = BeginLoadData;
            beginLoadData?.Invoke(arg);
        }

        private void OnEndLoadData(AgentPointsArgument arg)
        {
            var endLoadData = EndLoadData;
            endLoadData?.Invoke(arg);
        }

        public void GetTaPoints(double positionId, DateTime dateBegin, DateTime dateEnd)
        {
            var agentPointsArgument = new AgentPointsArgument();
            try
            {
                using (var distrdevDataContext =
                    new DISTRDEVDataContext.DISTRDEVDataContext(ConfigurationManager.ConnectionStrings["distributor"].ConnectionString))
                {
                    agentPointsArgument.Result = distrdevDataContext.Connection.ConnectionString;
                    OnBeginLoadData(agentPointsArgument);
                    distrdevDataContext.Connection.Open();
                    var list = distrdevDataContext.GetAgentPoint(positionId, dateBegin, dateEnd).ToList().Where(item => item.Code.HasValue).Select(item => item.Code.Value).ToList();
                    agentPointsArgument.Result = list;
                    distrdevDataContext.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                agentPointsArgument.Exception = ex;
                OnEndLoadData(agentPointsArgument);
                return;
            }
            OnEndLoadData(agentPointsArgument);
        }
    }
}
