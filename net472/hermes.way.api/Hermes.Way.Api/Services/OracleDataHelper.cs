using DISTRDEVDataContext;
using Distributor.DataClasses;
using GMap.NET;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Hermes.Way.Api.Services
{
    public sealed class OracleDataHelper
    {
        private static readonly Func<VShopAddressTer, bool> IsCoordinatesIsNull = (Func<VShopAddressTer, bool>)(t => !t.Latitude.HasValue && !t.Longtitude.HasValue);

        private object CreateDataConnection()
        {
            try
            {
                return (object)new DISTRDEVDataContext.DISTRDEVDataContext(ConfigurationManager.ConnectionStrings["distributor"].ConnectionString);
            }
            catch (Exception ex)
            {
                return (object)ex;
            }
        }

        public DistrDeliveryPointsDict GetDeliveryVehiclesPoint(DateTime docDate, double carId)
        {
            Dictionary<int, string> dictionary1 = new Dictionary<int, string>();
            Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
            DistrDeliveryPointsDict result = new DistrDeliveryPointsDict();
            object dataConnection = this.CreateDataConnection();
            if (dataConnection.GetType() == typeof(Exception))
                throw new Exception(typeof(DistrDeliveryPointsDict).ToString());

            try
            {
                DISTRDEVDataContext.DISTRDEVDataContext distrdevDataContext = (DISTRDEVDataContext.DISTRDEVDataContext)dataConnection;
                distrdevDataContext.Connection.Open();
                VDicCar car = distrdevDataContext.VDicCars.FirstOrDefault<VDicCar>((Expression<Func<VDicCar, bool>>)(x => x.Id.Equals(carId)));
                if (car != null)
                {
                    DateTime begindate = docDate.ToBeginDay();
                    DateTime enddate = docDate.ToEndDay();
                    DeliveryPointsProperties pointsProperties = new DeliveryPointsProperties()
                    {
                        Car = car,
                        DocDate = docDate
                    };
                    List<double?> codes = distrdevDataContext.VDeliverypointsbydocs.Where(items =>
                    items.CarId == car.Id & (items.DocDate >= (DateTime?)begindate && items.DocDate < (DateTime?)enddate) && 
                    items.LatitudeDegrees != (object)null && items.LatitudeMinutes != (object)null && items.LatitudeSeconds != 
                    (object)null && items.LongitudeDegrees != (object)null && items.LongitudeMinutes != (object)null && items.LongitudeSeconds != (object)null).Select(items => new
                    {
                        Code = items.Code
                    }).Distinct().ToList().Select(pc => pc.Code).ToList();
                    IQueryable<DicShop> qDict = distrdevDataContext.DicShops.Where(items => codes.Contains(items.Code));
                    IQueryable<VDeliverypointsbydoc> source = distrdevDataContext.VDeliverypointsbydocs.Where(items => items.CarId == car.Id & 
                    (items.DocDate >= (DateTime?)begindate && items.DocDate < (DateTime?)enddate));
                    List<double> doubleList = new List<double>();
                    List<double> list = source.Select(items => items.Code.Value).Distinct().ToList();
                    pointsProperties.DocumentPoints = JsonConvert.SerializeObject(list);
                    doubleList.Clear();
                    doubleList.AddRange(list);
                    int num1 = source.Select(items => new
                    {
                        Code = items.Code
                    }).Distinct().Count();
                    pointsProperties.PointsInDocument = num1;
                    var queryable = qDict.Select(items => new
                    {
                        id = items.Id,
                        LatDegree = items.LatitudeDegrees,
                        LatMin = items.LatitudeMinutes,
                        LatS = items.LatitudeSeconds,
                        LonDegree = items.LongitudeDegrees,
                        LonM = items.LongitudeMinutes,
                        LonS = items.LongitudeSeconds,
                        Latitude = items.Latitude,
                        Longtitude = items.Longtitude
                    }).Distinct();
                    int key = 0;
                    List<PointLatLng> pointLatLngList = new List<PointLatLng>();
                    try
                    {
                        foreach (var data in queryable)
                        {
                            var item = data;
                            PointLatLng pointLatLng1 = new PointLatLng(0.0, 0.0);
                            double? nullable = item.Longtitude;
                            int num2;
                            if (!nullable.HasValue)
                            {
                                nullable = item.Latitude;
                                num2 = nullable.HasValue ? 1 : 0;
                            }
                            else
                                num2 = 1;
                            if (num2 == 0)
                            {
                                PointLatLng pointLatLng2 = new PointLatLng(0.0, 0.0)
                                {
                                    Lat = double.Parse(item.LatDegree) + double.Parse(item.LatMin) / 60.0 + double.Parse(item.LatS) / 3600.0,
                                    Lng = double.Parse(item.LonDegree) + double.Parse(item.LonM) / 60.0 + double.Parse(item.LonS) / 3600.0
                                };
                                DicShop dicShop = qDict.FirstOrDefault(x => x.Id.Equals(item.id));
                                pointLatLng1 = pointLatLng2;
                                if (dicShop != null)
                                {
                                    dicShop.Latitude = new double?(pointLatLng1.Lat);
                                    dicShop.Longtitude = new double?(pointLatLng1.Lng);
                                    distrdevDataContext.SubmitChanges();
                                }
                            }
                            else
                            {
                                PointLatLng local1 = @pointLatLng1;
                                nullable = item.Latitude;
                                double num3 = nullable.Value;
                                local1.Lat = num3;
                                PointLatLng local2 = @pointLatLng1;
                                nullable = item.Longtitude;
                                double num4 = nullable.Value;
                                local2.Lng = num4;
                            }
                            pointLatLngList.Add(pointLatLng1);
                            Task<DicShop> t = Task<DicShop>.Factory.StartNew(() => qDict.Where(items =>
                            string.Compare(items.LongitudeDegrees, item.LonDegree, false) == 0 & string.Compare(items.LongitudeMinutes, item.LonM, false) == 0
                            & items.LongitudeSeconds == item.LonS & items.LatitudeDegrees == item.LatDegree & string.Compare(items.LatitudeMinutes, item.LatMin, false) == 0 & items.LatitudeSeconds == item.LatS)
                            .FirstOrDefault());
                            t.Wait();
                            if (t.Result == null)
                            {
                                ++key;
                            }
                            else
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                string str = $"\aКод РТТ {t.Result.Code} \nПериод работы:{(object)t.Result.Schedule}\n";
                                stringBuilder.Append(str);
                                DicShop ds = distrdevDataContext.DicShops.Where(items => items.Code == t.Result.Code).FirstOrDefault();
                                if (ds != null)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    VPointAdress vpointAdress = distrdevDataContext.VPointAdress.Where(items => items.Id == ds.Id).FirstOrDefault();
                                    if (!ReferenceEquals(vpointAdress, null))
                                    {
                                        stringBuilder.Append(vpointAdress.Adress);
                                        List<VShopContractor> qqq = distrdevDataContext.VShopContractors.Where(x => x.ShopCode == (double?)t.Result.Code).ToList();
                                        Task.Factory.StartNew(() => qqq.ForEach(contractors =>
                                        {
                                            sb.Append(Environment.NewLine);
                                            sb.Append(contractors.ContractorName);
                                        })).Wait();
                                        stringBuilder.Append(sb.ToString());
                                    }
                                }
                                if (!dictionary2.TryGetValue(key, out double num3))
                                    dictionary2.Add(key, t.Result.Id);
                                dictionary1.Add(key, stringBuilder.ToString());
                                ++key;
                            }
                        }
                        pointsProperties.PointsOnMap = pointLatLngList.Count;
                        pointsProperties.DocumentPoints = JsonConvert.SerializeObject(dictionary2);
                        pointsProperties.HintCollection = JsonConvert.SerializeObject(dictionary1);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    result.Add(JsonConvert.SerializeObject(pointsProperties), pointLatLngList);
                }
                distrdevDataContext.Connection.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public string GetShopAdressInfo(double shopcode)
        {
            string str = string.Empty;
            try
            {
                DISTRDEVDataContext.DISTRDEVDataContext dataConnection = (DISTRDEVDataContext.DISTRDEVDataContext)this.CreateDataConnection();
                dataConnection.Connection.Open();
                VShopAddressTer vshopAddressTer = dataConnection.VShopAddressTers.FirstOrDefault<VShopAddressTer>((Expression<Func<VShopAddressTer, bool>>)(x => x.ShopCode.Equals((object)shopcode)));
                if (vshopAddressTer != null)
                    str = JsonConvert.SerializeObject(new List<string>()
          {
            string.Format("Код РТТ: {0}",  vshopAddressTer.ShopCode),
            OracleDataHelper.IsCoordinatesIsNull(vshopAddressTer).ToCoordinatesResult(),
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
            AgentPointsEvent beginLoadData = this.BeginLoadData;
            if (beginLoadData == null)
                return;
            beginLoadData(arg);
        }

        private void OnEndLoadData(AgentPointsArgument arg)
        {
            AgentPointsEvent endLoadData = this.EndLoadData;
            if (endLoadData == null)
                return;
            endLoadData(arg);
        }

        public void GetTaPoints(double positionId, DateTime dateBegin, DateTime dateEnd)
        {
            AgentPointsArgument agentPointsArgument = new AgentPointsArgument();
            try
            {
                using (DISTRDEVDataContext.DISTRDEVDataContext distrdevDataContext =
                    new DISTRDEVDataContext.DISTRDEVDataContext(System.Configuration.ConfigurationManager.ConnectionStrings["distributor"].ConnectionString))
                {
                    agentPointsArgument.Result = (object)distrdevDataContext.Connection.ConnectionString;
                    this.OnBeginLoadData(agentPointsArgument);
                    distrdevDataContext.Connection.Open();
                    List<double> list = distrdevDataContext.GetAgentPoint(new double?(positionId), new DateTime?(dateBegin), new DateTime?(dateEnd)).ToList<GetAgentPointResult>().Where<GetAgentPointResult>((Func<GetAgentPointResult, bool>)(item => item.Code.HasValue)).Select<GetAgentPointResult, double>((Func<GetAgentPointResult, double>)(item => item.Code.Value)).ToList<double>();
                    agentPointsArgument.Result = (object)list;
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
