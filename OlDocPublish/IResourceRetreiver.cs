namespace Resources
{
	public interface IResourceRetreiver
	{
		char GetDelimiter();
		string GetHeaderTempPDF();
		string GetPathFileStorage();
		string GetPathFileStorageRemote();
		string GetPathOrderData();
		string GetPathProcessPool();
		string GetPathTempPDFPath();
	}
}