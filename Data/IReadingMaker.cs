using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IReadingMaker
    {
        IMeterReading NewReading(string accountId, string readAt, string value);
    }
}
