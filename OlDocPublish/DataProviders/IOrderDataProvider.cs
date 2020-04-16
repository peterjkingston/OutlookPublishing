namespace OlDocPublish.DataProviders
{
    public interface IOrderDataProvider
    {
        string[] FieldNames {get;}
        bool Exists(string field, string so);
        string GetData(string field, string so);
        string GetData(string field, string so, string indexField);
    }
}