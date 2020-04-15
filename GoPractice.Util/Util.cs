using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GoPractice.Util
{
    class Util
    {
        /// <summary>
        /// Reading and Echoing the File
        /// <br?
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
    }
}
