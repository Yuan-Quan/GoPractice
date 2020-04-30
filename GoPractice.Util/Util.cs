using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;

namespace GoPractice.MyUtil
{
public enum FileType
    {
        audio,
        image,
        video
    }

    public class Setting
    {
        public Setting(string key, string value,string description)
        {
            this.Key = key;
            this.Value = value;
            this.Description = description;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }

    public static class MyUtil
    {
        
        public static FileType GetFileType(string path)
        {
            string extName = path.Substring(path.LastIndexOf('.'), path.Length - 1 - path.LastIndexOf('.'));
            //switch ()
            //{
              //  default:
                //    break;
            //}
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// get settings!!
        /// </summary>
        /// <returns>Enumrable Settings</returns>
        public static IEnumerable<Setting> GetAllSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;

            foreach (var key in appSettings.AllKeys)
            {
                yield return new Setting(key, appSettings[key].Split(',')[0], appSettings[key].Split(',')[1]);
            }
        }

        /// <summary>
        /// Print current settings
        /// </summary>
        public static void ReadAllSettings()
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                if (appSettings.Count == 0)
                {
                    Console.WriteLine("AppSettings is empty.");
                }
                else
                {
                    Console.WriteLine("Current app settings: ");
                    foreach (var key in appSettings.AllKeys)
                    {
                        Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
                    }
                }
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
        }

        /// <summary>
        /// Read a setting by key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value of appSettings[key]</returns>
        public static string ReadSetting(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key] ?? null;
                return result;
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
                return null;
            }
        }

        /// <summary>
        /// Add a setting in appsettings
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void AddUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                    
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }

        /// <summary>
        /// Reading and Echoing the File
        /// <br></br>
        /// Each time the calling code requests the next item from the sequence, the code reads the next line of text from the file and returns it.
        /// </summary>
        /// <param name="file">Path of the file</param>
        /// <returns>string</returns>
        public static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using var reader = File.OpenText(file);
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        /// <summary>
        /// Return a string like Apr.3.2020
        /// </summary>
        /// <param name="dt">datetime</param>
        /// <returns>string of date</returns>
        public static string GetDateString(DateTime dt)
        {
            var month = dt.Month;
            var day = dt.Day;
            var year = dt.Year;


            return ""+MonthToString(month)+day+"."+year;
        }

        /// <summary>
        /// int to Jan. Feb. ...
        /// </summary>
        /// <param name="month">int month form 1 to 12</param>
        /// <returns>Jan. Feb. ...</returns>
        private static string MonthToString(int month)
        {
            return month switch
            {
                1 => "Jan.",
                2 => "Feb.",
                3 => "Mar.",
                4 => "Apr.",
                5 => "May",
                6 => "June",
                7 => "July",
                8 => "Aug.",
                9 => "Sept.",
                10 => "Oct.",
                11 => "Nov.",
                12 => "Dec.",
                _ => "err",
            };
        }

        private static int StringToMonth(string month)
        {
            return month switch
            {
                "Jan." => 1,
                "Feb." => 2,
                "Mar." => 3,
                "Apr." => 4,
                "May" => 5,
                "June" => 6,
                "July" => 7,
                "Aug." => 8,
                "Sept." => 9,
                "Oct." => 10,
                "Nov" => 11,
                "Dec." => 12,
                _ => -1,
            };
        }
        /// <summary>
        /// get the text info of checkbox
        /// </summary>
        /// <param name="str">uncheked single line</param>
        /// <returns>info text</returns>
        private static string GetBoxInfo(string str)
        {
            return str.Substring(GetBoxIndex(str) + 3, str.Length - GetBoxIndex(str) - 3);
        }

        /// <summary>
        /// check the checkbox
        /// </summary>
        /// <param name="str">single line string, contains only one checkbox</param>
        /// <returns>checked string</returns>
        private static string ChekCheckbox(string str)
        {
            var sb = new StringBuilder(str);
            sb[GetBoxIndex(str)] = 'x';
            return sb.ToString();
        }

        /// <summary>
        /// match a patter of checkbox
        /// </summary>
        /// <param name="str">string to match</param>
        /// <returns>index of box</returns>
        private static int GetBoxIndex(string str)
        {
            var match = Regex.Match(str, "- [ ]");
            return match.Index + 3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool IsCheckbox(string str)
        {
            return str.Contains("- [ ]");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static IEnumerable<string> TODOEdit(string fileName)
        {
            Console.WriteLine();

            Console.WriteLine("Now editing Checkboxes");
            Console.Write("Use ");
            
            ConsoleColor preForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\"yes [y]\" ");
            Console.ForegroundColor = preForegroundColor;

            preForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\"no [n]\" ");
            Console.ForegroundColor = preForegroundColor;

            Console.Write("to set checkbox state");
            
            Console.WriteLine();

            foreach (var line in MyUtil.ReadFrom($@"{MyUtil.ReadSetting("path").Split(',')[0]}/src/records/{fileName}"))
            {
                if (MyUtil.IsCheckbox(line))
                {
                    
                    while (true)
                    {
                        preForegroundColor = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(MyUtil.GetBoxInfo(line));
                        Console.ForegroundColor = preForegroundColor;
                        Console.Write(" -> ");

                        string ck = Console.ReadLine().Trim();
                        if ((ck == "yes") || (ck == "y"))
                        {
                            yield return MyUtil.ChekCheckbox(line);
                            break;
                        }
                        else if ((ck == "no") || (ck == "n"))
                        {
                            yield return line;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("unknow option");
                        }
                    }

                    Console.WriteLine();
                    
                }
                else
                {
                    yield return line;
                    continue;
                }
            }

            
        }
        
        public static void WriteAFile(List<string> ls, string path, string fileName)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(path, fileName)))
            {
                foreach (string line in ls)
                outputFile.WriteLine(line);
            }
            Console.WriteLine();
            var preForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("File Write Succeed!!");
            Console.ForegroundColor = preForegroundColor;
        }

        private static bool IsRow(string str)
        {
            return str.Contains(" | ")&&(!(str.Contains("------------")||str.Contains("Header")));
        }

        private static string GetRowInfo(string str)
        {
            return str.Split(" | ")[1];
        }

        private static string GetRowWkd(string str)
        {
            return (str.Split(" | ")[0]).Substring(2,3);
        }


        private static int StringToWkd(string jpWkd)
        {
            return jpWkd switch
            {
                "日曜日" => 0,
                "月曜日" => 1,
                "火曜日" => 2,
                "水曜日" => 3,
                "木曜日" => 4,
                "金曜日" => 5,
                "土曜日" => 6,
                _ => -1,
            };
        }

        private static string WkdToString(int DayOfWeek)
        {
            return DayOfWeek switch
            {
                0 => "日曜日",
                1 =>"月曜日" ,
                2 =>"火曜日" ,
                3 =>"水曜日" ,
                4 =>"木曜日" ,
                5 =>"金曜日" ,
                6 =>"土曜日" ,
                _ => "err",
            };
        }

        public static DateTime GetLatestDate()
        {
            var s = new List<string>(MyUtil.ReadFrom($@"{MyUtil.ReadSetting("path").Split(',')[0]}/README.md"));
            for (int i = s.Count - 1; i >= 0; i--)
            {
                if(s[i].Contains("From")||s[i].Contains("to"))
                {
                    return LineToDt(s[i]);
                }else
                {
                    continue;
                }
            }
            throw new Exception("No latest date found in README");
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime LineToDt(string str)
        {
            var date = str.Substring(str.IndexOf("From")+5, (str.IndexOf("to")-str.IndexOf("From")-5));
            return(DtStrToDt(date));
        }

        public static DateTime DtStrToDt(string str)
        {
            var month = str.Substring(0,str.IndexOfAny("0123456789".ToCharArray()));
            var str1 = str.Substring(str.IndexOfAny("0123456789".ToCharArray()), str.Length-str.IndexOfAny("0123456789".ToCharArray()));
            var day = str1.Split(".")[0];
            var year = str1.Split(".")[1];

            var m = StringToMonth(month);
            Int32.TryParse(day, out int d);
            Int32.TryParse(year, out int y);

            return new DateTime(y, m, d);
        } 

        public static string GenerateAListRow(string jpWkd, string path)
        {
            return $"{jpWkd} | __[Done](/src/record/{path})__";
        }

        //will return the corresponding line of date in README
        public static int GetLineOfDate(DateTime dt)
        {
            var firstDay = StartOfWeek(dt, DayOfWeek.Sunday);
            var s = new List<string>(MyUtil.ReadFrom($@"{MyUtil.ReadSetting("path").Split(',')[0]}/README.md"));
            var lineOfWkStart = -1;
            for (int i = s.Count - 1; i >= 0; i--)
            {
                if(s[i].Contains("From")||s[i].Contains("to"))
                {
                    if(LineToDt(s[i])==firstDay){
                        lineOfWkStart = i+1;
                        break;
                    }else
                    {
                        continue;
                    }
                }
            }
            if (lineOfWkStart == -1)
            {
                System.Console.WriteLine();
                System.Console.WriteLine("No required date in README.md\nwill creat it");
                throw new NotImplementedException();
            }

            //System.Console.WriteLine(lineOfWkStart);

            int dayOffset = -1;
            DateTime tempDt;
            for (int i = lineOfWkStart; i <= lineOfWkStart+10; i++)
            {
                if (IsRow(s[i]))
                {
                    dayOffset = StringToWkd(GetRowWkd(s[i]));
                }
                tempDt = firstDay.AddDays(dayOffset);
                if (tempDt.Equals(dt))
                {
                    return i+1;
                }else
                {
                    continue;
                }
            }
            throw new Exception("DATE NO FOUND!!");
        }
    }
}
