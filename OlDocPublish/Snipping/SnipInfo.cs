namespace OlDocPublish.Snipping
{
    public struct SnipInfo
    {
        public string SO {get;}
        public string DocumentType {get;}
        public int Start {get;}
        public int End {get;}
        public bool EOF {get;}
        public int DocPageCount {get;}

        public SnipInfo(string so,
                        string documentType,
                        int docPageCount,
                        int start,
                        int end,
                        bool eof)
        {
            this.SO = so;
            this.DocumentType = documentType;
            this.DocPageCount = docPageCount;
            this.Start = start;
            this.End = end;
            this.EOF = eof;
        }
    }
}