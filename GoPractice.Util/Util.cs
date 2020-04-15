using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoPractice.Util
{
    public class Util
    {
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

        public static string GetDateString(DateTime dt)
        {
            var month = dt.Month;
            var day = dt.Day;
            var year = dt.Year;


            return ""+MonthToString(month)+day+"."+year;
        }


        private static string MonthToString(int month)
        {
            switch (month)
            {
                case 1:
                    return "Jan.";
                    break;
                case 2:
                    return "Feb.";
                    break;
                case 3:
                    return "Mar.";
                    break;
                case 4:
                    return "Apr.";
                    break;
                case 5:
                    return "May";
                    break;
                case 6:
                    return "June";
                    break;
                case 7:
                    return "July";
                    break;
                case 8:
                    return "Aug.";
                    break;
                case 9:
                    return "Sept.";
                    break;
                case 10:
                    return "Oct.";
                    break;
                case 11:
                    return "Nov.";
                    break;
                case 12:
                    return "Dec.";
                    break;
                default:
                    return "err";
                    break;
            }
        } 
    }
}
