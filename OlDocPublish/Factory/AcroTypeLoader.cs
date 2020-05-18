using System;
using Acrobat;

namespace OlDocPublish.Factory
{
    public static class TypeLoader
    {
        public static CAcroApp GetApp()
        {
            return (CAcroApp)GetAcrobatObject("App");
        }

        internal static CAcroAVDoc GetAVDoc()
        {
            return (CAcroAVDoc)GetAcrobatObject("AVDoc");
        }

        public static CAcroRect GetRect()
        {
            return (CAcroRect)GetAcrobatObject("Rect");
        }

        public static CAcroPDDoc GetPDDoc()
        {
            return (CAcroPDDoc)GetAcrobatObject("PDDoc");
        }

        private static object GetAcrobatObject(string objectName)
        {
            Type AcrobatType = Type.GetTypeFromProgID($"AcroExch.{objectName}");
            return Activator.CreateInstance(AcrobatType);
        }
    }
}