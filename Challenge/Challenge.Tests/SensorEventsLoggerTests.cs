using Challenge.Database.Interfaces;
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

            var sensorEventsLogger = new SensorEventsLogger(Mock.Of<SensorRepositoryInterface>(), Mock.Of<ISensorEventRepository>());

            await Assert.ThrowsAsync<FormatException>(async() => await sensorEventsLogger.LogEvent(viewModel));
        }

        [Theory]
        [InlineData("brasil.sudeste.")]
        [InlineData("brasil..sensor1")]
        [InlineData(".sudeste.sensor1")]
        public async Task ShouldFailIfAnyOfTheThreePartsOfTheTagIsEmpty(string tag)
        {

            var viewModel = Mock.Of<AddSensorEventViewModel>(vm => vm.Tag == tag);

            var sensorEventsLogger = new SensorEventsLogger(Mock.Of<SensorRepositoryInterface>(), Mock.Of<ISensorEventRepository>());

            await Assert .ThrowsAsync<FormatException>(async () => await sensorEventsLogger.LogEvent(viewModel));
        }

        [Fact]
        public async Task ShouldUseExistingSensorIfThereIsAnIdMatch()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var sensorId = 1;
            var existingSensor = Mock.Of<Sensor>(sensor => sensor.Id == sensorId);
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = (int)DateTime.Now.Ticks,
                Tag = $"{country}.{region}.{name}",
                Value = "1"
            };
            var sensorRepositoryInterface = Mock.Of<SensorRepositoryInterface>(sri => sri.Get(country, region, name) == Task.FromResult(existingSensor));
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorRepositoryInterface, sensorEventRepository.Object);

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            Assert.Equal(sensorId, sensorEvent.SensorId);
        }

        [Fact]
        public async Task ShouldCreateAndAddNewSensorIfThereIsNotAnIdMatch()
        {
            var country = "brasil";
            var region = "sudeste";
            var name = "sensor1";
            var viewModel = new AddSensorEventViewModel
            {
                Timestamp = (int)DateTime.Now.Ticks,
                Tag = $"{country}.{region}.{name}",
                Value = "1"
            };
            var sensorDictionarySingletonMock = new Mock<SensorRepositoryInterface>();
            sensorDictionarySingletonMock.Setup(sds => sds.Get(country, region, name)).ReturnsAsync((Sensor) null);
            
            var sensorEventsLogger = new SensorEventsLogger(sensorDictionarySingletonMock.Object, Mock.Of<ISensorEventRepository>());

            await Assert.ThrowsAsync<Exception>(async () => await sensorEventsLogger.LogEvent(viewModel));

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
                Timestamp = (int)DateTime.Now.Ticks,
                Tag = $"{country}.{region}.{name}",
                Value = ""
            };
            var sensorRepositoryInterface = Mock.Of<SensorRepositoryInterface>(sds => sds.Get(country, region, name) == Task.FromResult(existingSensor));
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorRepositoryInterface, sensorEventRepository.Object);

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
                Timestamp = (int)DateTime.Now.Ticks,
                Tag = $"{country}.{region}.{name}",
                Value = value
            };
            var sensorRepositoryInterface = Mock.Of<SensorRepositoryInterface>(sri => sri.Get(country, region, name) == Task.FromResult(existingSensor));
            var sensorEventRepository = new Mock<ISensorEventRepository>();

            var sensorEventsLogger = new SensorEventsLogger(sensorRepositoryInterface, sensorEventRepository.Object);

            var sensorEvent = await sensorEventsLogger.LogEvent(viewModel);

            Assert.Equal(valueType, sensorEvent.ValueType);
        }

    }
}
