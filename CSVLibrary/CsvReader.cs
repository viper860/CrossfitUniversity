using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic.FileIO;

namespace Library
{
    public class CsvReader
    {
        #region Public members

        public List<CsvFieldTarget> CsvFieldTargets { get; private set; }
        public List<CsvRow> CsvRowsResult { get; private set; }
        public char Separator { get; private set; }
        public bool IsFirstLineColumnName { get; private set; }
        public string FilePath { get; private set; }

        #endregion

        #region Constructors
        public CsvReader(string filePath, List<CsvFieldTarget> csvFieldTargets, char separator, bool isFirstLineColumnName)
        {
            FilePath = filePath;
            CsvFieldTargets = csvFieldTargets;
            Separator = separator;
            IsFirstLineColumnName = isFirstLineColumnName;
            CsvRowsResult = new List<CsvRow>();
        }

        #endregion

        #region Public methods

        public List<CsvRow> ReadCsvRows()
        {
            CsvRowsResult = IsFirstLineColumnName ? GetCsvRowIterator().Skip(1).ToList() : GetCsvRowIterator().ToList();
            return CsvRowsResult;
        }

        #endregion

        #region Private methods

        private IEnumerable<CsvRow> GetCsvRowIterator()
        {
            //  using (StreamReader readFile = new StreamReader(FilePath))
            //  {
            //TextFieldParser parser = new TextFieldParser(new StringReader(csv));
            TextFieldParser parser = new TextFieldParser(FilePath);
            //parser.HasFieldsEnclosedInQuotes = true;
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;
            //parser.TrimWhiteSpace = true;
            //string line;
            string[] row;
            //string[] seps = { "\",", ",\"" };
            //char[] quotes = { '\"', ' ' };

            while (!parser.EndOfData)
            {
                try
                {
                    row = parser.ReadFields();
                }
                catch (Microsoft.VisualBasic.FileIO.MalformedLineException ex)
                {
                    // not a valid delimited line - log, terminate, or ignore
                    continue;
                }
                var resultRow = BuildCsvRow(row);
                yield return resultRow;
                //foreach (string field in fields)
                //{
                //    Console.WriteLine(field);
                //}
            }

            //while ((line = readFile.ReadLine()) != null)
            //{
            //    //row = line.Split(Separator);
            //    row = line.Split(seps, StringSplitOptions.None).Select(s => s.Trim(quotes).Replace("\\\"", "\"")).ToArray();
            //    var resultRow = BuildCsvRow(row);
            //    yield return resultRow;
            //}
            // }
        }


        private CsvRow BuildCsvRow(string[] row)
        {
            var resultRow = new CsvRow(CsvFieldTargets.Count);
            foreach (var csvField in CsvFieldTargets)
            {
                var field = new CsvFieldResult()
                {
                    FieldName = csvField.FieldName,
                    Position = csvField.Position,
                    FieldValue = row[csvField.Position]
                };
                resultRow.CsvFieldsResult.Add(field);
            }

            return resultRow;

        }

        #endregion

    }
}
