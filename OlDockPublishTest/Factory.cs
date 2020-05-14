using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OlDockPublishTest.Fakes;
using OlDocPublish.DataProviders;
using OlDocPublish.Processors;
using OlDocPublish.RulesMock;

namespace OlDockPublishTest
{
	static class Factory
	{
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
			if(_app == null)
			{
				Type applicationType = Type.GetTypeFromProgID($"Outlook.Application");
				_app = (Application)Activator.CreateInstance(applicationType);
			}

			return _app;
		}

		public static IRuleReader GetRuleReader()
		{
			return new RuleReader();
		}

		internal static SAPOrderDataProvider GetSAPOrderDataProvider()
		{
			throw new NotImplementedException();
		}

		private static Application _app;
		private static IDocumentProcessor _processor;

		public static string RuleReader_Path = "test_rule_to_read.json";
		public static string RuleWriter_Path = "test_rule_to_write.json";
	}
}
