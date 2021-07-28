using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Model
{
    public class ReadingModel
    {
        public int AccountId { get; private set; }
        public DateTime ReadAt { get; private set; }
        public int Value { get; private set; }

        public ReadingModel( int accountId, DateTime time, int value)
        {
            AccountId = accountId;
            ReadAt = time;
            Value = value;
        }
    }
}
