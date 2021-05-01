using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.Database.Repositories
{
    public interface ISensorRepository
    {

        Task<Sensor> Get(int id);
        Task<Sensor> Insert(Sensor sensor);

        Task<Sensor> GetByTagParameters(string country, string region, string name);


        Task<IList<Sensor>> GetAllSensors();

        Task<IList<KeyValuePair<Sensor, int>>> GetEventCountOfSensors();

    }
}
