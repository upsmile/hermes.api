using Xunit;
using FluentAssertions;
using Hermes.Way.Api.Services;
using System;
using System.Globalization;

namespace Hermes.Way.Api.Test
{

    public class OracleDataHelperIntegratedTest
    {
        [Theory(DisplayName ="get shop adress info test")]
        [InlineData(2378)]
        public void GetShopAdressInfo(double code)
        {
            var helper = new OracleDataHelper();
            var info = OracleDataHelper.GetShopAdressInfo(code);
            info.Should().NotBeNullOrEmpty();
        }

        [Theory(DisplayName = "get transport way test")]
        [InlineData("16.11.2018", 41803011)]
        public void GetTransportWayTestCase(string datestr, double carId)
        {
            var helper = new OracleDataHelper();
            var date = DateTime.Parse(datestr, new CultureInfo("ru-Ru"));
            var info = OracleDataHelper.GetDeliveryVehiclesPoint(date, carId);
            info.Should().NotBeNull();
        }

        [Theory(DisplayName = "get trade agents way points")]
        [InlineData("15.11.2018", 464382024)]
        public void GetAgentsWayTestCase(string datestr, double positionId)
        {
            var helper = new OracleDataHelper();
            var date = DateTime.Parse(datestr, new CultureInfo("ru-Ru"));
            helper.EndLoadData += argument =>
            {
                argument.Result.Should().NotBeNull();
                argument.Exception.Should().BeNull();
            };
            helper.GetTaPoints(positionId, date, date);
        }
    }

}
