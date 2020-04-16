using System;
using Microsoft.Office.Interop.Outlook;

namespace OutlookAddInController
{
	public interface IRuleCriteria
	{
		Action<MailItem, string[]> Action { get; set; }
		Func<MailItem, bool> Match { get; set; }
	}
}