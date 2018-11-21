using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace CacheServicesTests
{
    public class GetPointsTest
    {
        [Fact(DisplayName = "Deserialization data")]
        public void TestGetData()
        {
            /*var request = "http://193.242.159.206:8089/distributor.api/DistributorApiService.svc/api/03/3a8b8165-5b3a-48d2-be16-fd44294b34ca";
            var client = new HttpClient();
            var response = await client.GetAsync(request);
            string result;

            var response2 = await response.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(response2))
            {
                result = reader.ReadToEnd();
            }*/
            Exception ex = null;
            try
            {
                var points = File.ReadAllText("./Points.txt");

            
                var data = JsonConvert.DeserializeObject<PointsData>(points);
                var list = data.GetDeliveryPointsResult;

                var listPoints = JsonConvert.DeserializeObject<List<Point>>(list);

                listPoints.Should().NotBeNull();
            }
            catch (Exception exception)
            {
                ex = exception;
            }

            ex.Should().BeNull();
        }


        [Fact(DisplayName = "Serailization/Deserialization")]
        public void TestDeseralization()
        {
            var points =  new List<Point>
               {                  
                      new Point
                      {
                          Position = new Position
                          {
                              Lat = 1,
                              Lon = 1,
                              Ele = 1
                          },
                          Header = new Header
                          {
                              Code = 1,
                              Name = "test",
                              Branch = new Branch
                              {
                                 Name = "test",
                                  Id = Guid.NewGuid()
                              },
                              RequestString = "test",
                              Contractors = "test",
                              ResponceString = "test"
                          }
                      },
                      new Point
                       {
                           Position = new Position
                           {
                               Lat = 2,
                               Lon = 2,
                               Ele = 2
                           },
                           Header = new Header
                           {
                               Code = 2,
                               Name = "test2",
                               Branch = new Branch
                               {
                                   Name = "test2",
                                   Id = Guid.NewGuid()
                               },
                               RequestString = "test2",
                               Contractors = "test2",
                               ResponceString = "test2"
                           }
                       }                                
            };

            var json = JsonConvert.SerializeObject(points);

            json.Should().NotBeNullOrEmpty();

            Exception ex = null;
            
            try
            {
                var result= JsonConvert.DeserializeObject<List<Point>>(json);
            }
            catch (Exception exception)
            {
                ex = exception;
            }

            ex.Should().BeNull();
        }


        [Theory(DisplayName = "Get data from services")]
        [InlineData("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/03/3a8b8165-5b3a-48d2-be16-fd44294b34ca", false)]
        [InlineData("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/t/01", true)]
        [InlineData("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/t/02", true)]
        public async void GetDataFromService(string connectionString, bool addData)
        {
            var mockService = new Mock<IServiceProvider>();
            var helper = new CacheHelper(mockService.Object);
            if (addData)
            {
                var data = DateTime.Today.ToOADate();
                connectionString = $"{connectionString}/{data}";
            }

            var result = await helper.GetDataFromService(connectionString);
            result.Should().NotBeNull();
        }
    }
}