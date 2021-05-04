using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.ViewModels
{
    public class ShowEventViewModel
    {
        public string Country { get; set; }

        public string Region { get; set; }

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsError { get; set; }

        public string Value { get; set; }

    }
}
