using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using System;
using Giis.Visualassert;
using Giis.DotnetTestSplit;

namespace Test4Giis.DotnetTestSplit
{
    [TestFixture]
    public class TestSplit
    {
        [Test]
        [TestCase("Mstest", "One.ClassOne")]
        [TestCase("Mstest", "One.ClassTwo")]
        [TestCase("Mstest", "Two.ClassOne")]
        [TestCase("Mstest", "Two.ClassTwo")]
        [TestCase("Mstest", "Stp.ClassStp")]
        [TestCase("Nunit", "One.ClassOne")]
        [TestCase("Nunit", "One.ClassTwo")]
        [TestCase("Nunit", "Two.ClassOne")]
        [TestCase("Nunit", "Two.ClassTwo")]
        [TestCase("Nunit", "Stp.ClassStp")]
        [TestCase("Xunit", "One.ClassOne")]
        [TestCase("Xunit", "One.ClassTwo")]
        [TestCase("Xunit", "Two.ClassOne")]
        [TestCase("Xunit", "Two.ClassTwo")]
        [TestCase("Xunit", "Stp.ClassStp")]
        public void TestAll(string framework, string testClass)
        {
            //Generation of trx files y split must be executed from outside
            string testFileName = "TEST-TestAssets" + framework + "." + testClass + ".xml";
            string expectedFolder = "../../../../expected/" + framework.ToLower() + "-report.trx.split";
            string outFolder = "../../../../reports/" + framework.ToLower() + "-report.trx.split";
            //uncomment/customize only for debug
            //DotnetTestSplitMain w = new DotnetTestSplitMain();
            //w.Run("../../../../reports/mstest-report.trx", outFolder);

            //remove times and sort expected and actual to make comparable
            string expected = ReadComparable(Path.Combine(expectedFolder, testFileName));
            string actual = ReadComparable(Path.Combine(outFolder, testFileName));

            //to manually check differences of compared data
            //Directory.CreateDirectory(Path.Combine(outFolder + "compare-expected"));
            //Directory.CreateDirectory(Path.Combine(outFolder + "compare-actual"));
            //File.WriteAllText(Path.Combine(outFolder + "compare-expected", testFileName), expected);
            //File.WriteAllText(Path.Combine(outFolder + "compare-actual", testFileName), actual);

            VisualAssert va = new VisualAssert().SetUseLocalAbsolutePath(true); 
            va.AssertEquals(expected, actual, "Comparing file: " + testFileName, testFileName + ".html");
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
            string regex = "time=\"([0-9E\\-\\.])*\"";
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
