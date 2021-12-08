using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Giis.DotnetTestSplit
{
    class Splitter
    {
        public void SplitToJunit(string workingFolder, string junitReport)
        {
            Directory.CreateDirectory(workingFolder);
            Dictionary<string, List<XmlNode>> suites = new Dictionary<string, List<XmlNode>>();
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            //leo el sml a un string, si uso doc.Load podria fallar cuando haya caracteres no ascii;
            doc.LoadXml(junitReport);
            XmlNode node = doc.FirstChild.NextSibling.FirstChild; //el testsuite (la declaracion la cuenta como primer hijo)
            foreach (XmlNode xn in node.ChildNodes)
                if (xn.NodeType == XmlNodeType.Element && xn.Name == "testcase")
                {
                    //guardo los testcases bajo un diccionario con clave el nombre completo de la clase para crear un suite por cada uno
                    string className = xn.Attributes["classname"].Value;
                    if (className.Contains(",")) //en net la clase aparece como 'class name', 'assembly', Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
                    {
                        className = className.Split(',')[0].Trim();
                        xn.Attributes["classname"].Value = className;
                    }
                    List<XmlNode> suite;
                    suites.TryGetValue(className, out suite);
                    if (suite == null)
                        suites.Add(className, new List<XmlNode>());
                    suites[className].Add(xn);
                }
            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
            foreach (string key in suites.Keys)
            {
                int tests = 0;
                int errors = 0;
                int failures = 0;
                int skipped = 0;
                double time = 0.0;
                string outputFile = Path.Combine(workingFolder, "TEST-" + key + ".xml");
                Console.WriteLine("Split JUnit single class file: " + outputFile);
                StringWriter writer = new StringWriter();
                foreach (XmlNode xn in suites[key])
                {
                    if (xn.Attributes["outcome"].Value == "NotExecuted") //esto no lo crea el xslt
                        xn.AppendChild(xn.OwnerDocument.CreateElement("skipped"));
                    tests++;
                    string outcome = xn.Attributes["outcome"].Value;
                    if (outcome == "Failed") failures++;
                    else if (outcome == "NotExecuted") skipped++;
                    //Errors son siempre failed, Passed no se cuenta
                    //time = "0.217"
                    string timeString = xn.Attributes["time"].Value;
                    //los skipped aparecen con un tiempo NaN
                    if (string.IsNullOrEmpty(timeString) || timeString == "NaN")
                        xn.Attributes["time"].Value = "0.0"; //para que no aparezca NaN en la salida
                    else
                        time += double.Parse(timeString, provider); //solo suma si no es null
                                                                    //escribe al final porque puede haber sido modificado tras lo anterior
                    writer.WriteLine(xn.OuterXml);
                }
                string result = "<testsuite name=\"" + key + "\""
                    + " tests=\"" + tests + "\""
                    + " failures=\"" + failures + "\""
                    + " errors=\"" + errors + "\""
                    + " skipped=\"" + skipped + "\""
                    + " time=\"" + time.ToString(provider) + "\""
                    + " >\n";
                result += writer.ToString() + "</testsuite>";
                File.WriteAllText(outputFile, result);
            }
        }
    }
}
