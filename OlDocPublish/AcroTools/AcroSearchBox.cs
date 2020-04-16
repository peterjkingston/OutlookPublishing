using System;
using Acrobat;

namespace OlDocPublish.AcroTools
{
    
    class AcroSearchBox : CAcroRect
    {
        public short Left {get; set;}
        public short Top {get; set;}
        public short right {get; set;}
        public short bottom {get; set;}
        CAcroPDTextSelect _textSelect {get; set;}

        public AcroSearchBox (int top, int bottom, int left, int right)
        {
            this.Top = (short)top;
            this.bottom = (short)bottom;
            this.Left = (short)left;
            this.right = (short)right;
        }
    }
}