using Acrobat;

namespace OlDocPublish
{
    public class Factory : IAcrobatTypeProvider
    {
        public CAcroAVDoc GetAcroAVDoc()
        {
            return AcroTools.TypeLoader.GetAVDoc();
        }
    }
}