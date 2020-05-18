using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.Processors;

namespace OlDocPublishTest.DocumentProcessors
{
	[TestClass]
	public class Test_AcrobatAutoProcessor
	{
		[TestMethod]
		public void ProcessMailItem_ReturnsTrue_GivenMailwithPDFAttachment()
		{
			//Arrange
			AcrobatAutoProcessor processor = new AcrobatAutoProcessor(Factory.GetAcroApp(), Factory.GetPostOCR());

			//Act

			//Assert
			Assert.Inconclusive();
		}
	}
}
