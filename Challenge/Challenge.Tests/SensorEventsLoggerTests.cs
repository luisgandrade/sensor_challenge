using Challenge.Database.Repositories;
using Challenge.Models;
using Challenge.Services;
using Challenge.ViewModels;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests
{
    public class SensorEventsLoggerTests
    {
        [Theory]
        [InlineData("brasil.sudeste.sensor1.subsensor")]
        [InlineData("brasil.sudeste")]
        [InlineData("brasil")]
        public async Task ShouldFailIfTagHasNotExactlyThreeParts(string tag)
        {

            var viewModel = Mock.Of<AddSensorEventViewModel>(vm => vm.Tag == tag);

            var sensorEventsLogger = new SensorEventsLogger(Mock.Of<SensorDictionarySingleton>(), Mock.Of<ISensorRepository>(), Mock.Of<ISensorEventRepository>());

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async() => await sensorEventsLogger.LogEvent(viewModel));
        }

        [Theory]
        [InlineData("brasil.sudeste.")]
        [InlineData("brasil..sensor1")]
        [InlineData(".sudeste.sensor1")]
        public async Task ShouldFailIfAnyOfTheThreePartsOfTheTagIsEmpty(string tag)
        {

            var viewModel = Mock.Of<AddSensorEventViewModel>(vm => vm.Tag == tag);

            var sensorEventsLogger = new SensorEventsLogger(Mock.Of<SensorDictionarySingleton>(), Mock.Of<ISensorRepository>(), Mock.Of<ISensorEventRepository>());

            await Assert .ThrowsAsync<ArgumentNullException>(async () => await sensorEventsLogger.LogEvent(viewModel));
        }

        [Fact]
        public async Task ShouldUseExistingSensorIfThereIsAnIdMatch()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var existingSensor = Mock.Of<Sensor>();
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = DateTime.Now,
                Tag = $"{country}.{region}.{name}",
                Value = "1"
            };
            var sensorDictionarySingleton = Mock.Of<SensorDictionarySingleton>(sds => sds.GetSensor(country, region, name) == existingSensor);
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorDictionarySingleton, Mock.Of<ISensorRepository>(), sensorEventRepository.Object);

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            Assert.Equal(existingSensor, sensorEvent.Sensor);
        }

        [Fact]
        public async Task ShouldCreateAndAddNewSensorIfThereIsNotAnIdMatch()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = DateTime.Now,
                Tag = $"{country}.{region}.{name}",
                Value = "1"
            };
            var sensorDictionarySingletonMock = new Mock<SensorDictionarySingleton>();
            sensorDictionarySingletonMock.Setup(sds => sds.GetSensor(country, region, name)).Returns<Sensor>(null);
            sensorDictionarySingletonMock.Setup(sds => sds.AddSensor(It.IsAny<Sensor>())).Callback<Sensor>(_ => { });
            var sensorRepositoryMock = new Mock<ISensorRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorDictionarySingletonMock.Object, sensorRepositoryMock.Object, Mock.Of<ISensorEventRepository>());

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            sensorDictionarySingletonMock.Verify(sr => sr.AddSensor(It.Is<Sensor>(s => s.Country == country && s.Region == region && s.Name == name)), Times.Once());
            sensorRepositoryMock.Verify(sr => sr.Insert(It.Is<Sensor>(s => s.Country == country && s.Region == region && s.Name == name)), Times.Once());
        }

        [Fact]
        public async Task ShouldSetErrorFlagIfValueIsEmpty()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var existingSensor = Mock.Of<Sensor>();
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = DateTime.Now,
                Tag = $"{country}.{region}.{name}",
                Value = ""
            };
            var sensorDictionarySingleton = Mock.Of<SensorDictionarySingleton>(sds => sds.GetSensor(country, region, name) == existingSensor);
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorDictionarySingleton, Mock.Of<ISensorRepository>(), sensorEventRepository.Object);

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            Assert.True(sensorEvent.Error);
        }

        [Theory]
        [InlineData("", EventValueType.NotApplicable)]
        [InlineData("1", EventValueType.Numeric)]
        [InlineData("value", EventValueType.String)]
        public async Task ShouldAssignValueTypeAccordingly(string value, EventValueType valueType)
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var existingSensor = Mock.Of<Sensor>();
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = DateTime.Now,
                Tag = $"{country}.{region}.{name}",
                Value = value
            };
            var sensorDictionarySingleton = Mock.Of<SensorDictionarySingleton>(sds => sds.GetSensor(country, region, name) == existingSensor);
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorDictionarySingleton, Mock.Of<ISensorRepository>(), sensorEventRepository.Object);

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            Assert.Equal(valueType, sensorEvent.ValueType);
        }

    }
}
