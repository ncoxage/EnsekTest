using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    interface IReadingsLoadResults
    {
        public List<IMeterReading> Loaded { get; } 
        public List<IMeterReading> Rejected { get; }
    }
}
