using System.Collections.Generic;
using OlDocPublish.DataProviders;

namespace OlDocPublishTest.Fakes
{
	internal class Fake_DataReader : IDataReader
	{
		public string[,] GetRaw(string sourcePath)
		{
			return new string[3,2] 
			{
				{ "SalesDocument", "SomeFieldName" },
				{ "123456", "Some BS data" },
				{ "123457", "Some other BS data" },
			};
		}

		public Dictionary<string, string[]> GetRecordset(string sourcePath)
		{
			Dictionary<string, string[]> result = new Dictionary<string, string[]>();
			string[,] rawData = GetRaw(sourcePath);

			string[] record = new string[] { rawData[0, 0], rawData[0, 1] };
			result.Add("Fields", record);

			for(int row = 1; row < rawData.GetLength(0); row++)
			{
				record = new string[rawData.GetLength(1)];

				for(int column = 0; column < record.Length; column++)
				{
					record[column] = rawData[row, column];
				}

				result.Add(rawData[row, 0], record);
			}


			return result;
		}
	}
}