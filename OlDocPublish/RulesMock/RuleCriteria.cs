using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;

namespace OutlookAddInController
{
	public class RuleCriteria : IRuleCriteria
	{
		public Action<MailItem, string[]> Action { get; set; }
		public Func<MailItem, bool> Match { get; set; }
	}
}
