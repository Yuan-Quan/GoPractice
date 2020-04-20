using System;
using System.IO;
using CommandDotNet;
using ConsoleTables;
using GoPractice.MyUtil;
using HeyRed.MarkdownSharp;

namespace GoPracticeCli
{
    class Program
    {
        static void Startup()
        {
            //MyUtil.ReadAllSettings();
        }
        static int Main(string[] args)
        {
            Startup();
            var m = new MainEntry();
            m.EditFile();
            //return new AppRunner<MainEntry>()
            //.UseDefaultMiddleware()
            //.Run(args);
            return 0;
        }
    }



    public class MainEntry
    {
        //to store the console color when color changes
        ConsoleColor preForegroundColor;

        //format of date time
        const string dataFmt = "{0,-30}{1}";

        [Command(Name = "new",
        Usage = "new [date]",
        Description = "creat a new report",
        ExtendedHelpText = "creat a new report,\nspecify [date] to customize the file name.\ncustom template or file path not implemented")]
        [Obsolete]
        public void CreatNewReport(
            [Option(ShortName = "d")]string date = null
            )
        {
            if (date == null)
            {
                Console.WriteLine("No date specifyed, using current date");
                //maybe print the current timezone 
                date = MyUtil.GetDateString(DateTime.Now);
            }
            
            Console.WriteLine($"new record will be named {date}.md");
            Console.WriteLine();
            // Get the local time zone and the current local time and year.
            var localZone = TimeZone.CurrentTimeZone;
            DateTime currentDate = DateTime.Now;
            Console.WriteLine("Your current time zone set to:");
            Console.WriteLine(dataFmt, "UTC offset:", localZone.GetUtcOffset(currentDate));
            Console.WriteLine();

            if (File.Exists(@$"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{date}.md"))
            {
                //file already exists

                Console.Write($"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/templates.md already exitsts, ");

                Console.WriteLine("\n");
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("[1]SKIP:");
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
                Console.Write(@$" {MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{date}.md");
                Console.WriteLine();

                File.Delete(@$"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{date}.md");

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
                    File.Copy(@$"{MyUtil.ReadSetting("path").Split(',')[0]}/src/templates/DailyReport.md", @$"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{date}.md");
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

        [Command(Name = "config",
        Usage = "config [opration] [key] [value]\nexample: config set path ~/GoPractice",
        Description = "view change/add settings",
        ExtendedHelpText = "opration: view set add remove\nformat: [opration] key value,description")]
        public void Config(
            string opration = null,
            string k = null,
            string v = null
            )
        {
            switch (opration)
            {
                case "v":
                case "view":
                    ConfigView(k);
                    break;
                case "set":
                case "s":
                    ConfigSet(k, v);
                    break;
                case "add":
                case "a":
                    ConfigAdd(k, v);
                    break;
                case "remove":
                case "rm":
                    ConfigRemove(k, v);
                    break;
                default:
                    Console.WriteLine("Usage: config [opration] [key] [value]\nexample: config set path ~/GoPractice");
                    break;
            }

            static void ConfigRemove(string k, string v)
            {
                Console.WriteLine("Too lazy to write it. Just goto App.config and delete it.");
                throw new NotImplementedException();
            }

            void ConfigAdd(string k, string v)
            {
                MyUtil.AddUpdateAppSettings(k, v);
                Console.WriteLine();
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Configuration added successfully!!");
                Console.ForegroundColor = preForegroundColor;
            }

            void ConfigSet(string k, string v)
            {
                MyUtil.AddUpdateAppSettings(k, v);
                Console.WriteLine();
                preForegroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Configuration modified successfully!!");
                Console.ForegroundColor = preForegroundColor;
            }

            static void ConfigView(string k)
            {
                if (k == null)
                {
                    //view all appsettings
                    var table = new ConsoleTable("key", "value", "description");
                    foreach (var setting in MyUtil.GetAllSettings())
                    {
                        table.AddRow(setting.Key, setting.Value, setting.Description);
                    }

                    Console.WriteLine();
                    Console.WriteLine(" Here all your configurations in App.config:");
                    table.Write();
                    Console.WriteLine();
                }
            }
        }

        [Command(Name = "select",
        Usage = "select [file name]",
        Description = "select working file",
        ExtendedHelpText = "select working file")]
        public void Select(
            string fileName = null
            )
        {
            switch (fileName)
            {
                case "today":
                case null:
                    Console.WriteLine("Selecting today's record");
                    fileName = MyUtil.GetDateString(DateTime.Now);
                    break;
                default:
                    if (File.Exists($@"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{fileName}.md"))
                    {
                        break;
                    }

                    Console.WriteLine($"No record named {fileName}.md. Have you type the right name?");
                    return;
            }

            fileName += ".md";
            Console.WriteLine($"\nWorking on file: {fileName}\n");

            MyUtil.AddUpdateAppSettings("WorkingOn", fileName + ",Current working file");
        }

        [Command(Name = "edit",
        Usage = "edit [fileName]",
        Description = "edit a report",
        ExtendedHelpText = "edit file")]
        public void EditFile(
            string fileName = null
            )
        {
            Console.WriteLine();
            if (fileName == null)
            {
                Console.WriteLine("No file specified, using selected file.");
                fileName = MyUtil.ReadSetting("WorkingOn").Split(',')[0];
            }
            Console.WriteLine($"Working on {fileName}...");
            Console.WriteLine();
            MyUtil.TODOEdit(fileName);
        }

        [Command(Name = "cat",
        Usage = "cat [file name]",
        Description = "print a report",
        ExtendedHelpText = "just like cat command in a bash")]
        public void Cat(
            string file = null
            )
        {
            if (file == null)
            {
                Console.WriteLine();
                Console.WriteLine("No file specified, Will read current file");
                file = MyUtil.ReadSetting("WorkingOn").Split(',')[0];
            }
            Console.WriteLine();
            
            foreach (var line in MyUtil.ReadFrom($@"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/"+file))
            {
                Console.WriteLine(line);
            }
        }
    }
}
