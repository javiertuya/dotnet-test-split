using Microsoft.VisualStudio.TestTools.UnitTesting;
using Giis.DotnetTestSplit;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;

namespace Test4Giis.DotnetTestSplit
{
    [TestClass]
    public class TestAll
    {
        //private string trxFile = "../../../../reports/mstest-report.trx";
        private string expectedFolder = "../../../../expected/mstest-report.trx.split";
        private string outFolder = "../../../../reports/mstest-report.trx.split";

        [DataTestMethod]
        [DataRow("TEST-TestAssetsMstest.One.ClassOne.xml")]
        [DataRow("TEST-TestAssetsMstest.One.ClassOne.xml")]
        [DataRow("TEST-TestAssetsMstest.Two.ClassOne.xml")]
        [DataRow("TEST-TestAssetsMstest.Two.ClassTwo.xml")]
        public void TestMstest(string testFile)
        {
            //DotnetTestSplitMain w = new DotnetTestSplitMain();
            //w.Run(trxFile, outFolder);
            //Split must be executed from outside
            AssertFile(testFile);
        }
        private void AssertFile(string fileName)
        {
            string expected = File.ReadAllText(Path.Combine(expectedFolder, fileName));
            string actual = File.ReadAllText(Path.Combine(outFolder, fileName));
            //remove times and sort expected and actual to make comparable
            expected = MakeComparable(expected);
            actual = MakeComparable(actual);
            Assert.AreEqual(expected, actual, "Comparing file: " + fileName);
        }
        private string MakeComparable(string lines)
        {
            string regex = "time=\"([0-9.])*\"";
            lines= Regex.Replace(lines, regex, "time=\"\"").Replace("\r", "");
            string[] linesArray =lines.Split("\n");
            Array.Sort(linesArray);
            return string.Join('\n', linesArray);
        }
    }
}
