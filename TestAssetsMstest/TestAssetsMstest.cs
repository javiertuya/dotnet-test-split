using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestAssetsMstest.One
{
    [TestClass]
    public class ClassOne
    {
        [TestMethod]
        public void TestOneOnePass()
        {
        }
        [TestMethod]
        public void TestOneOnePassOutput()
        {
            System.Console.WriteLine("This is a console message");
        }
    }
    [TestClass]
    public class ClassTwo
    {
        [TestMethod]
        public void TestOneTwoPass()
        {
        }
        //attributes in same line to easy translation to nunit
        [TestMethod][Ignore]
        public void TestOneOneIgnored()
        {
            Assert.Fail("This is ignored, no failure");
        }
        //attributes in same line to easy translation to nunit
        [TestMethod][Ignore("This is the ignore cause")]
        public void TestOneOneIgnoredWithCause()
        {
            Assert.Fail("This is ignored with cause, no failure");
        }
        [TestMethod]
        public void TestOneTwoFail()
        {
            Assert.AreEqual(1, 2);
        }
        [TestMethod]
        public void TestOneTwoFailMessage()
        {
            Assert.AreEqual(1, 2, "This is a failure message");
        }
        [TestMethod]
        public void TestOneTwoFailException()
        {
            throw new Exception("This is an exception message");
        }
    }
}
namespace TestAssetsMstest.Two
{
    [TestClass]

    public class ClassOne
    {
        [TestMethod]
        public void TestTwoOnePass()
        {
        }
    }
    [TestClass]
    public class ClassTwo
    {
        [TestMethod]
        public void TestTwoTwoPass()
        {
            Assert.AreEqual(1, 1);
        }
        [DataTestMethod]
        [DataRow(2, 1)]
        [DataRow(4, 2)]
        [DataRow(88, 4)]
        public void TestTwoTwoParametrized(int expected, int actual)
        {
            Assert.AreEqual(expected, actual * 2);
        }
        [TestMethod]
        public void TestTwoTwoLong()
        {
            System.Threading.Thread.Sleep(541);
            Assert.AreEqual(1, 1);
        }
    }
}

namespace TestAssetsMstest.Stp
{
    [TestClass]
    public class ClassStp
    {
        [TestInitialize] public void SetupFail()
        {
            throw new Exception("This test setup fails");
        }
        [TestMethod]
        public void TestStpOne()
        {
        }
        [TestMethod]
        public void TestStpTwo()
        {
        }
    }
}
