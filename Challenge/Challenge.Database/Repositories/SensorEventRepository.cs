using Challenge.Database.Interfaces;
using Challenge.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public class SensorEventRepository : BaseRepository, ISensorEventRepository
    {
        public SensorEventRepository(IDbConnection dbConnection)
            : base(dbConnection)
        { }

        public async Task<IList<SensorEvent>> GetSuccessfullEventsWithNumericValue()
        {
            //return (await _dbConnection.GetListAsync<SensorEvent>(new { ValueType = EventValueType.Numeric })).AsList();
            throw new NotImplementedException();
        }

        public async Task Insert(SensorEvent sensorEvent)
        {
            var idAssigned = await _dbConnection.InsertAsync(sensorEvent);

            sensorEvent.GetType().GetProperty("Id").SetValue(sensorEvent, idAssigned);

        }
    }
}
