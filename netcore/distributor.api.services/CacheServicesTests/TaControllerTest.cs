using System;
using System.Collections.Concurrent;
using CacheServices.Controllers;
using CacheServices.Models;
using CacheServices.Services.Helpers;
using CacheServices.Services.Points;
using CacheServices.Services.TA;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CacheServicesTests
{
    public class TaControllerTest
    {
        private readonly TaController _controller;
        
        public TaControllerTest()
        {
            var configuration = new Mock<IConfiguration>(); 
            var mockService = new Mock<IServiceProvider>();
            configuration.SetupGet(x => x["CacheServices:TAConectionString"]).Returns("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/t/02"); 
            
            var service = new TaCacheService(new TAService(configuration.Object, new CacheHelper(mockService.Object)), configuration.Object);
            _controller = new TaController(service);
        }
        
        [Fact(DisplayName = "Get all TA")]
        public async void GetAllTA()
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetAllTa();
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<ConcurrentBag<TA>>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
        
        [Theory(DisplayName = "Get one TA")]
        [InlineData(41870011)]
        public async void GetOneTA(long code)
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetTa(code);
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<TA>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
    }
}