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
        private SensorDictionarySingleton _sensorDictionarySingleton;
        private ISensorRepository _sensorRepository;
        private ISensorEventRepository _sensorEventRepository;

        public SensorEventsLogger(SensorDictionarySingleton sensorDictionarySingleton, ISensorRepository sensorRepository, ISensorEventRepository sensorEventRepository)
        {
            _sensorDictionarySingleton = sensorDictionarySingleton;
            _sensorRepository = sensorRepository;
            _sensorEventRepository = sensorEventRepository;
        }

        public async Task<SensorEvent> LogEvent(AddSensorEventViewModel viewModel)
        {
            if (viewModel is null)
                throw new ArgumentNullException(nameof(viewModel));

            var sensorTagSplit = viewModel.Tag.Split(".").ToList();
            if (sensorTagSplit.Count != 3)
                throw new ArgumentOutOfRangeException("Expected three parts in the tag.");

            var sensor = _sensorDictionarySingleton.GetSensor(sensorTagSplit[0], sensorTagSplit[1], sensorTagSplit[2]);
            if(sensor is null)
            {
                sensor = new Sensor(sensorTagSplit[0], sensorTagSplit[1], sensorTagSplit[2]);
                await _sensorRepository.Insert(sensor);
                _sensorDictionarySingleton.AddSensor(sensor);
            }

            var isError = string.IsNullOrWhiteSpace(viewModel.Value);
            var valueType = EventValueType.NotApplicable;
            if (double.TryParse(viewModel.Value, out _))
                valueType = EventValueType.Numeric;
            else if (!isError)
                valueType = EventValueType.String;

            var newSensorEvent = new SensorEvent(sensor, viewModel.Timestamp, isError, viewModel.Value, valueType);
            await _sensorEventRepository.Insert(newSensorEvent);

            return newSensorEvent;
        }
    }
}
