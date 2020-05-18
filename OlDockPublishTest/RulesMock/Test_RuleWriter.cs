using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublishTest;
using OlDocPublish.RulesMock;
using RuleAction = OlDocPublish.RulesMock.RuleAction;

namespace OlDocPublishTest.RulesMock
{
	[TestClass]
	public class Test_RuleWriter
	{
		RuleWriter<RuleCriteria> _ruleWriter = new RuleWriter<RuleCriteria>();

		[TestMethod]
		public void WriteRuleCriterium_Writes()
		{
			//Arrange
			string path = Factory.RuleWriter_Path;
			DateTime lastWriteTime = File.GetLastWriteTime(path);

			RuleCriteria ruleCriteria = new RuleCriteria(null, null);
			{
				ruleCriteria.Property = RuleProperty.Subject;
				ruleCriteria.Condition = RulePropertyCondition.EqualTo;
				ruleCriteria.Validation = new string []{ "Test" };

				ruleCriteria.ResultingAction = new RuleAction[]{ RuleAction.MoveToFolder };
				ruleCriteria.ActionArg = "Test";

			}
			List<RuleCriteria> ruleList = new List<RuleCriteria>(new RuleCriteria[] { ruleCriteria });

			//Act
			_ruleWriter.WriteRuleCriterium(path, ruleList);
			DateTime thisWriteTime = File.GetLastWriteTime(path);

			//Assert
			Assert.AreNotEqual(lastWriteTime, thisWriteTime);
		}


	}
}
