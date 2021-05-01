using Challenge.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public class SensorEventRepository : ISensorEventRepository
    {

        private readonly IDbConnection _dbConnection;

        public SensorEventRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IList<SensorEvent>> GetSuccessfullEventsWithNumericValue()
        {
            throw new NotImplementedException();
        }

        public async Task Insert(SensorEvent sensorEvent)
        {
            var idAssigned = await _dbConnection.InsertAsync(sensorEvent);
            if (idAssigned.HasValue)
                sensorEvent.GetType().GetProperty("Id").SetValue(sensorEvent, idAssigned);
        }
    }
}
