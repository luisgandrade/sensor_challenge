using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Challenge.Database.Interfaces;
using Dapper.Contrib.Extensions;

namespace Challenge.Database.Repositories
{
    public class SensorRepository : BaseRepository, ISensorRepository
    {
        public SensorRepository(IDbConnection dbConnection)
            : base(dbConnection) { }

        public async Task<Sensor> Get(int id)
        {
            return await _dbConnection.GetAsync<Sensor>(id);
        }

        public async Task<IList<Sensor>> GetAllSensors()
        {
            return (await _dbConnection.QueryAsync<Sensor>("SELECT * FROM Sensor")).ToList();
        }

        public async Task<Sensor> GetByTagParameters(string country, string region, string name)
        {
            return await _dbConnection.QuerySingleOrDefaultAsync<Sensor>(@"
                SELECT * FROM Sensor
                WHERE country = @Country AND region = @Region AND name = @Name", new { Country = country, Region = region, Name = name });
        }

        public async Task<IList<KeyValuePair<Sensor, int>>> GetEventCountOfSensors()
        {

            return (await _dbConnection.QueryAsync<Sensor, int, KeyValuePair<Sensor, int>>(@"
                SELECT s.id, s.country, s.region, s.name, COUNT(se.id) FROM Sensor s
                INNER JOIN SensorEvent se on s.id = se.sensorId
                GROUP BY s.id, s.country, s.region, s.name", (sensor, count) => new KeyValuePair<Sensor, int>(sensor, count))).ToList();
        }

        public async Task Insert(Sensor sensor)
        {
            var idAssigned = await _dbConnection.InsertAsync(sensor);
            
            sensor.GetType().GetProperty("Id").SetValue(sensor, idAssigned);
        }
    }
}
