using System;
using System.Collections.Generic;
using System.Linq;

namespace OlDocPublish.SAPTools
{
    public class SAPOrderDataProvider : IOrderDataProvider
    {
        Dictionary<string, string[]> _recordset;
        List<string> _fields;
        string[,] _raw;

        public SAPOrderDataProvider(IDataReader dataReader)
        {
            _recordset = dataReader.GetRecordset();
            _raw = dataReader.GetRaw();
            _fields = _recordset.ContainsKey("Fields")?new List<string>(_recordset["Fields"]):new List<string>(new string[0]);
        }

        public string[] FieldNames {get => _fields.ToArray();}

        public bool Exists(string field, string so)
        {
            bool result = false;

            if(_recordset.ContainsKey(so))
            {
                if(_fields.Contains(field))
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

        public string GetData(string field, string key, string indexField)
        {
            string result = "";

            if(_fields.Contains(field))
            {
                int fieldIndex = _fields.FindIndex((x) => x == field);
                int dataIndex = _fields.FindIndex((x) => x == field);

                int rowCount = _raw.GetLength(1);
                for(int row = 0; row < rowCount; row++)
                {
                    result = _raw[fieldIndex, row] == key? _raw[dataIndex, row]: "";
                    if(result != ""){break;}
                }
            }

            return result;
        }
    }
}