using FluentAssertions;
using Xunit;

using Data.Model;

namespace Data.Tests
{
    public class AccountModelTests
    {
        [Fact]
        public void Ctor_PropertiesInitialisedCorrectly()
        {
            int id = 123;
            var first = "first";
            var last = "last";

            var sut = new AccountModel(accountId: id,
                                       firstName: first,
                                       lastName: last);

            sut.AccountId.Should().Be(id);
            sut.FirstName.Should().Be(first);
            sut.LastName.Should().Be(last);
        }
    }
}
