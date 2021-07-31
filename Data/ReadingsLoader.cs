using System.Globalization;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using CsvHelper;
using CsvHelper.Configuration;


namespace Data
{
    public class LoadResults : IReadingsLoadResults
    {
        public int Loaded { get; internal set; }
        public int Rejected { get; internal set; }

    }

    public class ReadingsLoader : IReadingsLoader
    {
        public async Task<IReadingsLoadResults> LoadToDb(IFormFile file, IMeterDBContext dbContext)
        {
            var result = new LoadResults();
            
            using (var reader = new CsvReader(new StreamReader(file.OpenReadStream()), CultureInfo.InvariantCulture))
            {
                reader.Context.RegisterClassMap<ReadingMap>();
                var list = reader.GetRecords<MeterReading>().GetEnumerator();
 
                while (list.MoveNext())
                {
                    if (await list.Current.LoadReading(dbContext)) result.Loaded++; else result.Rejected++;
                }
            }

            return result;
        }
    }
    public sealed class ReadingMap : ClassMap<MeterReading>
    {
        public ReadingMap()
        {
            Map(m => m.AccountId);
            Map(m => m.ReadAt).Name("MeterReadingDateTime");
            Map(m => m.ReadValue).Name("MeterReadValue");
        }
    }
}
