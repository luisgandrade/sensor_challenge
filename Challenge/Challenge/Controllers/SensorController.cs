using Challenge.Database.Interfaces;
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
        private readonly ISensorEventRepository _sensorEventRepository;

        public SensorController(SensorRepositoryInterface sensorRepositoryInterface, ISensorEventRepository sensorEventRepository)
        {
            _sensorRepositoryInterface = sensorRepositoryInterface;
            _sensorEventRepository = sensorEventRepository;
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
            var existingSensor = await _sensorRepositoryInterface.Get(viewModel.Country, viewModel.Region, viewModel.Name);
            if (existingSensor != null)
                return BadRequest("Tag já associada a um sensor");

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

        [HttpGet("numeric-events-data")]
        public async Task<IActionResult> GetDataOfNumericEvents()
        {
            var eventsWithNumericValue = await _sensorEventRepository.GetSuccessfullEventsWithNumericValue();
            var allSensors = await _sensorRepositoryInterface.GetAll();

            var vms = allSensors.GroupJoin(eventsWithNumericValue, sr => sr.Id, se => se.SensorId, (s, se) => new SensorNumericEventsViewModel
            {
                SensorId = s.Id,
                SensorTag = s.Country + "." + s.Region + "." + s.Name,
                Data = se.Select(sev => new ChartEventDataViewModel { Timestamp = sev.Timestamp, Value = double.Parse(sev.Value) }).ToList()
            }).Where(sne => sne.Data.Any());

            return Ok(vms);
        }

    }
}
