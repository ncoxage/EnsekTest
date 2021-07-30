using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IReadingsLoadResults
    {
        public int Loaded { get; } 
        public int Rejected { get; }
    }
}
