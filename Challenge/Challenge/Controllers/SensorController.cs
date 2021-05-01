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
    public class SensorController : ControllerBase
    {
        private readonly SensorRepositoryInterface _sensorRepositoryInterface;

        public SensorController(SensorRepositoryInterface sensorRepositoryInterface)
        {
            _sensorRepositoryInterface = sensorRepositoryInterface;
        }

        // GET: api/<SensorController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var allSensors = await _sensorRepositoryInterface.GetAll();
            var showSensorVms = allSensors.Select(asr => new ShowSensorViewModel
            {
                Country = asr.Country,
                Region = asr.Region,
                Name = asr.Name
            });
            return Ok(showSensorVms);
        }

        // GET api/<SensorController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var sensor = await _sensorRepositoryInterface.Get(id);
            if (sensor is null)
                return NotFound();

            return Ok(sensor);
        }

        // POST api/<SensorController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddSensorViewModel viewModel)
        {
            var newSensor = await _sensorRepositoryInterface.Insert(viewModel.Country, viewModel.Region, viewModel.Name);
            return CreatedAtAction(nameof(Get), new { id = newSensor.Id });
        }


        [HttpGet("event-count")]
        public async Task<IActionResult> GetEventCount()
        {

            var eventCountsBySensor = await _sensorRepositoryInterface.GetEventCountOfSensors();

            var eventCountByTagVMs = eventCountsBySensor.Select(ecs => new EventCountByTagViewModel
            {
                Tag = $"{ecs.Key.Country}.{ecs.Key.Region}.{ecs.Key.Name}",
                Count = ecs.Value
            }).Concat(eventCountsBySensor
                .GroupBy(ecs => new { ecs.Key.Country, ecs.Key.Region })
                .Select(gecs => new EventCountByTagViewModel
                {
                    Tag = $"{gecs.Key.Country}.{gecs.Key.Region}",
                    Count = gecs.Sum(ecs => ecs.Value)
                }));

            return Ok(eventCountByTagVMs);
        }

    }
}
