using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
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

		public static Application GetOutlookApplication()
		{
			Type applicationType = Type.GetTypeFromProgID($"Outlook.Application");
			return (Application)Activator.CreateInstance(applicationType);
		}

		public static IRuleReader GetRuleReader()
		{
			return new RuleReader();
		}

		public static string RuleReader_Path = "test_rule_to_read.json";
		public static string RuleWriter_Path = "test_rule_to_write.json";
	}
}
