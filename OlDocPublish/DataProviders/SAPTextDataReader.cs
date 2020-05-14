using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Helpers.Array;

namespace OlDocPublish.DataProviders
{
    
    public class SAPTextDataReader : IDataReader
    {
        /// <summary>
        /// Reads the specified text file, returns a 2-dimensional string array of rows, then columns.
        /// </summary>
        public string[,] GetRaw(string readPath)
        {
            List<string[]> dataList = new List<string[]>();
            string[,] raw = new string[,] { { "" }, { "" } };

            try
            {
                string[] textBody = File.ReadAllLines(readPath);

                foreach(string line in textBody)
                {
                    if(line.Length > 0 && line[0] == DataAccess.Delimiter)
                    {
                        IEnumerable<string> data = line.Split(DataAccess.Delimiter).Skip(1);

                        dataList.Add(data.Take(data.Count() - 1).ToArray());
                    }
                }

                int rows = dataList.Count;
                int cols = dataList.Count != 0? dataList[0].Length: 0;
                raw = new string[rows, cols];

                for (int row = 0; row < rows; row++)
                {
                    for (int col = 0; col < cols; col++)
                    {
                        raw[row, col] = dataList[row][col].Trim();
                    }
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {
                
            }
            
            return raw;
        }

        /// <summary>
        /// Reads the specified text file, returns a dictionary of each record, by document number.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> GetRecordset(string sourcePath)
        {
            string[,] raw = GetRaw(sourcePath);
            int fieldCount = raw.GetLength(1);
            int rowCount = raw.GetLength(0);
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();

            for(int row = 0; row < rowCount; row++)
            {
                string key = raw[row, 0];
                for (int field = 0; field < fieldCount; field++)
                {
                    if(!result.ContainsKey(key))
                    {
                        string[] data = raw.GetRow(row);
                        result.Add(key, data);
                    }
                }
            }

            return result;
        }
    }
}