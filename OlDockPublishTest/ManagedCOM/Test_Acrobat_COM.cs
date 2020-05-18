using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish;
using OlDocPublish.ManagedCOM;

namespace OlDockPublishTest.ManagedCOM
{
	[TestClass]
	public class Test_Acrobat_COM
	{
		string nondefined_ValidPDF_path = Path.GetFullPath("non-defined document.pdf");

		[TestMethod]
		public void OpenApplication_ReturnsNotNull()
		{
			//Arrange
			Acrobat_COM acrobat_COM = new Acrobat_COM();
			object notExpected = null;

			//Act
			object actual = acrobat_COM.OpenApplication();

			//Assert
			Assert.AreNotEqual(notExpected, actual);
		}

		[TestMethod]
		public void OpenNewPDDoc_ReturnsNotNull()
		{
			//Arrange
			Acrobat_COM acrobat_COM = new Acrobat_COM();
			object notExpected = null;

			//Act
			object actual = acrobat_COM.OpenNewPDDoc(nondefined_ValidPDF_path);
			
			//Assert
			Assert.AreNotEqual(notExpected, actual);
		}


		[TestMethod]
		public void OpenNewPDDoc_KeepsSuccesiveDocs()
		{
			//Arrange
			Acrobat_COM acrobat_COM = new Acrobat_COM();
			int itemCount = 5;
			for(int i = 0; i < itemCount; i++) { acrobat_COM.OpenNewPDDoc(nondefined_ValidPDF_path); }
			int expectedPDFCount = 5;
			int actualPDFCount = 0;

			//Act
			for(int i = 0; i < itemCount; i++)
			{
				actualPDFCount += acrobat_COM.PDDocs[i] != null ? 1 : 0;
			}

			//Assert
			Assert.AreEqual(expectedPDFCount, actualPDFCount);
		}
	}
}
