using System;
using System.Collections.Concurrent;
using CacheServices.Controllers;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using CacheServices.Services.Transports;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CacheServicesTests
{
    public class TransportsControllerTest
    {
        private readonly TransportsController _controller;
        
        public TransportsControllerTest()
        {
            var configuration = new Mock<IConfiguration>(); 
            var mockService = new Mock<IServiceProvider>();
            configuration.SetupGet(x => x["CacheServices:TransportConnectionString"]).Returns("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/t/01"); 
            
            var service = new TransportsCacheService(new TransportsService(configuration.Object, new CacheHelper(mockService.Object)), configuration.Object);
            _controller = new TransportsController(service);
        }
        
        [Fact(DisplayName = "Get all transports")]
        public async void GetAllTA()
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetAllTransports();
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<ConcurrentBag<Transport>>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
        
        [Theory(DisplayName = "Get one transport")]
        [InlineData(5253146020)]
        public async void GetOneTA(long code)
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetTransport(code);
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<Transport>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
    }
}