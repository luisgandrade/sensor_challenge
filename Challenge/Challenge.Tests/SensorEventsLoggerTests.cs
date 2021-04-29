using Challenge.Database.Repositories;
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

    }
}
