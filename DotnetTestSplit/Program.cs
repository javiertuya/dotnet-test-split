using System;
using System.IO;
using System.Reflection;

namespace Giis.DotnetTestSplit
{
    /**
     * Genera reports de junit a partir de la salida de mstest:
     * - Transforma el formato .trx a xml de JUnit aplicando la transformacion mstest-to-junit.xslt
     *   obteniendo un unico archvio con todos los resultados
     * - Separa el unico archivo .xml resultante en diferentes archivos (cada uno con una clase) para alimentar los componentes 
     *   junit de ant y los plugins de jenkins que produciran los reports en html
     */
    class Program
    {
        static void Main(string[] args)
        {
            var versionString = Assembly.GetEntryAssembly()?
                                   .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                                   .InformationalVersion
                                   .ToString();
            Console.WriteLine($"DotnetTestSplit v{versionString}");

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: DotnetTestSplit <trx file name> <destination folder>");
                return;
            }
            string inputFileName = args[0];
            string workingFolder = args[1];

            new DotnetTestSplitMain().Run(inputFileName, workingFolder);
        }
    }
    public class DotnetTestSplitMain
    { 
        public void Run(string inputFileName, string workingFolder)
        {
            Console.WriteLine("Process mstest file: " + inputFileName);
            //Transforma el archivo.trx a formato junit (archivo.trx.xml) utilizando la hoja de estilos de conversion
            string trx = File.ReadAllText(inputFileName);
            //string xslt = File.ReadAllText("mstest-to-junit.xslt"); //copied to bin on build
            string xslt = ReadXslt("mstest-to-junit.xslt"); //copied to bin on build as a resource
            string junit = new Transformer().TransformTrxToJUnit(xslt, trx);
            string junitFile = Path.Combine(inputFileName + ".junit.xml");
            Console.WriteLine("Transform to junit file: " + junitFile);
            File.WriteAllText(junitFile, junit);

            //Separa el contenido del resport formato junit en los diferentes archivos TEST-*
            new Splitter().SplitToJunit(workingFolder, junit);
        }
        private string ReadXslt(string resourceName)
        {
            var assembly = typeof(DotnetTestSplit.Program).GetTypeInfo().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            Stream resource = assembly.GetManifestResourceStream("DotnetTestSplit."+resourceName);
            return new StreamReader(resource).ReadToEnd();
        }
    }
}
