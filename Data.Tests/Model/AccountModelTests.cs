﻿using System;
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
    public class AccountModelTests
    {
        [Fact]
        public void ctor_PropertiesInitialiedCorrectly()
        {
            int id = 123;
            var first = "first";
            var last = "last";

            var sut = new AccountModel(id: id,
                                       first: first,
                                       last: last);

            sut.AccountId.Should().Be(id);
            sut.FirstName.Should().Be(first);
            sut.LastName.Should().Be(last);
        }
    }
}