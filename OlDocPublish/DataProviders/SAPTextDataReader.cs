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
        public string[,] GetRaw()
        {
            List<string[]> dataList = new List<string[]>();

            using(Stream stream = File.Open(Path.Combine(Paths.OrderData,"USOrders.txt"),FileMode.Open))
            {
                using(TextReader tReader = new StreamReader(stream))
                {
                    while(stream.Position != stream.Length)
                    {
                        if((char)tReader.Peek() == DataAccess.Delimiter)
                        {
                            IEnumerable<string> data = tReader.ReadLine().Split(DataAccess.Delimiter).Skip(1);

                            dataList.Add(data.Take(data.Count() - 1).ToArray());
						}
						else
						{
                            tReader.ReadLine();
						}
                    }
                }
                
            }

            int rows = dataList.Count;
            int cols = dataList[0].Length;
            string[,] raw = new string [rows, cols];
            
            for(int row = 0; row < rows; row++)
            {
                for(int col = 0; col < cols; col++)
                {
                    raw[row, col] = dataList[row][col].Trim();
                }
            }
            return raw;
        }

        /// <summary>
        /// Reads the specified text file, returns a 2-dimensional string array of rows, then columns.
        /// </summary>
        public string[,] GetRaw(string readPath)
        {
            List<string[]> dataList = new List<string[]>();

            using (Stream stream = File.Open(readPath, FileMode.Open))
            {
                using (TextReader tReader = new StreamReader(stream))
                {
                    while (stream.Position != stream.Length)
                    {
                        if ((char)tReader.Peek() == DataAccess.Delimiter)
                        {
                            IEnumerable<string> data = tReader.ReadLine().Split(DataAccess.Delimiter).Skip(1);

                            dataList.Add(data.Take(data.Count() - 1).ToArray());
                        }
                        else
                        {
                            tReader.ReadLine();
                        }
                    }
                }

            }

            int rows = dataList.Count;
            int cols = dataList[0].Length;
            string[,] raw = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    raw[row, col] = dataList[row][col].Trim();
                }
            }
            return raw;
        }

        /// <summary>
        /// Reads the specified text file, returns a dictionary of each record, by document number.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string[]> GetRecordset()
        {
            string[,] raw = GetRaw();
            int fieldCount = raw.GetLength(1);
            int rowCount = raw.GetLength(0);
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();

            for(int field = 0; field < fieldCount; field++)
            {
                for(int row = 0; row < rowCount; row++)
                {
                    string key = raw[row,field];
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