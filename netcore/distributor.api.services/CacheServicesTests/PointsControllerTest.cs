using System;
using System.Collections;
using System.Collections.Concurrent;
using CacheServices.Controllers;
using CacheServices.Models;
using CacheServices.Services;
using CacheServices.Services.Helpers;
using CacheServices.Services.Points;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace CacheServicesTests
{
    public class PointsControllerTest
    {
        private readonly PointsController _controller;
        
        public PointsControllerTest()
        {
            var configuration = new Mock<IConfiguration>(); 
            var mockService = new Mock<IServiceProvider>();
            configuration.SetupGet(x => x["CacheServices:PointsConnectionstring"]).Returns("http://93.157.14.7:8086/distributor.api/DistributorApiService.svc/api/03/3a8b8165-5b3a-48d2-be16-fd44294b34ca"); 
            
            var service = new PointsCacheService(new PointsService(configuration.Object, new CacheHelper(mockService.Object)), configuration.Object);
            _controller = new PointsController(service);
        }
        
        [Fact(DisplayName = "Get all RTT (TD) points")]
        public async void GetAll()
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetAllPoints();
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<ConcurrentBag<Point>>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
        
        [Theory(DisplayName = "Get one RTT (TD)")]
        [InlineData(2)]
        public async void GetOne(long code)
        {
            Exception ex = null;
            try
            {
                var result = await _controller.GetPoint(code);
            
                result.Should().BeOfType<OkObjectResult>();
                var value = ((OkObjectResult) result).Value;
                value.Should().BeOfType<Point>();
            }
            catch (Exception e)
            {
                ex = e;
            }

            ex.Should().BeNull();
        }
    }
}
