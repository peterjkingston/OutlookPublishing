using Acrobat;

namespace OlDocPublish.Factory
{
    public class Factory : IAcrobatTypeProvider
    {
        public CAcroAVDoc GetAcroAVDoc()
        {
            return TypeLoader.GetAVDoc();
        }
    }
}