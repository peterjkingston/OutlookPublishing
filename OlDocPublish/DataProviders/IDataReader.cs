using System.Collections.Generic;

namespace OlDocPublish.DataProviders
{
    public interface IDataReader
    {
        Dictionary<string, string[]> GetRecordset();
        string[,] GetRaw();
    }
}