using System;
using System.Linq;

using FluentAssertions;
using Xunit;

using Data.Model;

using static Data.Globals;

using Data.Tests.Builders;


namespace Data.Tests
{
    public class MeterReadingTests
    {
        static readonly Func<int, string> makeReadValue = (readValue) => readValue.ToString("00000");
        static readonly Func<DateTime, string> makeReadAt = (readAt) => readAt.ToString(READAT_FORMAT);

        #region LoadReading

        [Fact]
        public async void LoadReading_True_WhenReadingValid()
        {
            int testId = 123;
            var testReading = new ReadingModel(accountId: testId,
                                               readAt: Convert.ToDateTime(makeReadAt(DateTime.UtcNow)),
                                               value: 0);
            var seedAccount = new AccountModel(accountId: testId);

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { seedAccount }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().AccountId.Should().Be(testId);
                context.Readings.Count().Should().Be(0);

                var sut = new MeterReading
                {
                    AccountId = testReading.AccountId.ToString(),
                    ReadAt = makeReadAt(testReading.ReadAt),
                    ReadValue = makeReadValue(testReading.Value)
                };

                (await sut.LoadReading(context)).Should().BeTrue();

                context.Readings.Count().Should().Be(1);

                var actual = seedAccount.Readings.First();
                actual.ReadAt.Should().Be(testReading.ReadAt);
                actual.Value.Should().Be(testReading.Value);
            }
        }

        [Fact]
        public async void LoadReading_False_WhenDuplicateReading()
        {
            int testId = 123;
            var seedReading = new ReadingModel(accountId: testId,
                                               readAt: Convert.ToDateTime(makeReadAt(DateTime.UtcNow)),
                                               value: 0);

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) })
                                       .ReadingSeeds(new[] { seedReading }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().AccountId.Should().Be(testId);
                context.Readings.Count().Should().Be(1);

                var sut = new MeterReading
                {
                    AccountId = seedReading.AccountId.ToString(),
                    ReadAt = makeReadAt(seedReading.ReadAt),
                    ReadValue = makeReadValue(seedReading.Value)
                };

                (await sut.LoadReading(context)).Should().BeFalse();

                context.Readings.Count().Should().Be(1);
            }
        }

        [Fact]
        public async void LoadReading_False_WhenReadValueInvalid()
        {
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().AccountId.Should().Be(testId);
                context.Readings.Count().Should().Be(0);

                var sut = new MeterReading
                {
                    AccountId = (testId + 1).ToString(),
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = "0"
                };

                (await sut.LoadReading(context)).Should().BeFalse();

                context.Readings.Count().Should().Be(0);
            }
        }

        [Fact]
        public async void LoadReading_False_WhenReadAtInvalid()
        {
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().AccountId.Should().Be(testId);
                context.Readings.Count().Should().Be(0);

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = string.Empty,
                    ReadValue = makeReadValue(0)
                };

                (await sut.LoadReading(context)).Should().BeFalse();

                context.Readings.Count().Should().Be(0);
            }
        }

        [Fact]
        public async void LoadReading_False_WhenAccountMissing()
        {
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = builder.Build();

                context.Accounts.FirstOrDefault().AccountId.Should().Be(testId);
                context.Readings.Count().Should().Be(0);

                var sut = new MeterReading
                {
                    AccountId = (testId + 1).ToString(),
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = makeReadValue(0)
                };

                (await sut.LoadReading(context)).Should().BeFalse();

                context.Readings.Count().Should().Be(0);
            }
        }

        #endregion

        #region ReadAtIsValid

        [Theory]
        [InlineData("24/12/2020 12:00")]
        [InlineData("30/04/2021 12:00")]
        [InlineData("29/02/2020 12:00")]
        public void ReadAtIsValid_True_WhenCorrectFormat(string readAt)
        {
            var sut = new MeterReading
            {
                AccountId = "123",
                ReadAt = readAt,
                ReadValue = makeReadValue(0)
            };

            sut.ReadAtIsValid().Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotARealDate 001")]
        [InlineData("24/12/2020")]
        [InlineData("24/12/2020 12:00:34")]
        [InlineData("31/04/2021 12:00")]
        [InlineData("29/02/2021 12:00")]
        public void ReadAtIsValid_False_WhenNotCorrectFormat(string readAt)
        {

            var sut = new MeterReading
            {
                AccountId = "123",
                ReadAt = readAt,
                ReadValue = makeReadValue(0)
            };

            sut.ReadAtIsValid().Should().BeFalse();
        }

        #endregion

        #region ReadValueIsValid

        [Theory]
        [InlineData("00000")]
        [InlineData("12345")]
        [InlineData("99999")]
        public void ReadValueIsValid_True_WhenCorrectFormat(string value)
        {
            var sut = new MeterReading
            {
                AccountId = "123",
                ReadAt = makeReadAt(DateTime.UtcNow),
                ReadValue = value
            };

            sut.ReadValueIsValid().Should().BeTrue();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotANumber")]
        [InlineData("NotAN")]
        [InlineData("123")]
        [InlineData("0x123")]
        [InlineData("123.7")]
        [InlineData("-2600")]
        public void ReadValueIsValid_False_WhenNotCorrectFormat(string value)
        {
            var sut = new MeterReading
            {
                AccountId = "123",
                ReadAt = makeReadAt(DateTime.UtcNow),
                ReadValue = value
            };

            sut.ReadValueIsValid().Should().BeFalse();
        }



        #endregion

        #region AccountIdIsValid

        [Fact]
        public void AccountIdIsValid_True_WhenAccountExists()
        {
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = (IMeterDBContext)builder.Build();

                context.Accounts.Count().Should().Be(1);
                context.Accounts.First().AccountId.Should().Be(testId);

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = makeReadValue(0)
                };

                sut.AccountIdIsValid(context).Should().BeTrue();
            }
        }


        [Fact]
        public void AccountIdIsValid_False_WhenAccountMising()
        {
            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: 123) }))
            {
                var context = (IMeterDBContext)builder.Build();

                context.Accounts.Count().Should().NotBe(0);

                var sut = new MeterReading
                {
                    AccountId = "1234",
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = makeReadValue(0)
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        [Fact]
        public void AccountIdIsValid_False_WhenAccountsEmpty()
        {

            using (var builder = new MeterDBContextBuilder())
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = "123",
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = makeReadValue(0)
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("NotANumber")]
        [InlineData("0x123")]
        [InlineData("123.7")]
        [InlineData("-26")]
        public void AccountIdIsValid_False_WhenNotPureInt(string accountId)
        {

#pragma warning disable CA1806 // Do not ignore method results
            Int32.TryParse(accountId, out int testId);
#pragma warning restore CA1806 // Do not ignore method results

            //  use MeterDBContext/AccountModel instance to ensure it's absence is not the cause of the false returns
            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = makeReadAt(DateTime.UtcNow),
                    ReadValue = makeReadValue(0)
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        #endregion
    }
}
