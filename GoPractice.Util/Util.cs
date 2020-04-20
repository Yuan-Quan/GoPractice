using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;

namespace GoPractice.MyUtil
{
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
        public static /*IEnumerable<string>*/ void TODOEdit(string fileName)
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
                    
                    inner: while (true)
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
        
    }
}
