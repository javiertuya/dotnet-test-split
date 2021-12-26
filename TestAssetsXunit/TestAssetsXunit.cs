using Xunit;
using System;

namespace TestAssetsXunit.One
{
    //
    public class ClassOne
    {
        [Fact]
        public void TestOneOnePass()
        {
        }
        [Fact]
        public void TestOneOnePassOutput()
        {
            System.Console.WriteLine("This is a console message");
        }
    }
    //
    public class ClassTwo
    {
        [Fact]
        public void TestOneTwoPass()
        {
        }
        //attributes in same line to easy translation to nunit
        [Fact (Skip="cannotbeempty")]
        public void TestOneOneIgnored()
        {
            Assert.True(1 == 2, "This is ignored, no failure");
        }
        //attributes in same line to easy translation to nunit
        [Fact (Skip="This is the ignore cause")]
        public void TestOneOneIgnoredWithCause()
        {
            Assert.True(1 == 2, "This is ignored with cause, no failure");
        }
        [Fact]
        public void TestOneTwoFail()
        {
            Assert.Equal(1, 2);
        }
        [Fact]
        public void TestOneTwoFailMessage()
        {
            Assert.True(1 == 2, "This is a failure message");
        }
        [Fact]
        public void TestOneTwoFailException()
        {
            throw new Exception("This is an exception message");
        }
    }
}
namespace TestAssetsXunit.Two
{
    //

    public class ClassOne
    {
        [Fact]
        public void TestTwoOnePass()
        {
        }
    }
    //
    public class ClassTwo
    {
        [Fact]
        public void TestTwoTwoPass()
        {
            Assert.Equal(1, 1);
        }
        [Theory]
        [InlineData(2, 1)]
        [InlineData(4, 2)]
        [InlineData(88, 4)]
        public void TestTwoTwoParametrized(int expected, int actual)
        {
            Assert.Equal(expected, actual * 2);
        }
        [Fact]
        public void TestTwoTwoLong()
        {
            System.Threading.Thread.Sleep(541);
            Assert.Equal(1, 1);
        }
    }
}

namespace TestAssetsXunit.Stp
{
    //
    public class ClassStp
    {
        public ClassStp()
        {
            throw new Exception("This test setup fails");
        }
        [Fact]
        public void TestStpOne()
        {
        }
        [Fact]
        public void TestStpTwo()
        {
        }
    }
}
