using System;

namespace Challenge.Models
{
    public class SensorEvent
    {

        public int Id { get; private set; }

        public Sensor Sensor { get; set; }

        public DateTime Timestamp { get; private set; }

        public EventStatus Status { get; private set; }

        public string Value { get; private set; }

        public SensorEvent(Sensor sensor, DateTime timestamp, EventStatus status, string value)
        {
            if (sensor is null)
                throw new ArgumentNullException(nameof(sensor));
            
            Timestamp = timestamp;
            Status = status;
            Value = value;
        }
    }
}
