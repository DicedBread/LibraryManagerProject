using library_manager_server;
using library_manager_server.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace library_manager_server_tests
{
    public class Tests
    {
        ILogger<WeatherForecastController> logger;

        [SetUp]
        public void Setup()
        {
            List<string> list = new List<string>();
            
            ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            ILogger<WeatherForecastController> logger = factory.CreateLogger<WeatherForecastController>();
        }

        [Test]
        public void Test1()
        {
            WeatherForecastController fc = new WeatherForecastController(logger);
             //var vSMock = new Mock<WeatherForecastController>();
            WeatherForecast res = fc.Get(2);
            Assert.AreEqual(res.TemperatureC, 2);
        }
    }
}