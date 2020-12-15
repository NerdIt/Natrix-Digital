using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Natrix_Digital
{
    class Program
    {

        public static string debugDocument = @"C:\Users\admin\Documents\Code.txt";
        static void Main(string[] args)
        {
            List<string> lines = new List<string>();

            using(StreamReader sr = new StreamReader(debugDocument))
            {
                string line;
                while((line = sr.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }

            string contraction = ContractFile(lines.ToArray());
            Interpreter.Interprete(contraction);
            Console.ReadKey();
        }

        const string reduceMultiSpace = @"[ ]{2,}";
        static string ContractFile(string[] lines)
        {
            string masterContracted = "";
            for(int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    masterContracted += lines[i][j];
                    
                }
            }

            masterContracted = Regex.Replace(masterContracted.Replace("\t", " "), reduceMultiSpace, " ");
            //Console.WriteLine(masterContracted);
            
            return "";
        }

        
    }

    static class Interpreter
    {


        public static void Interprete(string contraction)
        {

        }

    }
}
