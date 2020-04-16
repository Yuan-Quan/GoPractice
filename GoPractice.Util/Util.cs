using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoPractice.Util
{
    public static class Util
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
    }
}
