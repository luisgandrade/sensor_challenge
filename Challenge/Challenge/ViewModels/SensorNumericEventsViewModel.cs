using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.ViewModels
{
    public class SensorNumericEventsViewModel
    {
        public int SensorId { get; set; }

        public string SensorTag { get; set; }

        public IList<ChartEventDataViewModel> Data { get; set; }

    }
}
