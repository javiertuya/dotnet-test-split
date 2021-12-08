using System;
using System.IO;

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
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: NetcoreJunitSplit 'trx file name'");
                return;
            }
            string inputFileName = args[0];
            string workingFolder = inputFileName + ".split";
            Console.WriteLine("Split mstest into multiple junit testsuite files from: " + inputFileName);

            new DotnetTestSplitMain().Run(inputFileName, workingFolder);
        }
    }
    public class DotnetTestSplitMain
    { 
        public void Run(string inputFileName, string workingFolder)
        {
            //Transforma el archivo.trx a formato junit (archivo.trx.xml) utilizando la hoja de estilos de conversion
            string trx = File.ReadAllText(inputFileName);
            string xslt = File.ReadAllText("mstest-to-junit.xslt"); //copied to bin on build
            string junit = new Transformer().TransformTrxToJUnit(xslt, trx);
            string junitFile = Path.Combine(inputFileName + ".junit.xml");
            Console.WriteLine("Generating JUnit file for all classes: " + junitFile);
            File.WriteAllText(junitFile, junit);

            //Separa el contenido del resport formato junit en los diferentes archivos TEST-*
            new Splitter().SplitToJunit(workingFolder, junit);

        }
    }
}
