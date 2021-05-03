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

        private readonly ISensorEventRepository _sensorEventRepository;
        private readonly SensorEventsLogger _sensorEventsLogger;

        public SensorEventController(ISensorEventRepository sensorEventRepository, SensorEventsLogger sensorEventsLogger)
        {
            _sensorEventRepository = sensorEventRepository;
            _sensorEventsLogger = sensorEventsLogger;
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
    }
}
