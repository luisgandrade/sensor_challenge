using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public interface ISensorEventRepository
    {

        Task Insert(SensorEvent sensorEvent);

        Task<IList<SensorEvent>> GetSuccessfullEventsWithNumericValue();
    }
}
