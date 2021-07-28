using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;
using Xunit;

using Data;
using Data.Model;


namespace Data.Tests
{
    public class ReadingModelTests
    {
        [Fact]
        public void ctor_PropertiesInitialiedCorrectly()
        {
            int id = 123;
            DateTime readAt = DateTime.UtcNow;
            int value = 987654;

            var sut = new ReadingModel(accountId: id,
                                       time: readAt,
                                       value: value);

            sut.AccountId.Should().Be(id);
            sut.ReadAt.Should().Be(readAt);
            sut.Value.Should().Be(value);
        }
    }
}
