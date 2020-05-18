using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Helpers.Array;
using Microsoft.Office.Interop.Outlook;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OlDocPublish.DataProviders;

namespace OlDocPublishTest.DataProviders
{
	[TestClass]
	public class Test_SAPTextDataReader
	{
		string validSamplePath = Path.GetFullPath("sampleSAPData.txt");
		string invalidSamplePath = "non-existent File.txt";

		[TestMethod]
		public void ValidSamplePath_Found()
		{
			//Arrange
			bool expected = true;

			//Act
			bool actual = File.Exists(validSamplePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void InvalidSamplePath_NotFound()
		{
			//Arrange
			bool expected = false;

			//Act
			bool actual = File.Exists(invalidSamplePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetRaw_ParsesRows_GivenValidSamplePath()
		{
			//Arrange
			SAPTextDataReader reader = new SAPTextDataReader();
			string[,] raw = reader.GetRaw(validSamplePath);
			int expectedRowCount = 4;

			//Act
			int actualRowCount = raw.GetLength(0);

			//Assert
			//throw new Exception($"Actual row count: {actualRowCount}, FileExists? {File.Exists(validSamplePath)}");
			Assert.AreEqual(expectedRowCount,actualRowCount);
		}

		[TestMethod]
		public void GetRaw_ParsesColumns_GivenValidSamplePath()
		{
			//Arrange
			SAPTextDataReader reader = new SAPTextDataReader();
			string[,] raw = reader.GetRaw(validSamplePath);
			int expectedColumnCount = 4;

			//Act
			int actualColumnCount = raw.GetLength(1);

			//Assert
			Assert.AreEqual(expectedColumnCount, actualColumnCount);
		}

		[TestMethod]
		public void GetRaw_ReturnsEmpty2DArray_GivenInvalidSamplePath()
		{
			//Arrange
			SAPTextDataReader reader = new SAPTextDataReader();
			string[,] raw = reader.GetRaw(invalidSamplePath);
			bool expected = false;
			bool actual = false;

			//Act
			foreach(string member in raw)
			{
				if(member != "")
				{
					actual = true;
				}
			}

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetRecordset_ReturnsRawEquivilantDictionary()
		{
			//Arrange
			SAPTextDataReader reader = new SAPTextDataReader();
			string[,] raw = reader.GetRaw(validSamplePath);
			Dictionary<string,string[]> dict = reader.GetRecordset(validSamplePath);

			bool expected = true;
			bool actual = true;
			

			//Act
			for(int rawRow = 1; rawRow < raw.GetLength(0); rawRow++)
			{
				string[] dictRecord = dict[raw[rawRow, 0]];
				for (int rawColumn = 0; rawColumn < raw.GetLength(1); rawColumn++)
				{
					if(raw[rawRow,rawColumn] != dictRecord[rawColumn])
					{
						actual = false;
						string msg = $"{raw[rawRow, rawColumn]} != {dictRecord[rawColumn]}\n " + dictRecord.ConcatAll(" | ");
						throw new System.Exception(msg);
						break;
					}
				};
			}


			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
