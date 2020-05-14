using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.DataProviders;

namespace OlDockPublishTest.DataProviders
{
	[TestClass]
	public class Test_SAPOrderDataProvider
	{
		IDataReader _reader = new Fakes.Fake_DataReader();

		[TestMethod]
		public void Exists_ReturnsFalse_WhenFieldNotPresent()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "123456";
			string fieldName = "Not a field";
			bool expected = false;

			//Act
			bool actual = provider.Exists(fieldName, so); ;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Exists_ReturnsFalse_WhenSONotPresent()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "Not an SO";
			string fieldName = "SomeFieldName";
			bool expected = false;

			//Act
			bool actual = provider.Exists(fieldName, so); ;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Exists_ReturnsTrue_WhenFieldIsPresent()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "123456";
			string fieldName = "SomeFieldName";
			bool expected = true;

			//Act
			bool actual = provider.Exists(fieldName, so); ;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Exists_ReturnsTrue_WhenSOIsPresent()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "123456";
			string fieldName = "SomeFieldName";
			bool expected = true;

			//Act

			bool actual = provider.Exists(fieldName, so); ;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetData_ReturnsInfo_GivenValidLookup()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "123456";
			string fieldName = "SomeFieldName";
			string expected = "Some BS data";

			//Act
			string actual = provider.GetData(fieldName, so);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetData_ReturnsEmptyString_GivenInvalidLookup()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string so = "123456";
			string fieldName = "INVALID FIELD NAME";
			string expected = "";

			//Act
			string actual = provider.GetData(fieldName, so);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetData_ReturnsInfo_GivenValidAlternateLookup()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string key = "Some other BS data";
			string selectedField = "SalesDocument";
			string indexField = "SomeFieldName";
			string expected = "123457";

			//Act
			string actual = provider.GetData(selectedField, key, indexField);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetData_ReturnsEmptyString_GivenInvalidAlternateLookup()
		{
			//Arrange
			SAPOrderDataProvider provider = new SAPOrderDataProvider(_reader);
			string key = "Some INVALID data";
			string fieldName = "SalesDocument";
			string lookupField = "SomeFieldName";
			string expected = "";

			//Act
			string actual = provider.GetData(fieldName, key, lookupField);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
