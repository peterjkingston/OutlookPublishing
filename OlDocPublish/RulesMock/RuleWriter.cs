using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace OlDocPublish.RulesMock
{
	public class RuleWriter<T> where T: IRuleCriteria
	{
		public void WriteRuleCriterium(string writePath, IEnumerable<T> ruleCriterias)
		{
			DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
			using (Stream stream = new FileStream(writePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				jsonSerializer.WriteObject(stream, ruleCriterias);
			}
		}
	}
}
