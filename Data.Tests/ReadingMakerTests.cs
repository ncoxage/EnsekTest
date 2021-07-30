using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Data;

using static Data.Globals;

namespace Data.Tests
{
    public class ReadingMakerTests
    {
        [Fact]
        public void NewReading_CreatesMeterReading()
        {
            var testId = "123";
            var testDate = DateTime.UtcNow.ToString(READAT_FORMAT);
            var testValue = "12345";

            var sut = new ReadingMaker();

            var actual = sut.NewReading(accountId: testId, readAt: testDate, value: testValue);

            actual.AccountId.Should().Be(testId);
            actual.ReadAt.Should().Be(testDate);
            actual.ReadValue.Should().Be(testValue);
        }
    }
}
