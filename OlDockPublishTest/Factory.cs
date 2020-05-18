using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acrobat;
using Microsoft.Office.Interop.Outlook;
using OlDocPublishTest.Fakes;
using OlDocPublish.DataProviders;
using OlDocPublish.Factory;
using OlDocPublish.Processors;
using OlDocPublish.RulesMock;
using System.Diagnostics;
using OlDockPublishTest.Fakes;

namespace OlDocPublishTest
{
	static class Factory
	{
		public static MailItem GetTestMail_withPDFAttachment(Application app, string pdfSourcePath)
		{
			MailItem mail = GetTestMail(app);
			mail.Attachments.Add(pdfSourcePath);

			return mail;
		}

		internal static CAcroPDDoc GetAcroPDDoc(string pdfSource)
		{
			CAcroAVDoc aVDoc = GetAcroAVDoc();
			if(aVDoc.Open(pdfSource, "autoTemp"))
			{
				return aVDoc.GetPDDoc();
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// Retrieves an unmanaged instance of Acrobat
		/// </summary>
		/// <returns></returns>
		internal static CAcroApp GetAcroApp()
		{
			return OlDocPublish.Factory.TypeLoader.GetApp();
		}

		internal static CAcroAVDoc GetAcroAVDoc()
		{
			CAcroApp app = GetAcroApp();
			CAcroAVDoc avDoc = app.GetAVDoc(0);
			return avDoc;
		}

		internal static IPostOCR GetPostOCR()
		{
			return new Fake_PostOCR();
		}

		public static MailItem GetTestMail(Application app)
		{
			MailItem mail = app.CreateItem(OlItemType.olMailItem);
			{
				mail.To = "someone@consoto.com";
				mail.Subject = "Test";
				mail.Body = "Some plain text for this body!";
			}

			mail.Save();

			return mail;
		}

		internal static IDocumentProcessor GetProcessor()
		{
			if(_processor == null)
			{
				_processor = new Fake_DocumentProcessor();
			}

			return _processor;
		}

		public static Application GetOutlookApplication()
		{
			if (_app == null)
			{
				Process[] processes = Process.GetProcessesByName("OUTLOOK");
				if (processes.Length != 0)
				{ 
					Type applicationType = Type.GetTypeFromProgID($"Outlook.Application");
					_app = (Application)Activator.CreateInstance(applicationType);
				}
			}

			return _app;
		}

		public static IRuleReader GetRuleReader()
		{
			return new RuleReader();
		}

		internal static SAPOrderDataProvider GetSAPOrderDataProvider()
		{
			return new SAPOrderDataProvider(new Fakes.Fake_DataReader(), "");
		}

		private static Application _app;
		private static IDocumentProcessor _processor;

		public static string RuleReader_Path = "test_rule_to_read.json";
		public static string RuleWriter_Path = "test_rule_to_write.json";
	}
}
