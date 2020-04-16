using System.Collections.Generic;

namespace OlDocPublish
{
    public interface IDataReader
    {
        Dictionary<string, string[]> GetRecordset();
        string[,] GetRaw();
    }
}