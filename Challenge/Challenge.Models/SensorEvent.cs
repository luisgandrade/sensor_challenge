using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Challenge.Models
{
    [Table("SensorEvent")]
    public class SensorEvent
    {
        public int Id { get; private set; }

        public int SensorId { get; set; }

        public DateTime Timestamp { get; private set; }

        public bool Error { get; set; }

        public string Value { get; private set; }

        public EventValueType ValueType { get; set; }

        public SensorEvent(Sensor sensor, DateTime timestamp, bool error, string value, EventValueType valueType)
        {
            if (sensor is null)
                throw new ArgumentNullException(nameof(sensor));

            SensorId = sensor.Id;
            Timestamp = timestamp;
            Error = error;
            Value = value;
            ValueType = valueType;
        }
    }
}
