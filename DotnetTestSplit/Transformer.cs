using System.IO;
using System.Xml;

namespace Giis.DotnetTestSplit
{
    class Transformer
    {
        public string TransformTrxToJUnit(string styleSheet, string trxReport)
        {
            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.LoadXml(trxReport);
            XmlDocument style = new XmlDocument();
            style.XmlResolver = null;
            style.LoadXml(styleSheet);

            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();
            transform.Load(style); // compiled stylesheet
            System.IO.StringWriter writer = new System.IO.StringWriter();
            XmlReader xmlReadB = new XmlTextReader(new StringReader(doc.DocumentElement.OuterXml));
            transform.Transform(xmlReadB, null, writer);
            return writer.ToString();
        }
    }
}
