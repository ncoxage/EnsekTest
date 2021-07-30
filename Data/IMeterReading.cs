using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMeterReading
    {
        string AccountId { get; set; }
        string ReadAt { get; set;  }
        string ReadValue { get; set; }

        bool IsValid(IMeterDBContext context);
        Task<bool> LoadReading(IMeterDBContext context);
    }
}
