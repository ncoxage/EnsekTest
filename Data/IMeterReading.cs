using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMeterReading
    {
        string AccountId { get;  }
        string ReadAt { get;  }
        string ReadValue { get;  }

        bool IsValid(IMeterDBContext context);
        Task<bool> LoadReading(IMeterDBContext context);
    }
}
