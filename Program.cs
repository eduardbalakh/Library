using System;
using System.Linq;
using System.Text.RegularExpressions;
using Library.Data;


namespace Library
{
    class Program
    {
        
        [STAThread]
        static void Main(string[] args)
        {
            string lastCommand = string.Empty;
            TextProcessor.Initialize();
            CommandParse(String.Join(" ",args),true);
         
            do
            {
                lastCommand = Console.ReadLine();//ReadLineOrEsc(); //ReadLine() не катит из-за условия выхода по Esc. Он занимает поток ввода и метод ReadKey() не получает значения клавиши Esc.
                CommandParse(lastCommand,false);

            } while (lastCommand != "");
            

        }


        private static void CommandParse(string s,bool startCom)
        {

            if (String.IsNullOrEmpty(s))
            {
                if (!startCom)
                {
                    Environment.Exit(0);
                }
                
            }
            else
            {
                s = Regex.Replace(s, @"\p{C}+", string.Empty);
                var tempSplit = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string command,path;
                if (tempSplit.Length == 0)
                {
                    command = " ";
                    path = string.Empty;
                }
                else
                {


                    if (tempSplit.Length == 1)
                    {
                        command = tempSplit[0];
                        path = string.Empty;
                    }
                    else
                    {
                        command = $"{tempSplit[0]} {tempSplit[1]}";
                        path = String.Join(" ", tempSplit.Skip(2).Select(o => o));
                    }
                }

                RequestContext request;
                switch (command.ToLower())
                {
                    case "создание словаря":
                        //создание словаря C:\Users\Эдуард\OneDrive - УрФУ\testfile.txt
                        request = TextProcessor.CreateLibrary(path);
                        Console.WriteLine(request);
                        break;
                    case "обновление словаря":
                        //обновление словаря C:\Users\Эдуард\OneDrive - УрФУ\testfile1.txt
                        request = TextProcessor.UpdateLibrary(path);
                        Console.WriteLine(request);
                        break;
                    case "очистить словарь":
                        request = TextProcessor.DeleteLibrary();
                        Console.WriteLine(request);
                        break;
                    default:
                        request = TextProcessor.GetFilteredList(s.ToLower(), 5);
                        Console.WriteLine(request);
                        break;
                }
            }
        }

        private static string ReadLineOrEsc()
        {
            string retString = "";

            int curIndex = 0;
            do
            {
                ConsoleKeyInfo readKeyResult = Console.ReadKey(true);

                // handle Esc
                if (readKeyResult.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine();
                    return "";
                }
                // handle Enter
                if (readKeyResult.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    return retString;
                }
                // handle backspace
                if (readKeyResult.Key == ConsoleKey.Backspace)
                {
                    if (curIndex > 0)
                    {
                        retString = retString.Remove(retString.Length - 1);
                        Console.Write(readKeyResult.KeyChar);
                        Console.Write(' ');
                        //Console.Write(readKeyResult.KeyChar);

                        curIndex--;
                    }
                }

                retString += readKeyResult.KeyChar;
                Console.Write(readKeyResult.KeyChar);
                curIndex++;
            }
            while (true);
        }

        
        

    }
}
