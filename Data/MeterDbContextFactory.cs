using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Data
{
    public class MeterDBContextFactory : IDesignTimeDbContextFactory<MeterDBContext>
    {
        public MeterDBContextFactory()
        {
        }

        public MeterDBContext CreateDbContext(string[] args)
        {
            return new MeterDBContext(new DbContextOptionsBuilder<MeterDBContext>().UseSqlite("Data Source=../MeterReadings.db;").Options);
        }
    }
}
