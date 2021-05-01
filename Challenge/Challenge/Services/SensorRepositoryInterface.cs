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

        private KeyValuePair<Tuple<string, string, string>, Sensor> CreateDictionaryEntry(Sensor sensor) =>
            new KeyValuePair<Tuple<string, string, string>, Sensor>(
                new Tuple<string, string, string>(sensor.Country, sensor.Region, sensor.Name),
                sensor);


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

    }
}
