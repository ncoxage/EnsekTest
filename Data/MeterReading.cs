using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class MeterReading 
    {
        public string AccountId { get; set; }
        public string ReadAt { get; set; }
        public string ReadValue { get; set; }

    }
}
