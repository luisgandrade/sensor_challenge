using Challenge.Database.Interfaces;
using Challenge.Database.Repositories;
using Challenge.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.Services
{
    public class SensorRepositoryInterface
    {
        private static ConcurrentDictionary<Tuple<string, string, string>, Sensor> _sensorsCache;

        private readonly ISensorRepository _sensorRepository;

        protected SensorRepositoryInterface()
        {
            _sensorsCache = new ConcurrentDictionary<Tuple<string, string, string>, Sensor>();
        }

        public SensorRepositoryInterface(ISensorRepository sensorRepository) 
            : this()
        {
            _sensorRepository = sensorRepository;
        }        

        public virtual async Task<IList<Sensor>> GetAll()
        {
            var allSensors = await _sensorRepository.GetAllSensors();
            foreach (var sensor in allSensors)
                _sensorsCache.TryAdd(new Tuple<string, string, string>(sensor.Country, sensor.Region, sensor.Name), sensor);

            return allSensors;
        }

        public virtual async Task<Sensor> Get(int id)
        {
            var sensor = await _sensorRepository.Get(id);
            if(sensor != null)
                _sensorsCache.TryAdd(new Tuple<string, string, string>(sensor.Country, sensor.Region, sensor.Name), sensor);

            return sensor;
        }

        public virtual async Task<Sensor> Get(string country, string region, string name)
        {
            Sensor sensorFound = null;
            if (!_sensorsCache.TryGetValue(new Tuple<string, string, string>(country, region, name), out sensorFound)){
                sensorFound = await _sensorRepository.GetByTagParameters(country, region, name);
                if(sensorFound != null)
                    _sensorsCache.TryAdd(new Tuple<string, string, string>(country, region, name), sensorFound);
            }
                
            return sensorFound;
        }

        public virtual async Task<Sensor> Insert(string country, string region, string name)
        {
            var newSensor = new Sensor(country, region, name);
            await _sensorRepository.Insert(newSensor);
            _sensorsCache.TryAdd(new Tuple<string, string, string>(country, region, name), newSensor);
            return newSensor;
        }

        public virtual async Task<IList<KeyValuePair<Sensor, int>>> GetEventCountOfSensors() => await _sensorRepository.GetEventCountOfSensors();

    }
}
