using System;
using System.IO;

namespace Resources
{
    public class ResourceRetreiver : IResourceRetreiver
    {
        public string GetPathOrderData() { return Paths.OrderData; }
        public string GetPathFileStorage() { return Paths.FileStorage; }
        public string GetPathTempPDFPath() { return Paths.TempPDFPath; }
        public string GetPathFileStorageRemote() { return Paths.RemoteFileStorage; }
        public string GetPathProcessPool() { return Paths.TodaysSOs; }
        public char GetDelimiter() { return DataAccess.Delimiter; }
        public string GetHeaderTempPDF() { return Headers.GetTemporaryPDF(); }

    }

    public static class Paths
    {
        public static string OrderData {get =>  Path.Combine(FileStorage, "Admin Parts");}

        public static string FileStorage {get => "C:\\Users\\" + System.Environment.UserName +"\\Desktop\\File Storage Local";}
        public static string TempPDFPath { get; internal set; }
        public static string RemoteFileStorage { get; internal set; }
        public static string TodaysSOs { get; internal set; }
    }

    public static class DataAccess
    {
        public static char Delimiter {get => '|';}
    }

    public static class Headers
    {
        internal static string GetTemporaryPDF()
        {
            throw new NotImplementedException();
        }
    }
}