using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Challenge.ViewModels
{
    public class AddSensorViewModel
    {
        [Required]
        public string Country { get; set; }

        [Required]
        public string Region { get; set; }

        [Required]
        public string Name { get; set; }


    }
}
