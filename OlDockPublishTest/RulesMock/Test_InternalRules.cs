using System;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.RulesMock;

namespace OlDockPublishTest.RulesMock
{
	[TestClass]
	public class Test_InternalRules
	{
		#region CLASS ARRANGE
		//-- BEGIN CLASS ARRANGE --//


		//-- END CLASS ARRANGE --//
		#endregion CLASS ARRANGE

		[TestMethod]
		public void Process_DoesNotError()
		{
			//Arrange
			IRuleReader ruleReader = Factory.GetRuleReader();
			InternalRules internalRules = new InternalRules(ruleReader, Factory.RuleReader_Path);

			Application app = Factory.GetOutlookApplication();
			MailItem mail = Factory.GetTestMail(app);
			bool expected = false;
			bool actual = false;

			//Act
			try
			{
				internalRules.Process(mail);
			}
			catch
			{
				actual = true;
			}


			//Assert
			Assert.AreEqual(expected, actual);
		}


	}
}
