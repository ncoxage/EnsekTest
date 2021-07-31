using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Data
{
    public interface IReadingsLoader
    {
        Task<IReadingsLoadResults> LoadToDb(IFormFile file, IMeterDBContext dbContext);
    }
}
