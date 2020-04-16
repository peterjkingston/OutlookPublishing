using System;
using Acrobat;

namespace OlDocPublish.AcroTools
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

        private static object GetAcrobatObject(string objectName)
        {
            Type AcrobatType = Type.GetTypeFromProgID($"AcroExch.{objectName}");
            return Activator.CreateInstance(AcrobatType);
        }
    }
}