using Challenge.Database.Repositories;
using Challenge.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Services
{
    public class SensorDictionarySingleton
    {
        private static SensorDictionarySingleton _instance;

        private ConcurrentDictionary<Tuple<string, string, string>, Sensor> _sensors;

        protected SensorDictionarySingleton()
        {

        }

        public static async Task<SensorDictionarySingleton> GetInstance(ISensorRepository sensorRepository)
        {
            if(_instance is null)
            {
                _instance = new SensorDictionarySingleton();
                var sensorsRegistered = await sensorRepository.GetAllSensors();
                _instance._sensors = new ConcurrentDictionary<Tuple<string, string, string>, Sensor>(
                    sensorsRegistered.Select(sensor => new KeyValuePair<Tuple<string, string, string>, Sensor>(new Tuple<string, string, string>(sensor.Country, sensor.Region, sensor.Name), sensor)));
            }
            return _instance;
        }


        public virtual Sensor GetSensor(string country, string region, string name)
        {
            Sensor sensorFound = null;
            _sensors.TryGetValue(new Tuple<string, string, string>(country, region, name), out sensorFound);
            return sensorFound;
        }

        public virtual void AddSensor(Sensor sensor) => _sensors.TryAdd(new Tuple<string, string, string>(sensor.Country, sensor.Region, sensor.Name), sensor);
    }
}
