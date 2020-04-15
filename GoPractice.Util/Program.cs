using System;
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


        }
           
    }
}
