using System;
using System.IO;

namespace OlDocPublish.Logging
{

    public class SalesOrderLogger : ISalesOrderLogger
    {
        public SalesOrderLogger(IDocumentProcessor processor)
        {
            processor.SnippingComplete += Processor_OnSnippingComplete;
        }

        private void Processor_OnSnippingComplete(object sender, EventArgs e)
        {
            IDocumentProcessor processor = (IDocumentProcessor)sender;
            LogSalesOrder(processor.SO);
        }

        public void LogSalesOrder(string sO)
        {
            string logFilePath = Resources.Paths.TodaysSOs;
            DateTime fileDate;
            using (FileStream stream = File.Open(logFilePath, FileMode.OpenOrCreate))
            {
                using (TextReader textReader = new StreamReader(stream))
                {
                    fileDate = DateTime.Parse(textReader.ReadLine());
                }
            }

            if (fileDate != DateTime.Today)
            {
                using (FileStream stream = File.Open(logFilePath, FileMode.Create))
                {
                    using (TextWriter textWriter = new StreamWriter(stream))
                    {
                        textWriter.WriteLine(DateTime.Today.ToString());
                    }
                }
            }

            using (FileStream stream = File.Open(logFilePath, FileMode.Append))
            {
                using (TextWriter textWriter = new StreamWriter(stream))
                {
                    textWriter.WriteLine(sO);
                }
            }
        }
    }
}