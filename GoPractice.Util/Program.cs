using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using CommandDotNet;
using GoPractice.Util;

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

            if (File.Exists(@$"{path}/src/records/templates.md"))
            {
                File.Delete(@$"{path}/src/records/templates.md");
                Console.Write($"{path}/src/records/templates.md exitsted, will ");
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("DELETE");
                Console.ForegroundColor = preForegroundColor;
                Console.Write(" it.");
                Console.WriteLine();
            }

            try
            {
                File.Copy(@$"{path}/src/templates/DailyReport.md", @$"{path}/src/records/test01.md");
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
