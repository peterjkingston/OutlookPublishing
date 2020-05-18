using System;
using System.Collections.Generic;
using System.Linq;

namespace OlDocPublish.DataProviders
{
    public class SAPOrderDataProvider : IOrderDataProvider
    {
        Dictionary<string, string[]> _recordset;
        List<string> _fields;
        string[,] _raw;

        public SAPOrderDataProvider(IDataReader dataReader, string sourcePath)
        {
            _recordset = dataReader.GetRecordset(sourcePath);
            _raw = dataReader.GetRaw(sourcePath);
            _fields = _recordset.ContainsKey("Fields")?new List<string>(_recordset["Fields"]):new List<string>(new string[0]);
        }

        public string[] FieldNames {get => _fields.ToArray();}

        public bool Exists(string field, string so)
        {
            bool result = false;

            if (_recordset.ContainsKey(so))
            {
                if (_fields.Contains(field))
                {
                    int fieldIndex = _fields.FindIndex((x) => x == field);
                    result = _fields[fieldIndex] != "";
                }
            }

            return result;
        }

        public string GetData(string field, string so)
        {
            string result = "";

            if(Exists(field, so))
            {
                int fieldIndex = _fields.FindIndex((x)=> x == field);
                result = _recordset[so][fieldIndex];
            }

            return result.Trim();
        }

        public string GetData(string selectField, string key, string indexField)
        {
            string result = "";

            if(_fields.Contains(selectField))
            {
                int fieldIndex = _fields.FindIndex((x) => x == indexField);
                int dataIndex = _fields.FindIndex((x) => x == selectField);

                int rowCount = _raw.GetLength(0);
                for(int row = 0; row < rowCount; row++)
                {
                    result = _raw[row, fieldIndex] == key? _raw[row, dataIndex]: "";
                    if(result != ""){break;}
                }
            }
           
            return result;
        }
    }
}