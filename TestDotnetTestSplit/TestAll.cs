using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using System;

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
        [DataRow("TEST-TestAssetsMstest.One.ClassTwo.xml")]
        [DataRow("TEST-TestAssetsMstest.Two.ClassOne.xml")]
        [DataRow("TEST-TestAssetsMstest.Two.ClassTwo.xml")]
        public void TestMstest(string testFileName)
        {
            //DotnetTestSplitMain w = new DotnetTestSplitMain();
            //w.Run(trxFile, outFolder);
            //Split must be executed from outside

            //remove times and sort expected and actual to make comparable
            string expected = ReadComparable(Path.Combine(expectedFolder, testFileName));
            string actual = ReadComparable(Path.Combine(outFolder, testFileName));

            //to check differences manually with compared data
            Directory.CreateDirectory(Path.Combine(outFolder + "compare-expected"));
            Directory.CreateDirectory(Path.Combine(outFolder + "compare-actual"));
            File.WriteAllText(Path.Combine(outFolder + "compare-expected", testFileName), expected);
            File.WriteAllText(Path.Combine(outFolder + "compare-actual", testFileName), actual);

            Assert.AreEqual(expected, actual, "Comparing file: " + testFileName);
        }
        private string ReadComparable(string fileName)
        {
            //sort by inner elements (testcase)
            var xDoc = XDocument.Load(fileName);
            var newxDoc = new XElement("testsuite", xDoc.Root
                                               .Elements()
                                               .OrderByDescending(x => (string)x.Attribute("classname"))
                                               .ThenBy(x => (string)x.Attribute("name"))
                                        );
            xDoc.Root.Elements().Remove();
            xDoc.Root.Add(newxDoc.Elements());
            //remove timestamps and CR
            string xml = xDoc.ToString();
            string regex = "time=\"([0-9\\.])*\"";
            xml = Regex.Replace(xml, regex, "time=\"\"");
            //normalize absolute file paths (expected files are stored without the path of the solution)
            xml = xml.Replace(@"\", "/");
            string[] curDir = Directory.GetCurrentDirectory().Replace(@"\", "/").Split("/");
            string baseDir = string.Join("/", curDir, 0, curDir.Length - 4);
            xml = xml.Replace(baseDir, "", StringComparison.InvariantCultureIgnoreCase);
            //xml = xml.Replace("/home/runner/work/dotnet-test-split/dotnet-test-split/", "");

            return xml.Replace("\r", "");
        }
    }
}
