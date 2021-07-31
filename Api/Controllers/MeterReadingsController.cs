using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Data;

using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    [ApiController]
    [Route("meter-reading-uploads")]
    public class MeterReadingsController
    {
        IMeterDBContext DbContext { get; }
        IReadingsLoader ReadingMaker { get; }
        ILogger Logger { get; }

        public MeterReadingsController(IMeterDBContext dbContext, IReadingsLoader readingMaker, ILogger<MeterReadingsController> logger)
        {
            DbContext = dbContext;
            ReadingMaker = readingMaker;
            Logger = logger;
        }

        [HttpPost]
        public async Task<IReadingsLoadResults> Post(IFormFile file)
        {
            try
            {
                return await this.ReadingMaker.LoadToDb(file, DbContext);
            }
            catch(Exception e)
            {
                Logger.LogError($"{this.GetType().Name}.{nameof(Post)} - Error loading meter readings\n{e.Message}");
                throw;
            }
        }

    }
}
