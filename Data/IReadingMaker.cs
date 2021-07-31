using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Data
{
    public interface IReadingMaker
    {
        Task<IReadingsLoadResults> LoadReadings(IFormFile file, IMeterDBContext dbContext);
    }
}
