using Challenge.Database.Repositories;
using Challenge.Models;
using Challenge.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests
{
    public class SensorRepositoryInterfaceTests
    {



        [Fact]
        public async Task ShouldTryToGetSensorInRepositoryIfItIsNotCached()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";

            var sensorRepositoryMock = new Mock<ISensorRepository>();
            var sensorRepositoryInterface = new SensorRepositoryInterface(sensorRepositoryMock.Object);

            var sensor = await sensorRepositoryInterface.Get(country, region, name);

            sensorRepositoryMock.Verify(sr => sr.GetByTagParameters(country, region, name), Times.Once());
        }

        [Fact]
        public async Task ShouldAddSensorToCacheIfItIsNotCached()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";

            var sensorFromDb = Mock.Of<Sensor>();
            var sensorRepositoryMock = new Mock<ISensorRepository>();
            sensorRepositoryMock.Setup(sr => sr.GetByTagParameters(country, region, name)).ReturnsAsync(sensorFromDb);
            var sensorRepositoryInterface = new SensorRepositoryInterface(sensorRepositoryMock.Object);

            _ = await sensorRepositoryInterface.Get(country, region, name);
            var sensorCached = await sensorRepositoryInterface.Get(country, region, name);

            sensorRepositoryMock.Verify(sr => sr.GetByTagParameters(country, region, name), Times.Once());
        }

        [Fact]
        public async Task ShouldAddSensorInsertedToCache()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";

            var sensorRepositoryMock = new Mock<ISensorRepository>();            
            var sensorRepositoryInterface = new SensorRepositoryInterface(sensorRepositoryMock.Object);

            var sensorInserted = await sensorRepositoryInterface.Insert(country, region, name);
            var sensorCached = await sensorRepositoryInterface.Get(country, region, name);

            Assert.Equal(sensorInserted, sensorCached);
        }
    }
}
