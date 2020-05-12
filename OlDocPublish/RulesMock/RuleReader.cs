using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace OlDocPublish.RulesMock
{
	public class RuleReader : IRuleReader
	{

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public List<IRuleCriteria> GetCriteria(string criteria_path)
		{
			List<IRuleCriteria> criterium = new List<IRuleCriteria>();
			IEnumerable<RuleCriteria> jsonObj;

			DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<RuleCriteria>));
			using (Stream stream = new FileStream(criteria_path, FileMode.OpenOrCreate))
			{
				jsonObj = (IEnumerable<RuleCriteria>)jsonSerializer.ReadObject(stream);
			}

			try
			{
				
				criterium = new List<IRuleCriteria>(jsonObj);
			}
			catch
			{
				throw;
			}

			return criterium;
		}
	}
}