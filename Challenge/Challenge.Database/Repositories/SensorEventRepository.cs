using Challenge.Database.Interfaces;
using Challenge.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public class SensorEventRepository : BaseRepository, ISensorEventRepository
    {
        public SensorEventRepository(IDbConnection dbConnection)
            : base(dbConnection)
        { }


        public async Task<SensorEvent> Get(int id)
        {
            return await _dbConnection.GetAsync<SensorEvent>(id);
        }

        public async Task<IList<SensorEvent>> GetSuccessfullEventsWithNumericValue()
        {
            return (await _dbConnection.QueryAsync<SensorEvent>("SELECT * FROM SensorEvent WHERE valueType = @ValueType", new { ValueType = EventValueType.Numeric })).ToList();
        }

        public async Task Insert(SensorEvent sensorEvent)
        {
            var idAssigned = await _dbConnection.InsertAsync(sensorEvent);

            sensorEvent.GetType().GetProperty("Id").SetValue(sensorEvent, idAssigned);

        }
    }
}
