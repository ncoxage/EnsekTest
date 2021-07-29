using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using FluentAssertions;
using Xunit;

using Data;
using Data.Model;

using Data.Tests.Builders;


namespace Data.Tests
{
    public class ReadingModelTests
    {
        public static int SQLITE_CONSTRAINT = 19;

        [Fact]
        public void AccountId_NavigatesToAccount()
        {
            int seedId = 123;
            var seedAccount = new AccountModel(accountId: seedId);

            using (var builder = new MeterDBContextBuilder()
                                    .AccountSeeds(new List<AccountModel> { seedAccount }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().Should().Be(seedAccount); // confirm DB seeded as expected

                var sut = new ReadingModel(seedId, DateTime.UtcNow, 0);

                context.Readings.Add(sut);

                context.SaveChanges();

                sut.Account.Should().Be(seedAccount);
            }
        }

        [Fact]
        public void AccountId_ReadAt_IsUnique()
        {
            int seedId = 123;
            var readTime = DateTime.UtcNow;
            var seedAccount = new AccountModel(accountId: seedId);

            using (var builder = new MeterDBContextBuilder()
                                    .AccountSeeds(new List<AccountModel> { seedAccount })
                                    .ReadingSeeds(new List<ReadingModel> { new ReadingModel(accountId: seedId,
                                                                                            readAt: readTime,
                                                                                            value: 1) }))
            {
                var context = builder.Build();

                // confirm DB seeded as expected
                context.Accounts.FirstOrDefault().Should().Be(seedAccount);
                context.Readings.FirstOrDefault().Account.Should().Be(seedAccount);

                context.Readings.Add(new ReadingModel(accountId: seedId,
                                                      readAt: readTime,
                                                      value: 10));

                Action test = () => context.SaveChanges();

                test.Should().Throw<DbUpdateException>()
                                .WithInnerException<SqliteException>()
                                .Where(e => e.SqliteErrorCode == SQLITE_CONSTRAINT
                                        && e.Message.Contains("UNIQUE")
                                        && e.Message.Contains(nameof(ReadingModel.ReadAt)));
            }
        }

        [Fact]
        public void AccountId_EnforcesIntegrity()
        {
            int existing = 123;

            using (var builder = new MeterDBContextBuilder()
                                    .AccountSeeds(new List<AccountModel> { new AccountModel(accountId: existing) }))
            {
                var context = builder.Build();

                context.Readings.Add(new ReadingModel(accountId: existing + 1, 
                                                      readAt: DateTime.UtcNow, 
                                                      value: 0));

                Action test = () => context.SaveChanges();

                test.Should().Throw<DbUpdateException>()
                                .WithInnerException<SqliteException>()
                                .Where(e => e.SqliteErrorCode == SQLITE_CONSTRAINT
                                        && e.Message.Contains("FOREIGN KEY"));
            }
        }

        [Fact]
        public void ctor_PropertiesInitialiedCorrectly()
        {
            int id = 123;
            DateTime readAt = DateTime.UtcNow;
            int value = 987654;

            var sut = new ReadingModel(accountId: id,
                                       readAt: readAt,
                                       value: value);

            sut.AccountId.Should().Be(id);
            sut.ReadAt.Should().Be(readAt);
            sut.Value.Should().Be(value);
        }
    }
}
