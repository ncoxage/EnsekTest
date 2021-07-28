using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;


using Microsoft.EntityFrameworkCore;


using FluentAssertions;
using Xunit;

using Data.Tests.Builders;

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
            using (var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseSqlite(MeterDBContextBuilder.CreateInMemoryDatabase()).Options))
            {
                Action test = () => context.Database.EnsureCreated();

                test.Should().Throw<Exception>().WithInnerException<ApplicationException>().WithMessage(CONFIG_MSG);
            }
        }

    }
}
