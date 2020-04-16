using System;
using Microsoft.Office.Interop.Outlook;

namespace OlDocPublish.RulesMock
{
	public interface IRuleCriteria
	{
		Action<MailItem, string[]> Action { get; set; }
		Func<MailItem, bool> Match { get; set; }
	}
}