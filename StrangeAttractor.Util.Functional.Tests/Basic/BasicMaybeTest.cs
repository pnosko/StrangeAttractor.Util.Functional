﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using StrangeAttractor.Util.Functional.Extensions;

namespace StrangeAttractor.Util.Functional.Tests
{
    public class Dummy
    {
        public string Name { get; set; }
    }

    public class LowerDummy : Dummy
    {
        public string Desc { get; set; }
    }


    public class SeparateDummy
    {
        public Dummy Dummy { get; set; }
    }

    public class DummyUpperException : ArgumentException
    {
        public DummyUpperException() : base() { }
        public DummyUpperException(string message) : base("UPPER: " + message) { }
    }

    public class DummyLowerException : DummyUpperException
    {
        public DummyLowerException() : base() { }
        public DummyLowerException(string message) : base("LOWER: " + message) { }
    }

    public class BasicOptionTest
    {
        [Fact]
        public void WhenValueExistsOrElseContravariant_ItHasAValue()
        {
            var dummy = new LowerDummy { Name = "name" };

            var result = dummy.ToOption().OrElse(() => (new Dummy()).ToOption());

            Assert.True(result.HasValue);
            Assert.Equal("name", result.Value.Name);
        }

        [Fact]
        public void WhenValueExistsOrElse_ItHasAValue()
        {
            var dummy = new LowerDummy { Name = "name" };

            var result = dummy.ToOption().OrElse(() => (new LowerDummy()).ToOption());

            Assert.True(result.HasValue);
            Assert.Equal("name", result.Value.Name);
        }

        [Fact]
        public void WhenValueNull_ItDoesNotHaveAValue()
        {
            Dummy obj = null;
            var result = obj.ToOption();

            Assert.False(result.HasValue);
        }

        [Fact]
        public void WhenCastingToIncompatibleType_ItHasNoValue()
        {
            var obj = new Dummy();
            var result = obj.Cast<SeparateDummy>();

            Assert.False(result.HasValue);
        }

        [Fact]
        public void WhenUsingIntermediateSelectMany_ItHasAValue()
        {
            var dummy = new Dummy();
            var other = new SeparateDummy { Dummy = dummy };

            var result = from val1 in dummy.ToOption()
                         from val2 in other.ToOption()
                         select val2.Dummy == val1;

            Assert.True(result.HasValue);
            Assert.True(result.GetOrDefault());
        }

        [Fact]
        public void WhenUsingIntermediateSelectManyWithNull_ItHasNoValue()
        {
            var result = from val1 in (new Dummy()).ToOption()
                         from val2 in ((SeparateDummy)null).ToOption()
                         select val2.Dummy == val1;

            Assert.False(result.HasValue);
        }
    }
}
