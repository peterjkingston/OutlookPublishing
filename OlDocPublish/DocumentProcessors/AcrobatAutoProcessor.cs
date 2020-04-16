using System;
using System.Collections.Generic;
using Microsoft.Office.Interop.Outlook;
using Acrobat;
using OlDocPublish.Snipping;


namespace OlDocPublish
{
    public class AcrobatAutoProcessor : IDisposable, IDocumentProcessor
    {
        ///<summary>
        ///
        ///</summary>
        public event EventHandler<SnipInfo> SnipRequested;

        ///<summary>
        ///
        ///</summary>
        public event EventHandler<SnipInfo> SnipIterated;

        ///<summary>
        ///
        ///</summary>
        public event EventHandler SnippingComplete;

        ///<summary>
        ///
        ///</summary>
        public event EventHandler<SnipInfo> SnipAfter;

        ///<summary>
        ///
        ///</summary>
        public event EventHandler<SnipInfo> BeforeSnipping;

        //Acrobat Specific
        CAcroApp _app {get; set;} = null;
        CAcroPDDoc _pdDoc {get; set;} = null;
        CAcroAVDoc _avDoc {get; set;} = null;
        
        IPostOCR _postOCR {get; set;} = null;

        //Outlook Specific
        public MailItem Mail {get; private set;} = null;
        public string SO {get; private set;} = "";
        
        AcrobatAutoProcessor(CAcroApp app, IPostOCR postOCR)
        {
            _app = app;
            _postOCR = postOCR;
        }

        ~AcrobatAutoProcessor()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool itIsSafeToAlsoFreeManagedObjects)
        {
            _pdDoc.Close();
            _avDoc.Close(-1);

            _app.Hide();
            _app.CloseAllDocs();
            _app = null;

            if(itIsSafeToAlsoFreeManagedObjects)
            {
                //Free any pointers, etc. here.
            }
        }

        ///<summary>
        ///Returns the PDDoc object associated with this instance.
        ///</summary>
        private CAcroPDDoc GetPDDocFromFile(string directory)
        {
            _avDoc = (CAcroAVDoc)_app.GetAVDoc(0);
            if(_avDoc.Open(directory,"autoTemp") != 0)
            {
                CAcroPDDoc result = (CAcroPDDoc)_avDoc.GetPDDoc();
                return result;
            }
            else
            {
                return null;
            }
        }

        ///<summary>
        ///Returns a Dictionary object describing the start of each new subdocument.
        ///</summary>
        public void GetDocPageTypes(out List<string> docPageTypes, out List<int> docPages)
        {
            docPages = new List<int>();
            docPageTypes = new List<string>();

            _pdDoc = GetPDDocFromFile("");
            int pageCount = _pdDoc.GetNumPages();
            string docType = "";
            string lastDocType = docType;
    
            for(int page = 0; page < pageCount; page++)
            {
                bool skipThis = false;
                
                switch (_postOCR.GetPageID(page))
                {
                    case 0:
                        docType = "Delivery Note";
                        break;
                    case 1:
                        docType = "Bill of Lading";
                        break;
                    case 2:
                        docType = "Certificate of Analysis";
                        break;
                    case 3:
                        docType = "Seal Manifest";
                        break;
                    default:
                        skipThis = true;
                        break;
                }

                skipThis = docType == lastDocType || skipThis;
                if(!skipThis)
                {
                    docPages.Add(page + 1);
                    docPageTypes.Add(docType);
                }
            }
        }

        ///<summary>
        ///TODO
        ///</summary>
        public bool ProcessMailItem(MailItem mail)
        {
            Mail = mail;
            List<string> docPageTypes;
            List<int> docPages;
            GetDocPageTypes(out docPageTypes, out docPages);
            SO = _postOCR.GetSO();

            int lastPage = docPageTypes.Count -1;
            int nextPageStart;
            bool success = false;

            try
            {
                for(int snip = 0; snip < docPageTypes.Count; snip++)
                {
                    int[] snips = new int[docPageTypes.Count];

                    
                    if(snip == docPageTypes.Count)
                    {
                        nextPageStart = -1;
                    }
                    else
                    {
                        nextPageStart = docPages[snip];
                    }
                    string docType = docPageTypes[snip];
                    int docPage = docPages[snip];

                    SnipInfo info = new SnipInfo
                    (
                        SO,
                        docType,
                        docPageTypes.Count,
                        docPage,
                        nextPageStart,
                        snip == docPageTypes.Count-1
                    );

                    OnBeforeSnipping(info);
                    OnSnipRequested(info);
                    OnSnipAfter(info);
                    OnSnipIterated(info);
                }
                OnSnippingComplete(new EventArgs());
                success = true;
            }
            catch(System.Exception ex)
            {
                Console.WriteLine(ex);
                throw;   
            }

            return success;
        }

        protected virtual void OnBeforeSnipping(SnipInfo e)
        {
            BeforeSnipping(this, e);
        }

        protected virtual void OnSnipRequested(SnipInfo e)
        {
            SnipRequested(this, e);
        }

        protected virtual void OnSnipAfter(SnipInfo e)
        {
            SnipAfter(this, e);
        }

        protected virtual void OnSnipIterated(SnipInfo e)
        {
            SnipIterated(this, e);
        }

        protected virtual void OnSnippingComplete(EventArgs e)
        {
            SnippingComplete(this, e);
        }

    }
}
