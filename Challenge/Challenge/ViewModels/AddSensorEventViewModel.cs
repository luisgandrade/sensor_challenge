using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.ViewModels
{
    public class AddSensorEventViewModel
    {

        public virtual DateTime Timestamp { get; set; }

        public virtual string Tag { get; set; }

        public virtual string Value { get; set; }

    }
}
