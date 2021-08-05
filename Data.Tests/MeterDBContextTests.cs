using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using FluentAssertions;
using Xunit;

using Data.Tests.Builders;

namespace Data.Tests
{
    public class MeterDBContextTests
    {
        const string CONFIG_MSG = "Configure called";
        class DummySet : IEntityTypeConfiguration<DummySet>
        {
#pragma warning disable IDE0060 // Remove unused parameter
            public void Configure(EntityTypeBuilder<DummySet> builder)
#pragma warning restore IDE0060 // Remove unused parameter
            {
                throw new ApplicationException(CONFIG_MSG);
            }
        }

        class TestContext : MeterDBContext
        {
            public TestContext(DbContextOptions dbOptions) : base(dbOptions)
            {

            }

            public DbSet<DummySet> Dummy { get => this.Set<DummySet>(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///   NEED TO TEST multiple sets !!!
        /// </remarks>
        [Fact]
        public void CallsConfigureForDbSetProperty()
        {
            using (var context = new TestContext(new DbContextOptionsBuilder<TestContext>().UseSqlite(MeterDBContextBuilder.CreateInMemoryDatabase()).Options))
            {
                Action test = () => context.Database.EnsureCreated();

                test.Should().Throw<Exception>().WithInnerException<ApplicationException>().WithMessage(CONFIG_MSG);
            }
        }

    }
}
