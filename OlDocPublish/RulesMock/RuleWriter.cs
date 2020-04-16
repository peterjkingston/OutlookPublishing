using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OutlookAddInController;
using System.Runtime.Serialization.Json;
using System.IO;

namespace OlDocPublish.RulesMock
{
	public class RuleWriter
	{
		public void WriteRuleCriterium(string writePath, IEnumerable<IRuleCriteria> ruleCriterias)
		{
			DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(IEnumerable<IRuleCriteria>));
			using (Stream stream = new FileStream(writePath, FileMode.OpenOrCreate))
			{
				jsonSerializer.WriteObject(stream, ruleCriterias);
			}
		}
	}
}
