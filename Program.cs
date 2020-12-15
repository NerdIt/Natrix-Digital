using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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
                    string commentTest = "";
                    for(int j = 0; j < line.Length; j++)
                    {
                        if(!Char.IsWhiteSpace(line[j]))
                        {
                            commentTest += line[j];
                        }
                        if (commentTest.Length >= 2)
                            break;
                    }
                    if(!commentTest.Equals("//"))
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
            List<string> stringSplit = new List<string>() { "" };
            bool insideString = false;
            for(int i = 0; i < masterContracted.Length; i++)
            {

                if (masterContracted[i] == '"' && !insideString)
                {
                    insideString = true;
                    stringSplit[stringSplit.Count - 1] = Regex.Replace(stringSplit[stringSplit.Count - 1].Replace("\t", " "), reduceMultiSpace, " ");
                    stringSplit.Add("");
                    stringSplit[stringSplit.Count - 1] += masterContracted[i];
                }
                else if(masterContracted[i] == '"' && insideString)
                {
                    insideString = false;
                    stringSplit[stringSplit.Count - 1] += masterContracted[i];
                    stringSplit.Add("");
                }
                else
                {
                    stringSplit[stringSplit.Count - 1] += masterContracted[i];
                }

                
                
            }

            if(!insideString && stringSplit[stringSplit.Count - 1][stringSplit[stringSplit.Count - 1].Length - 1] != '"')
            {
                stringSplit[stringSplit.Count - 1] = Regex.Replace(stringSplit[stringSplit.Count - 1].Replace("\t", " "), reduceMultiSpace, " ");
            }


            masterContracted = "";
            for(int i = 0; i < stringSplit.Count; i++)
            {
                masterContracted += stringSplit[i];
            }

            //masterContracted = Regex.Replace(masterContracted.Replace("\t", " "), reduceMultiSpace, " ");
            //Console.WriteLine(masterContracted);
            
            return masterContracted;
        }

        
    }

    static class Interpreter
    {
        public static List<Token> tokens = new List<Token>();
        private static string contractedCode;

        public static void Interprete(string contraction)
        {
            contractedCode = contraction;
            //  Perform crude version of the Lixical analysis
            Tokenise();

            for(int i = 0; i < tokens.Count; i++)
            {

                Console.WriteLine(tokens[i].tokenString);
            }
        }

        public static void Tokenise()
        {

            string[] splitDistribution = SplitStringExclusion(' ', contractedCode);
            //for (int j = 0; j < splitDistribution.Length; j++)
            //{
            //    Console.WriteLine(splitDistribution[j]);
            //}
            List<string> basicToken = new List<string>();
            bool inString = false;

            //for(int i = 0; i < splitDistribution.Length; i++)
            //{
            //    Console.WriteLine(splitDistribution[i]);
            //}
            for (int i = 0; i < splitDistribution.Length; i++)
            {
                bool pureString = true;
                bool pureOther = false;
                
                List<string> processingToken = new List<string>() { "" };
                int tokenIndex = 0;

                for(int j = 0; j < splitDistribution[i].Length; j++)
                {
                    //Console.WriteLine(splitDistribution[i][j] + " ::: " + inString);
                    //Thread.Sleep(100);
                    if (((Char.IsLetterOrDigit(splitDistribution[i][j]) || splitDistribution[i][j] == '_' || splitDistribution[i][j] == '"' || 
                        Char.IsWhiteSpace(splitDistribution[i][j])) || inString) && pureString)
                    {
                        if (splitDistribution[i][j] == '"')
                            inString = !inString;
                        processingToken[tokenIndex] += splitDistribution[i][j];
                    }
                    else
                    {
                        pureString = false;
                        pureOther = true;
                        if (!(Char.IsLetterOrDigit(splitDistribution[i][j]) || splitDistribution[i][j] == '_' || splitDistribution[i][j] == '"') && pureOther)
                        {
                            tokenIndex++;
                            processingToken.Add("");
                            //Console.Write("\n");
                            processingToken[tokenIndex] += splitDistribution[i][j];
                        }
                        else
                        {
                            pureString = true;
                            pureOther = false;
                            tokenIndex++;
                            processingToken.Add("");
                            //Console.Write("\n");
                            processingToken[tokenIndex] += splitDistribution[i][j];
                        }
                    }
                }
                
               
                for (int j = 0; j < processingToken.Count; j++)
                {
                    if(processingToken[j].Equals("") || processingToken[j].Equals(" "))
                    {
                        //processingToken.RemoveAt(j);
                    }
                    else
                    {
                        basicToken.Add(processingToken[j]);
                    }
                    
                }

                //Repair Strings
                //for(int j = 0; j < basicToken.Count; j++)
                //{
                //        if (basicToken[j][0] == '"' && basicToken[j][basicToken[j].Length - 1] != '"')
                //        {
                //            string newToken = basicToken[j];
                //            int newIndex = j;
                //            for (int l = j; l < basicToken.Count; l++)
                //            {
                //                if (basicToken[l][basicToken[l].Length - 1] != '"')
                //                {
                //                    newToken += basicToken[l];
                //                }
                //                else
                //                {
                //                    newToken += basicToken[l];
                //                    newIndex = l;
                //                    break;
                //                }
                //            }
                //            basicToken[j] = newToken;
                //            j = newIndex;
                //        }
                //}
                
            }
            for (int i = 0; i < basicToken.Count; i++)
            {
                //Console.WriteLine(basicToken[i] + "|");
                if(IsKeyword(basicToken[i]))
                {
                    tokens.Add(new Keyword(basicToken[i]));
                }
                else if(IsSeparator(basicToken[i]))
                {
                    tokens.Add(new Separator(basicToken[i]));
                }
                else if(IsOperator(basicToken[i]))
                {
                    tokens.Add(new Operator(basicToken[i]));
                }
                else if (IsLiteral(basicToken[i]))
                {
                    tokens.Add(new Literal(basicToken[i]));
                }
                else
                {
                    tokens.Add(new Identifier(basicToken[i]));
                }
            }


            
        }


        private static bool IsKeyword(string value)
        {
            switch(value.ToLower().Replace(" ", ""))
            {
                case "environment":     return true;
                case "object":          return true;
                case "var":             return true;
                case "method":          return true;
                case "return":          return true;
                case "printline":       return true;
                case "print":           return true;
                case "getline":         return true;
                case "get":             return true;
                case "num":             return true;
                case "text":            return true;
                case "bool":            return true;
                default:                return false;
            }
        }

        private static bool IsSeparator(string value)
        {
            
            switch (value.Replace(" ", ""))
            {
                case "(": return true;
                case ")": return true;
                case ";": return true;
                case "{": return true;
                case "}": return true;
                case "[": return true;
                case "]": return true;
                default: return false;
            }
        }

        private static bool IsOperator(string value)
        {
            switch (value.Replace(" ", ""))
            {
                case "+": return true;
                case "-": return true;
                case "*": return true;
                case "/": return true;
                case ":": return true;
                case "=": return true;
                default: return false;
            }
        }
        private static bool IsLiteral(string value)
        {
            value = value.ToLower();
            if(value.Equals("true") || value.Equals("false"))
                return true;
            if (value[0] == '"' && value[value.Length - 1] == '"')
                return true;
            double d;
            if (double.TryParse(value, out d))
                return true;
            return false;
        }

        private static string[] SplitStringExclusion(Char splitAt, string body)
        {
            List<string> split = new List<string>() { "" };
            bool insideString = false;
            for(int i = 0; i < body.Length; i++)
            {
                if(body[i] == '\"' && !insideString)
                {
                    //split[split.Count - 1] += body[i];
                    insideString = true;
                }
                else if(body[i] == '\"' && insideString)
                {
                    //split[split.Count - 1] += body[i];
                    insideString = false;
                }

                if(Char.IsWhiteSpace(body[i]) && !split[split.Count - 1].Equals("") && !insideString)
                {
                    split.Add("");
                }
                else
                {
                    split[split.Count - 1] += body[i];
                }
                //Console.Write(body[i]);
            }

            return split.ToArray();
        }



    }
}
