using Challenge.Database.Interfaces;
using Challenge.Models;
using Challenge.Services;
using Challenge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Challenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorEventController : ControllerBase
    {
        private readonly DateTime LINUX_BASE_TIMESTAMP = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        private readonly ISensorEventRepository _sensorEventRepository;
        private readonly SensorEventsLogger _sensorEventsLogger;
        private readonly SensorRepositoryInterface _sensorRepositoryInterface;

        public SensorEventController(ISensorEventRepository sensorEventRepository, SensorEventsLogger sensorEventsLogger, SensorRepositoryInterface sensorRepositoryInterface)
        {
            _sensorEventRepository = sensorEventRepository;
            _sensorEventsLogger = sensorEventsLogger;
            _sensorRepositoryInterface = sensorRepositoryInterface;
        }



        // GET api/<SensorEventController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _sensorEventRepository.Get(id));
        }

        // POST api/<SensorEventController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddSensorEventViewModel viewModel)
        {
            var newSensorEvent = await _sensorEventsLogger.LogEvent(viewModel);

            return CreatedAtAction(nameof(Get), new { id = newSensorEvent.Id });
        }

        [HttpGet("all-events/{from?}")]
        public async Task<IActionResult> GetAllEventsFromTimestamp([FromQuery] DateTime? from)
        {
            from ??= LINUX_BASE_TIMESTAMP;

            var events = await _sensorEventRepository.GetEventsAfterTimestamp(from.Value);
            var sensors = await _sensorRepositoryInterface.GetAll();

            var vms = events.Join(sensors, ev => ev.SensorId, sr => sr.Id, (ev, sr) => new ShowEventViewModel
            {
                Country = sr.Country,
                Region = sr.Region,
                Name = sr.Name,
                Timestamp = ev.Timestamp,
                IsError = ev.Error,
                Value = ev.Value
            });

            return Ok(vms);
        }
    }
}
