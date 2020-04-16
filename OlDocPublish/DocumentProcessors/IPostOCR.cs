namespace OlDocPublish.Processors
{
    public interface IPostOCR
    {
        int GetPageID(int page);
        string GetSO();
    }
}