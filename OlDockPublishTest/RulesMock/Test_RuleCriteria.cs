using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.Processors;
using OlDocPublish.RulesMock;

namespace OlDockPublishTest.RulesMock
{
	[TestClass]
	public class Test_RuleCriteria
	{
		#region CLASS ARRANGE
		// -- BEGIN CLASS ARRANGE --//

		IDocumentProcessor _processor => Factory.GetProcessor();
		Application _app => Factory.GetOutlookApplication();

		private RuleCriteria GetDefaultCriteria()
		{
			RuleCriteria criteria = new RuleCriteria(_app, _processor);
			{
				criteria.Property = RuleProperty.Subject;
				criteria.Condition = RulePropertyCondition.EqualTo;
				criteria.Validation = new string[] { "Test" };

				criteria.ResultingAction = new OlDocPublish.RulesMock.RuleAction[] { OlDocPublish.RulesMock.RuleAction.MoveToFolder };
				criteria.ActionArg = "Test Folder";
			}

			return criteria;
		}

		// -- END CLASS ARRANGE --//
		#endregion CLASS ARRANGE

		[TestMethod]
		public void Match_FindsEqual_Subject()
		{
			//Arrange
			MailItem testMail = Factory.GetTestMail(_app);
			MailItem_ID mailID = new MailItem_ID(testMail);
			RuleCriteria criteria = GetDefaultCriteria();
			{
				//No changes
			}
			bool expected = true;

			//Act
			bool actual = criteria.Match(mailID);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Match_FindsContains_Subject()
		{
			//Arrange
			MailItem testMail = Factory.GetTestMail(_app);
			MailItem_ID mailID = new MailItem_ID(testMail);
			RuleCriteria criteria = GetDefaultCriteria();
			{
				criteria.Condition = RulePropertyCondition.Contains;
				criteria.Validation = new string[] { "es" };
			}
			bool expected = true;

			//Act
			bool actual = criteria.Match(mailID);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Match_FindsNotEqual_Subject()
		{
			//Arrange
			MailItem testMail = Factory.GetTestMail(_app);
			MailItem_ID mailID = new MailItem_ID(testMail);
			RuleCriteria criteria = GetDefaultCriteria();
			{
				criteria.Condition = RulePropertyCondition.NotEqualTo;
				criteria.Validation = new string[] { "es" };
			}
			bool expected = true;

			//Act
			bool actual = criteria.Match(mailID);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Match_FindsNotContains_Subject()
		{
			//Arrange
			MailItem testMail = Factory.GetTestMail(_app);
			MailItem_ID mailID = new MailItem_ID(testMail);
			RuleCriteria criteria = GetDefaultCriteria();
			{ 
				criteria.Condition = RulePropertyCondition.DoesNotContain;
				criteria.Validation = new string[] { "Hammock" };
			}
			bool expected = true;

			//Act
			bool actual = criteria.Match(mailID);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void DoAction_MoveToFolder_MovesMail()
		{
			//Arrange
			string uuid = System.Guid.NewGuid().ToString();
			MailItem testMail = Factory.GetTestMail(_app);;
			testMail.Body = uuid;
			testMail.Save();
			MailItem_ID mailID = new MailItem_ID(testMail);
			RuleCriteria criteria = GetDefaultCriteria();
			{
				criteria.ResultingAction = new OlDocPublish.RulesMock.RuleAction[] { OlDocPublish.RulesMock.RuleAction.MoveToFolder };
				criteria.ActionArg = "Test Folder";
			}
			
			Folder targetFolder = _app.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent.Folders["Test Folder"];
			bool expected = true;
			bool actual = false;

			//Act
			criteria.DoAction(mailID);

			//Assert
			foreach(MailItem mail in targetFolder.Items)
			{
				if(mail.Body.Contains(uuid)) { actual = true; }
			}

			if(targetFolder == null ||
				targetFolder.Items.Count == 0) 
			{ Assert.Fail(); }

			Assert.AreEqual(expected, actual);

		}

		[TestMethod]
		public void DoAction_ProcessPDF_Triggers()
		{
			//Arrange
			RuleCriteria ruleCriteria = GetDefaultCriteria();
			{
				ruleCriteria.ResultingAction = new OlDocPublish.RulesMock.RuleAction[]{ OlDocPublish.RulesMock.RuleAction.ProcessPDF};
			}
			MailItem mail = Factory.GetTestMail(_app);
			MailItem_ID mailID = new MailItem_ID(mail);
			string beforeProcess = _processor.SO;

			//Act
			ruleCriteria.DoAction(mailID);
			string afterProcess = _processor.SO;

			//Assert
			Assert.AreNotEqual(beforeProcess,afterProcess);
		}
	}
}
