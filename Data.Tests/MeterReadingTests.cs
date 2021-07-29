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
        #region ReadValueIsValid

        [Theory]
        [InlineData("00000")]
        [InlineData("12345")]
        [InlineData("99999")]
        public void ReadValueIsValid_True_WhenCorrectFormat(string value)
        {
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = value
                };

                sut.ReadValueIsValid().Should().BeTrue();
            }
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
            int testId = 123;

            using (var builder = new MeterDBContextBuilder()
                                       .AccountSeeds(new[] { new AccountModel(accountId: testId) }))
            {
                var context = (IMeterDBContext)builder.Build();

                var sut = new MeterReading
                {
                    AccountId = testId.ToString(),
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = value
                };

                sut.ReadValueIsValid().Should().BeFalse();
            }
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
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
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
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
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
                    ReadAt = DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm"),
                    ReadValue = 0.ToString("00000")
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
