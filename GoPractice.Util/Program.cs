using System;
using System.IO;
using CommandDotNet;

namespace GoPractice.Util
{
    class Program
    {
        static int Main(string[] args)
        {
            return new AppRunner<MainEntry>()
            .UseDefaultMiddleware()
            .Run(args);
        }
    }



    public class MainEntry
    {
        //the path to the root of GoPracice
        readonly string path = "/mnt/c/Users/yq/source/repos/GoPractice";

        //to store the console color when color changes
        ConsoleColor preForegroundColor;

        [Command(Name = "new",
        Usage = "new [date]",
        Description = "creat a new report",
        ExtendedHelpText = "creat a new report,\nspecify [date] to customize the file name.\ncustom template or file path not implemented")]
        public void CreatNewReport(
            [Option(ShortName = "d")]string date = null
            )
        {
            if (date == null)
            {
                Console.WriteLine("No date specifyed, using current date");
                //maybe print the current timezone 
                date = Util.GetDateString(DateTime.Now);
            }
            
            Console.WriteLine($"new record will be named {date}.md");

            if (File.Exists(@$"{path}/src/records/{date}.md"))
            {
                //file already exists

                Console.Write($"{path}/src/records/templates.md already exitsts, ");

                Console.WriteLine("\n");
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[1]SKIP");
                Console.ForegroundColor = preForegroundColor;
                Console.Write(" break this opreation.");
                Console.WriteLine();

                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("[2]DELETE:");
                Console.ForegroundColor = preForegroundColor;
                Console.Write(" will delete it.");
                Console.WriteLine();

                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("[3]OVERWRITE:");
                Console.ForegroundColor = preForegroundColor;
                Console.Write(" will delete it, then creat a new one.");
                Console.WriteLine();

                while (true)
                {

                    Console.Write("Select a option -> ");

                    if (Int32.TryParse(Console.ReadLine(), out int result))
                    {
                        switch (result)
                        {
                            case 2:
                                DeleteExist();
                                return;
                            case 1:
                                Console.WriteLine("will stop opreation 'new'.");
                                return;
                            case 3:
                                DeleteExist();
                                GenerateReport();
                                return;
                            default:
                                Console.WriteLine("No this option!!");
                                continue;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not a valid input!!");
                        continue;
                    }

                }
            }
            else
            {
                //no exist file

                GenerateReport();

                return;
            }

            //Delete duplicate file
            void DeleteExist()
            {
                Console.Write($"Will ");
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("DELETE");
                Console.ForegroundColor = preForegroundColor;
                Console.Write(@$" {path}/src/records/{date}.md");
                Console.WriteLine();

                File.Delete(@$"{path}/src/records/{date}.md");

                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Done!");
                Console.ForegroundColor = preForegroundColor;
                Console.WriteLine();
            }

            //New a report
            void GenerateReport()
            {
                try
                {
                    File.Copy(@$"{path}/src/templates/DailyReport.md", @$"{path}/src/records/{date}.md");
                }
                catch (DirectoryNotFoundException)
                {
                    preForegroundColor = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("WRONG PATH!! Have you set the right path in app.config???");
                    Console.ForegroundColor = preForegroundColor;
                    Console.WriteLine();
                    throw;
                }


                ConsoleColor preConsoleColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"File {date}.md generated successfully");
                Console.ForegroundColor = preConsoleColor;
                Console.WriteLine();
            }
        }

    }
}
