using System.Threading.Tasks;
using CacheServices.Services.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace CacheServicesTests
{
    public class HelpersTest
    {       
        [Theory(DisplayName = "String extention - remove escape characters and quotes first/last symbol")]
        [InlineData("\"jhsvfrjhdfhj\\\\\\ighsihg\"")]
        public void Remove(string @string)
        {
            @string.Should().Contain(@"\");
            @string.Should().Contain(@"""");

            @string = @string.RemoveEscapeCharactersAndQuotes();
            
            @string.Should().NotContain(@"\");
            @string.Should().NotContain(@"""");
        }

        [Theory(DisplayName = "GetResponse")]
        [InlineData("testResult", "test", "test", "test")]
        [InlineData("testResult", null, "test", "test")]
        [InlineData("testResult", "test", null, "test")]
        [InlineData("testResult", "test", "test", null)]
        [InlineData("testResult", null, null, null)]
        public async void GetResponse(string returnValue, string logIn, string logOut, string kindData)
        {
            var controller = new CacheControllerBase();
            var meth = Task.Run(() => returnValue);
            ActionResult result;
            
            
            if (string.IsNullOrEmpty(logIn) & string.IsNullOrEmpty(logOut) & string.IsNullOrEmpty(kindData))
                result = await controller.GetResponse(async () => await meth);
            else
                result = await controller.GetResponse(async () => await meth, logIn, logOut, kindData);
            
            result.Should().BeOfType<OkObjectResult>();
            var value = ((OkObjectResult) result).Value;
            value.Should().Be(returnValue);
        }
    }
}