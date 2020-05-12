using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;

namespace OlDocPublish.RulesMock
{
	/// <summary>
	/// Throws runtime error if T is not a concrete type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class RuleWriter<T>
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
