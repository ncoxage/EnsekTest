using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Data.Model;

using Data.Tests.Builders;


namespace Data.Tests
{
    public class MeterReadingTests
    {
        #region AccountIdIsValid

        [Fact]
        public void AccountId_Valid_WhenAccountExists()
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
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
                };

                sut.AccountIdIsValid(context).Should().BeTrue();
            }
        }


        [Fact]
        public void AccountId_Invalid_WhenAccountMising()
        {
            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: 123) }))
            {
                var context = (IMeterDBContext)builder.Build();

                context.Accounts.Count().Should().NotBe(0);

                var sut = new MeterReading
                {
                    AccountId = "1234",
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        [Fact]
        public void AccountId_Invalid_WhenAccountsEmpty()
        {

            using (var builder = new MeterDBContextBuilder())
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = "123",
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("NotANumber")]
        [InlineData("0x123")]
        [InlineData("123.7")]
        [InlineData("-26")]
        public void AccountId_Invalid_WhenNotPureInt(string accountId)
        {
            int testId = 0;

            Int32.TryParse(accountId, out testId);

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
                };

                sut.AccountIdIsValid(context).Should().BeFalse();
            }
        }

        #endregion
    }
}
