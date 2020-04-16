using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace OlDocPublish.RulesMock
{
	public class RuleReader : IRuleReader
	{
		public List<IRuleCriteria> GetCriteria(string criteria_path)
		{
			List<IRuleCriteria> criterium = new List<IRuleCriteria>();
			object jsonObj;

			DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<IRuleCriteria>));
			using (Stream stream = new FileStream(criteria_path, FileMode.OpenOrCreate))
			{
				jsonObj = jsonSerializer.ReadObject(stream);
			}

			try
			{
				criterium = new List<IRuleCriteria>((IRuleCriteria[])jsonObj);
			}
			catch
			{
				throw;
			}

			return criterium;
		}
	}
}