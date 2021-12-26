using NUnit.Framework;
using System;

namespace TestAssetsNunit.One
{
    [TestFixture]
    public class ClassOne
    {
        [Test]
        public void TestOneOnePass()
        {
        }
        [Test]
        public void TestOneOnePassOutput()
        {
            System.Console.WriteLine("This is a console message");
        }
    }
    [TestFixture]
    public class ClassTwo
    {
        [Test]
        public void TestOneTwoPass()
        {
        }
        //attributes in same line to easy translation to nunit
        [Test][Ignore("")]
        public void TestOneOneIgnored()
        {
            Assert.Fail("This is ignored, no failure");
        }
        //attributes in same line to easy translation to nunit
        [Test][Ignore("This is the ignore cause")]
        public void TestOneOneIgnoredWithCause()
        {
            Assert.Fail("This is ignored with cause, no failure");
        }
        [Test]
        public void TestOneTwoFail()
        {
            Assert.AreEqual(1, 2);
        }
        [Test]
        public void TestOneTwoFailMessage()
        {
            Assert.AreEqual(1, 2, "This is a failure message");
        }
        [Test]
        public void TestOneTwoFailException()
        {
            throw new Exception("This is an exception message");
        }
    }
}
namespace TestAssetsNunit.Two
{
    [TestFixture]

    public class ClassOne
    {
        [Test]
        public void TestTwoOnePass()
        {
        }
    }
    [TestFixture]
    public class ClassTwo
    {
        [Test]
        public void TestTwoTwoPass()
        {
            Assert.AreEqual(1, 1);
        }
        [Test]
        [TestCase(2, 1)]
        [TestCase(4, 2)]
        [TestCase(88, 4)]
        public void TestTwoTwoParametrized(int expected, int actual)
        {
            Assert.AreEqual(expected, actual * 2);
        }
        [Test]
        public void TestTwoTwoLong()
        {
            System.Threading.Thread.Sleep(541);
            Assert.AreEqual(1, 1);
        }
    }
}

namespace TestAssetsNunit.Stp
{
    [TestFixture]
    public class ClassStp
    {
        [SetUp] public void SetupFail()
        {
            throw new Exception("This test setup fails");
        }
        [Test]
        public void TestStpOne()
        {
        }
        [Test]
        public void TestStpTwo()
        {
        }
    }
}
