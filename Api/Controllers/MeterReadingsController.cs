using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using CsvHelper;

using Data;

using System.IO;
using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingsController
    {
        IMeterDBContext DbContext { get; }
        IReadingMaker ReadingMaker { get; }
        ILogger Logger;

        public MeterReadingsController(IMeterDBContext dbContext, IReadingMaker readingMaker, ILogger<MeterReadingsController> logger)
        {
            DbContext = dbContext;
            ReadingMaker = readingMaker;
            Logger = logger;
        }

        [HttpPost]
        public async Task<IReadingsLoadResults> Post(IFormFile file)
        {
            return await this.ReadingMaker.LoadReadings(file, DbContext);
        }

    }
}
