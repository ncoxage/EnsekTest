using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ReadingMaker : IReadingMaker
    {
        public IMeterReading NewReading(string accountId, string readAt, string value)
        {
            return new MeterReading { AccountId = accountId, ReadAt = readAt, ReadValue = value };
        }
    }
}
