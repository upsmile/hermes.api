using CacheServices.Controllers;
using CacheServices.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CacheServicesTests
{
    public class InfoControllerTest
    {
        private InfoController _controller;

        public InfoControllerTest()
        {
              _controller = new InfoController();  
        }
        
        [Fact(DisplayName = "Get app version")]
        public async void GetVersion()
        {
            var result = await _controller.GetVersion();
            
            result.Should().BeOfType<OkObjectResult>();
            var value = ((OkObjectResult) result).Value;
            value.Should().BeOfType<string>();
        }
    }
}