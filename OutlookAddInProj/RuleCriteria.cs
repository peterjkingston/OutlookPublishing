using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace OutlookAddInController
{
	public class RuleCriteria : IRuleCriteria, ISerializable
	{
		public bool Match(MailItem mail)
		{
			throw new NotImplementedException();
		}

		public void Action(MailItem mail)
		{
			throw new NotImplementedException();
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
			
		}
	}
}
