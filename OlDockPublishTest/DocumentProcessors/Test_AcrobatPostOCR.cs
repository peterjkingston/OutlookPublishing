using System;
using System.IO;
using Acrobat;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.DataProviders;
using OlDocPublish.ManagedCOM;
using OlDocPublish.Processors;

namespace OlDocPublishTest.DocumentProcessors
{
	//[TestClass]
	public class Test_AcrobatPostOCR
	{
		string nondefined_ValidPDF_path = Path.GetFullPath("non-defined document.pdf");
		IOrderDataProvider dataProvider = Factory.GetSAPOrderDataProvider();

		//[TestMethod]
		public void GetPageID_ReturnsNeg1_GivenNotRecognizedPDFPage()
		{
			//Arrange
			Acrobat_COM acrobat_COM = new Acrobat_COM(nondefined_ValidPDF_path);
			CAcroPDDoc pDDoc = acrobat_COM.PDDocs[0];

			AcrobatPostOCR postOCR = new AcrobatPostOCR(dataProvider, pDDoc);
			MailItem mail = Factory.GetTestMail_withPDFAttachment
			(
				Factory.GetOutlookApplication(),
				nondefined_ValidPDF_path		
			);
			int firstPage = 1;
			
			int expected = -1;


			//Act
			int actual = postOCR.GetPageID(firstPage);


			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetPageID_ReturnsZeroPlus_GivenRecognizedPDFPage()
		{
			//Arrange


			//Act


			//Assert
			Assert.Inconclusive();
		}
	}
}
