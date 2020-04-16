using System;
using System.Collections.Generic;
using System.IO;
using Helpers.Array;

namespace OlDocPublish.DataProviders
{
    public class SAPTextDataReader : IDataReader
    {
        public string[,] GetRaw()
        {
            List<string[]> dataList = new List<string[]>();

            using(Stream stream = File.Open(Paths.FileStorage,FileMode.Open))
            {
                using(TextReader tReader = new StreamReader(stream))
                {
                    while(stream.CanRead)
                    {
                        if((char)tReader.Peek() == DataAccess.Delimiter)
                        {
                            string[] data = tReader.ReadLine().Split(DataAccess.Delimiter);
                            dataList.Add(data);
                        }
                    }
                }
                
            }

            int rows = dataList.Count;
            int cols = dataList[0].Length;
            string[,] raw = new string [cols, rows];
            
            for(int row = 0; row < rows; row++)
            {
                for(int col = 0; col < cols; col++)
                {
                    raw[col,row] = dataList[row][col];
                }
            }
            return raw;
        }

        public Dictionary<string, string[]> GetRecordset()
        {
            string[,] raw = GetRaw();
            int fieldCount = raw.GetLength(0);
            int rowCount = raw.GetLength(1);
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();

            for(int field = 0; field < fieldCount; field++)
            {
                for(int row = 0; row < rowCount; row++)
                {
                    string key = raw[field,row];
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