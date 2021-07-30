using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    public struct LoadReturn
    {
        public int Loaded { get; }
        public int Rejected { get; }

        public LoadReturn(int loaded, int rejected)
        {
            Loaded = loaded;
            Rejected = rejected;
        }
    }
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingsController
    {
        [HttpPost]
        public async Task<LoadReturn> Post(string readings)
        {
            return new LoadReturn(0, 0);
        }
    }
}
