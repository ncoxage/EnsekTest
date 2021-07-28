using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;


using FluentAssertions;
using Xunit;

namespace Data.Tests
{
    public class MeterDBContextTests
    {
        const string CONFIG_MSG = "Configure called";
        class DummySet
        {
            public static void Configure(ModelBuilder builder)
            {
                throw new ApplicationException(CONFIG_MSG);
            }
        }

        class TestContext : MeterDBContext
        {
            public TestContext(DbContextOptions dbOptions) : base(dbOptions)
            {

            }

            public DbSet<DummySet> dummy { get => this.Set<DummySet>(); }
        }

        [Fact]
        public void CallsConfigureForDbSetProperties()
        {
            using (var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseSqlite(CreateInMemoryDatabase()).Options))
            {
                Action test = () => context.Database.EnsureCreated();

                test.Should().Throw<Exception>().WithInnerException<ApplicationException>().WithMessage(CONFIG_MSG);
            }
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();
            return connection;
        }

    }
}
