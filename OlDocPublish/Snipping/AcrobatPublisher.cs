using System;
using System.IO;
using Acrobat;
using Helpers.IO;
using OlDocPublish.Factory;
using OlDocPublish.Processors;
using OlDocPublish.DataProviders;

namespace OlDocPublish.Snipping
{
    public class AcrobatPublisher : IPublisher, IDisposable
    {
        private CAcroAVDoc _avDoc;
        private CAcroPDDoc _pDDoc;
        private IAcrobatTypeProvider _pdfProvider;

        public event EventHandler<string> FileCreated;
        
        public AcrobatPublisher(IDocumentProcessor processor, IAcrobatTypeProvider pdfProvider)
        {
            processor.SnipRequested += Processor_OnSnipRequested;
            _pdfProvider = pdfProvider;
        }

        private void Processor_OnSnipRequested(object o, SnipInfo snipInfo)
        {
            Publish(DataProviders.Paths.TempPDFPath, snipInfo.SO, $"{snipInfo.SO} - {snipInfo.DocumentType}", snipInfo.Start, snipInfo.End, snipInfo.EOF);
        }

        public void Publish(string tempFilePath, string so,string label, int startPageNum, int endPageNum, bool eof = false)
        {
            _pDDoc = GetPDDocFromFile(tempFilePath);
            int lenPages = GetEndPages(eof, endPageNum) - startPageNum;

            try
            {
                if(_pDDoc != null)
                {
                    string fullPath = GetPublishDirectory(so, label);
                    string copyPath = GetCopyDirectory(so, label);
                    FileCreated += this_OnFileCreated;
                    CreateFile(startPageNum, lenPages, _pDDoc, fullPath, so);
                }
            }
            catch
            {

            }
            finally
            {
                _pDDoc.Close();
            }
        }

        private void this_OnFileCreated(object o, string originalSavePath)
        {
            string so = Path.GetDirectoryName(originalSavePath).GetParentDirectoryName();
            string label = Path.GetFileNameWithoutExtension(originalSavePath);
            string copyDir = GetCopyDirectory(so, label);
            
            System.IO.File.Copy(originalSavePath, copyDir);
        }

        private void OnFileCreated(string fullPath)
        {
            if(FileCreated != null) FileCreated(this, fullPath);
        }

        private string GetCopyDirectory(string so, string label)
        {
            string pubDir = Path.Combine(DataProviders.Paths.RemoteFileStorage, so);
            if(!Directory.Exists(pubDir)) Directory.CreateDirectory(pubDir);

            return Path.Combine(pubDir, $"{label}.pdf");
        }

        private void CreateFile(int startPageNum, int lenPages, CAcroPDDoc fromDoc, string saveToDirectory, string so)
        {
            CAcroAVDoc newAVDoc = _pdfProvider.GetAcroAVDoc();
            CAcroPDDoc newPDDoc = (CAcroPDDoc)newAVDoc.GetPDDoc();


            try
            {
                newPDDoc.Create();
                newPDDoc.InsertPages(-1, fromDoc, startPageNum - 1, lenPages, 0);
                OnFileCreated(saveToDirectory);
            }
            catch
            {

            }
            finally
            {
                newPDDoc?.Close();
                newAVDoc?.Close(-1);
            }
        }

        private string GetPublishDirectory(string so, string label)
        {
            string pubDir = Path.Combine(DataProviders.Paths.FileStorage, so);
            if(!Directory.Exists(pubDir)) Directory.CreateDirectory(pubDir);

            return Path.Combine(pubDir, $"{label}.pdf");
        }

        private int GetEndPages(bool eof, int endPageNum)
        {
            if(eof)
            {
                return _pDDoc.GetNumPages() + 1;
            }
            else
            {
                return endPageNum;
            }
        }

        private CAcroPDDoc GetPDDocFromFile(string tempFilePath)
        {
            _avDoc = _pdfProvider.GetAcroAVDoc();
            if(Convert.ToBoolean(_avDoc.Open(tempFilePath, Headers.GetTemporaryPDF())))
            {
               return (CAcroPDDoc)_avDoc.GetPDDoc(); 
            }
            else
            {
                return null;
            }
        }

        public void Dispose()
        {
            _pDDoc?.Close();
            _avDoc?.Close(-1);

            _pDDoc = null;
            _pDDoc = null;
        }
    }
}