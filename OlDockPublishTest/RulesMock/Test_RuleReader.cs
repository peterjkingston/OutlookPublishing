using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDockPublishTest;
using OlDocPublish.RulesMock;

namespace OlDockPublish.RulesMock
{
	[TestClass]
	public class Test_RuleReader
	{
		RuleReader _ruleReader = new RuleReader();

		[TestMethod]
		public void GetCriteria_ReturnsValidRules()
		{
			//Arrange
			string path = Factory.RuleReader_Path;

			//Act
			List<IRuleCriteria> ruleCriterias = _ruleReader.GetCriteria(path);

			//Assert
			Assert.IsNotNull(ruleCriterias[0]);
		}
	}
}
