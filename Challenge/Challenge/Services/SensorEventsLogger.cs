using Challenge.Database.Interfaces;
using Challenge.Database.Repositories;
using Challenge.Models;
using Challenge.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Services
{
    public class SensorEventsLogger
    {
        private SensorRepositoryInterface _sensorRespositoryInterface;
        private ISensorEventRepository _sensorEventRepository;

        public SensorEventsLogger(SensorRepositoryInterface sensorRespositoryInterface, ISensorEventRepository sensorEventRepository)
        {
            _sensorRespositoryInterface = sensorRespositoryInterface;
            _sensorEventRepository = sensorEventRepository;
        }

        private DateTime UnixTimestampToLocalDatetime(int unixTimestamp)
        {
            var offset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            return offset.LocalDateTime;
        }

        public async Task<SensorEvent> LogEvent(AddSensorEventViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var sensorTagSplit = viewModel.Tag.Split(".").ToList();
            if (sensorTagSplit.Count != 3)
                throw new FormatException("Expected three parts in the tag.");
            if (sensorTagSplit.Any(part => string.IsNullOrWhiteSpace(part)))
                throw new FormatException("Some part of the tag is empty");

            var sensor = await _sensorRespositoryInterface.Get(sensorTagSplit[0], sensorTagSplit[1], sensorTagSplit[2]);
            if (sensor is null)
                throw new Exception("Sensor not registered");

            var isError = string.IsNullOrWhiteSpace(viewModel.Value);
            var valueType = EventValueType.NotApplicable;
            if (double.TryParse(viewModel.Value, out _))
                valueType = EventValueType.Numeric;
            else if (!isError)
                valueType = EventValueType.String;

            var newSensorEvent = new SensorEvent(sensor, UnixTimestampToLocalDatetime(viewModel.Timestamp), isError, viewModel.Value, valueType);
            await _sensorEventRepository.Insert(newSensorEvent);

            return newSensorEvent;
        }
    }
}
