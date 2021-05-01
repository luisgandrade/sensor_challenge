using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Models
{
    public class Sensor
    {
        public int Id { get; private set; }

        public string Country { get; private set; }

        public string Region { get; private set; }

        public string Name { get; private set; }

        public Sensor(string country, string region, string name)
        {
            if (string.IsNullOrWhiteSpace(country))
                throw new ArgumentNullException(nameof(country));
            if (string.IsNullOrWhiteSpace(region))
                throw new ArgumentNullException(nameof(region));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Country = country;
            Region = region;
            Name = name;
        }

        protected Sensor()
        {

        }
    }
}
