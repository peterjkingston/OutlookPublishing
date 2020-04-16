using Microsoft.Office.Interop.Outlook;

namespace OutlookAddInController
{
	public interface IRuleCriteria
	{
		void Action(MailItem mail);
		bool Match(MailItem mail);
	}
}