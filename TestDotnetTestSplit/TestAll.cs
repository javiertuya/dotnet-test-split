using NUnit.Framework;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
using Giis.Visualassert;
using Giis.DotnetTestSplit;

namespace Test4Giis.DotnetTestSplit
{
    /// <summary>
    /// Tests are intended to run from commandline, in a GitHub Acctions workflow or in local:
    /// Execute commands in file run-test.bat that
    /// - Generates the test assets for each platform by executing the tess (most of tests must fail)
    /// - Optionally execute 'ant report' to see a junit style html report for each platform
    /// - Compiles, installs and executes DotnetTestSplit
    /// - Executes this test or each class and platform producing the result in a .trx file
    ///   under the TestResults folder of this project
    /// - As comparison is made with VisualAssert, the html files that were produced containt the detailed
    ///   expected/actual test comparison
    /// </summary>
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
            // Generation of trx files y split must be executed from outside
            // Here we compare th splitted files for all combinations of class that produce the report and platform
            // Uses VisualAssert to have a clear display of differences in tests that fail
            string testFileName = "TEST-TestAssets" + framework + "." + testClass + ".xml";
            string expectedFolder = "../../../../expected/" + framework.ToLower() + "-report.trx.split";
            string outFolder = "../../../../reports/" + framework.ToLower() + "-report.trx.split";
            //create the reports output folder if it does not exist so a standalone run does not
            //crash with DirectoryNotFoundException; a missing actual file just fails the comparison
            Directory.CreateDirectory(outFolder);
            //remove times and sort expected and actual to make comparable
            string expected = ReadComparable(Path.Combine(expectedFolder, testFileName));
            string actual = ReadComparable(Path.Combine(outFolder, testFileName));

            VisualAssert va = new VisualAssert().SetUseLocalAbsolutePath(true); 
            va.AssertEquals(expected, actual, "Comparing file: " + testFileName, testFileName + ".html");
        }
        private string ReadComparable(string fileName)
        {
            //when the actual splitted file has not been generated, return empty content so the
            //comparison fails clearly (showing the missing file) instead of throwing
            if (!File.Exists(fileName))
                return "";
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
            //normalize absolute source paths in stack traces so the comparison is independent
            //of the environment (CI vs local): strip everything before the TestAssets* project folder
            xml = xml.Replace(@"\", "/");
            xml = Regex.Replace(xml, @"in [^\n]*?/(TestAssets(Mstest|Nunit|Xunit))/", "in /$1/");

            return xml.Replace("\r", "").Replace("\n\n", "\n");
        }
    }
}
