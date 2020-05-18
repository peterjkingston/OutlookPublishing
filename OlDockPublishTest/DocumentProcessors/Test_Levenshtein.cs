using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Levenshtein;

namespace OlDocPublishTest.DocumentProcessors
{
	[TestClass]
	public class Test_Levenshtein_EditDistance
	{
		[TestMethod]
		public void IsCloseTo_ReturnsTrue_GivenIdenticalStringsThreshold1()
		{
			//Arrange
			string first = "Some string!";
			string second = "Some string!";
			double threshold = 1.00;

			bool expected = true;

			//Act
			bool actual = EditDistance.IsCloseTo(first, second, threshold);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsCloseTo_ReturnsFalse_GivenNonidenticalStringsThreshold1()
		{
			//Arrange
			string first = "Some string!";
			string second = "Some string";
			double threshold = 1.00;

			bool expected = false;

			//Act
			bool actual = EditDistance.IsCloseTo(first, second, threshold);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsCloseTo_ReturnsTrue_GivenSimilarStringsThreshold0_5()
		{
			//Arrange
			string first = "Some string!";
			string second = "Tome tring!";
			double threshold = 0.50;

			bool expected = true;

			//Act
			bool actual = EditDistance.IsCloseTo(first, second, threshold);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsCloseTo_ReturnsFalse_GivenInsimilarStringsThreshold0_5()
		{
			//Arrange
			string first = "Some string!";
			string second = "Elephant Mongoose!";
			double threshold = 0.50;

			bool expected = false;

			//Act
			bool actual = EditDistance.IsCloseTo(first, second, threshold);

			//Assert
			if(expected != actual)
			{
				int distance = EditDistance.GetEditDistance(first, second);
				throw new Exception($"Edit distance was {distance}");
			}
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetEditDistance_Returns0_GivenIdenticalStrings()
		{
			//Arrange
			string first = "Some string!";
			string second = "Some string!";

			int expected = 0;

			//Act
			int actual = EditDistance.GetEditDistance(first, second);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetEditDistance_Returns5_GivenLongerStringTop()
		{
			//Arrange
			string first = "Some string!12345";
			string second = "Some string!";

			int expected = 5;

			//Act
			int actual = EditDistance.GetEditDistance(first, second);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetEditDistance_Returns5_GivenLongerStringBottom()
		{
			//Arrange
			string first = "Some string!";
			string second = "Some string!12345";

			int expected = 5;

			//Act
			int actual = EditDistance.GetEditDistance(first, second);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
