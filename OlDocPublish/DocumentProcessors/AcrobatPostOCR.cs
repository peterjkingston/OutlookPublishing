using System;
using System.Text.RegularExpressions;
using Acrobat;
using OlDocPublish.DataProviders;

namespace OlDocPublish.Processors
{
    public class AcrobatPostOCR : IPostOCR
    {
        //Future plans to move this to an external definition.
        private enum PageID
        {
            csUNKDocument = -1,
            csDeliveryNote,
            csInternalBOL,
            csCertificateAnalysis,
            csSealManifest
        }

        const string DELIVERY_NOTE = "DELIVERY NOTE";
        const string INTERNAL_BOL = "BILL OF LADING";
        const string CERT_ANALYSIS = "Date: ??/??/??";
        const string SEAL_MANIFEST = "DRUM SEAL MANIFEST";
        private IOrderDataProvider _dataProvider;
        private CAcroPDDoc _pdDoc;

        public AcrobatPostOCR(IOrderDataProvider dataProvider, CAcroPDDoc pDDoc)
        {
            _dataProvider = dataProvider;
            _pdDoc = pDDoc;
            
        }

        ///<summary>
        ///
        ///</summary>
        public int GetPageID(int page)
        {
            int id;
            double threshold = 0.5;

            if(Levenshtein.EditDistance.IsCloseTo(SearchAround(715,690,262,378, page, DELIVERY_NOTE, threshold), DELIVERY_NOTE, threshold))
            {
                id = (int)PageID.csDeliveryNote;
            }
            else if(Levenshtein.EditDistance.IsCloseTo(SearchAround(1000, 720, 400, 500, page, INTERNAL_BOL, threshold), INTERNAL_BOL, threshold))
            {
                id = (int)PageID.csInternalBOL;
            }
            else if(Levenshtein.EditDistance.IsCloseTo(SearchAround(1000,625,450,460, page, CERT_ANALYSIS, 0.28), CERT_ANALYSIS, 0.28))
            {
                id = (int)PageID.csCertificateAnalysis;
            }
            else if(Levenshtein.EditDistance.IsCloseTo(SearchAround(1000, 719, 286, 365, page, SEAL_MANIFEST, threshold), SEAL_MANIFEST, threshold))
            {
                id = (int)PageID.csSealManifest;
            }
            else
            {
                id = (int)PageID.csUNKDocument;
            }

            return id;
        }

        ///<summary>
        ///
        ///</summary>
        public string GetSO()
        {
            Regex exp = new Regex(@"\s*\d*{7}s*");
            int pageCount = _pdDoc.GetNumPages();
            int id;
            string SO = "";

            for(int page = 0; page < pageCount; page++)
            {
                id = GetPageID(page);
                switch ((PageID)id)
                {
                    case PageID.csDeliveryNote:
                        SO = SearchAroundRegex(680, 669, 124, 173, page, exp).TrimStart('0');    
                    break;
                    
                    case PageID.csInternalBOL:
                        SO = SearchAroundRegex(578, 565, 463, 504, page, exp).TrimStart('0'); 
                    break;
                        
                    case PageID.csCertificateAnalysis:
                        SO = SearchAroundRegex(538, 526, 421, 457, page, exp).TrimStart('0'); 
                    break;

                    default:
                        SO = "";
                    break;
                }
                if(ValidateSO(SO, _pdDoc, id)){break;}
            }
            
            return SO;
        }

        

        private string SearchAroundRegex(int top, int bottom, int left, int right, int page, Regex exp)
        {
            CAcroRect cAcroRect = Factory.TypeLoader.GetRect();
            cAcroRect.Top = (short)top;
            cAcroRect.bottom = (short)bottom;
            cAcroRect.Left = (short)left;
            cAcroRect.right = (short)right;


            string found = "";

            for(int instruction = 0; instruction < 5; instruction++)
            {
                switch (instruction)
                {
                    case 0:
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 1:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 2:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 3:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 4:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    default:
                        found = "";
                    break;
                }

                if(exp.IsMatch(found)){break;}
            }

            return found;
        }

        private bool ValidateSO(string so, CAcroPDDoc pdDoc, int id)
        {
            bool result = false;
            Regex isSO = new Regex("\\d{7}");
            Regex isTooLong = new Regex("\\d{8}");
            Regex notNumbers = new Regex("[a-z, 0, \n]*");
            string replaceClear = "";

            so = notNumbers.Replace(so, replaceClear);
            if(isSO.IsMatch(so) && !isTooLong.IsMatch(so))
            {
                if(_dataProvider.Exists("Name 1", so))
                {
                    string dataName1 = _dataProvider.GetData("Name 1", so);
                    int endPages = pdDoc.GetNumPages();
                    for(int page = 0; page < endPages; page++)
                    {
                        string readName1 = GetName1(GetPageID(page), page, so);
                        result = Levenshtein.EditDistance.IsCloseTo(readName1, dataName1, 0.5);
                        if(result){break;}
                    }
                }
            }

            return result;
        }

        private string GetName1(int id, int page, string so)
        {
            string result = "";
            PageID pageID = (PageID)id;

            if(_dataProvider.Exists("Document", so))
            {
                string expected = _dataProvider.GetData("Name 1", so);
                
                switch (pageID)
                {
                    case PageID.csDeliveryNote:
                        result = SearchAround(597, 585, 386, 600, page, expected, 0.5);
                        if(result.Substring(0,4) == "To: "){result = _dataProvider.GetData("Name 1", result.Substring(4,6), "Sold-to-pt");}
                    break;

                    case PageID.csInternalBOL:
                        result = SearchAround(671, 660, 394, 600, page, expected, 0.5);
                    break;

                    case PageID.csCertificateAnalysis:
                        result = SearchAround(654, 640, 33, 200, page, expected, 0.5);
                    break;

                    default:
                    break;
                }
            }

            return result;
        }

        private string SearchAround(int top, int bottom, int left, int right, int page, string expected, double threshold)
        {
            CAcroRect cAcroRect = Factory.TypeLoader.GetRect();
            cAcroRect.Top = (short)top;
            cAcroRect.bottom = (short)bottom;
            cAcroRect.Left = (short)left;
            cAcroRect.right = (short)right;


            string found = "";

            for(int instruction = 0; instruction < 5; instruction++)
            {
                switch (instruction)
                {
                    case 0:
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 1:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 2:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 3:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    case 4:
                        ScootRect(cAcroRect, 5, 0);
                        found = ReadRect(cAcroRect, page);
                    break;

                    default:
                        found = "";
                    break;
                }

                found = TrimAtLineBreak(found);
                if(Levenshtein.EditDistance.IsCloseTo(found, expected, threshold)){break;}
            }

            return found;
        }

        private void ScootRect(CAcroRect cAcroRect, int verticalShift, int horizontalShift)
        {
            cAcroRect.Top += (short)verticalShift;
            cAcroRect.bottom += (short)verticalShift;

            cAcroRect.Left += (short)horizontalShift;
            cAcroRect.right += (short)horizontalShift;
        }

        private string ReadRect(CAcroRect cAcroRect, int page)
        {
            CAcroPDTextSelect textSelect = (CAcroPDTextSelect)_pdDoc.CreateTextSelect(page, cAcroRect);
            if(textSelect == null)
            {
                return "";
            }
            else
            {
                string text = "";
                int endText = textSelect.GetNumText();
                for(int vector = 0; vector < endText; vector++)
                {
                    text += textSelect.GetText(vector);
                }

                return text;
            }
        }

        private string TrimAtLineBreak(string thisStr)
        {
            return thisStr.Split('\n')[0];
        }

    }
}