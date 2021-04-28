using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public interface ISensorRepository
    {

        Task Insert(Sensor sensor);

        Task<IList<Sensor>> GetAllSensors();

        Task<IList<KeyValuePair<Sensor, int>>> GetEventCountOfSensors(IList<Sensor> sensors);

    }
}
