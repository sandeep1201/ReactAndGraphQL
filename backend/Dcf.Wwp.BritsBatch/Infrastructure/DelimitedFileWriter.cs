using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Dcf.Wwp.BritsBatch.Infrastructure
{
    /// <summary>
    ///     Class to write delimited files.
    /// </summary>
    public class DelimitedFileWriter : StreamWriter
    {
        public DelimitedFileWriter(Stream stream, string delimiter, Encoding encoding) : base(stream, encoding)
        {
            AutoFlush = true;
            Delimiter = delimiter;
        }

        public string Delimiter { get ; set ; }

        /// <summary>
        ///     Writes out a row of the Csv file.
        /// </summary>
        /// <param name="row"><code>Row</code> to write.</param>
        public void WriteRow(Row row)
        {
            WriteRow(row, true);
        }

        /// <summary>
        ///     Writes out a row of the Csv file. overloads to skip replacing quotes and delimiter values
        /// </summary>
        /// <param name="row"><code>Row</code> to write.</param>
        /// <param name="replaceDelimiter"></param>
        public void WriteRow(Row row, bool replaceDelimiter)
        {
            if (Delimiter == null)
            {
                throw new ArgumentNullException("Delimiter", "You must specify a delimiter.");
            }

            var builder = new StringBuilder();
            char cDelim;

            // set delimiter as a character.
            if (IsTextNumeric(Delimiter))
            {
                cDelim = Convert.ToChar(short.Parse(Delimiter));
            }
            else
            {
                cDelim = Convert.ToChar(Delimiter);
            }

            var value = string.Empty;

            for (var i = 0; i < row.Count; i++ )
            {
                value = row[i];

                // Add separator if this isn't the first value
                if (i > 0)
                {
                    builder.Append(cDelim);
                }

                if (replaceDelimiter)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        value = "";
                    }

                    if (value.IndexOfAny(new [] { '"', cDelim }) != -1)
                    {
                        // Special handling for values that contain comma or quote
                        // Enclose in quotes and double up any double quotes
                        builder.AppendFormat("\"{0}\"", value.Replace("\"", "\"\""));
                    }
                    else
                    {
                        builder.Append(value);
                    }
                }
                else
                {
                    builder.Append(value);
                }
            }

            row.LineText = builder.ToString();
            WriteLine(row.LineText);
        }

        /// <summary>
        ///     Duplicates the static helper method, as there's really no need for a reference to all that for one method :)
        /// </summary>
        /// <param name="strTextEntry"></param>
        /// <returns></returns>
        private bool IsTextNumeric(string strTextEntry)
        {
            var objNotWholePattern = new Regex("(^\\d+(\\.?\\d+){0,1}$)");
            return objNotWholePattern.IsMatch(strTextEntry);
        }
    }
}
