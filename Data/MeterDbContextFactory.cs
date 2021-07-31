using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

using static Data.Globals;

namespace Data.Migrations
{
    public class MeterDBContextFactory : IDesignTimeDbContextFactory<MeterDBContext>
    {
        static readonly string SETTINGS_FILE = "migrationsettings.json";

        internal static IConfiguration Configuration; 

        public MeterDBContextFactory()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile(SETTINGS_FILE, optional: true);

            Configuration = builder.Build();
        }

        public MeterDBContext CreateDbContext(string[] args)
        {
            return new MeterDBContext(new DbContextOptionsBuilder<MeterDBContext>().UseSqlite($"Data Source={Configuration.GetValue(DB_FILE_KEY, DB_FILE_DEFAULT)};").Options);
        }
    }
}
