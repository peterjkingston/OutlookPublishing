using Microsoft.Office.Interop.Outlook;

namespace OutlookAddInProj
{
	public interface IInternalRules
	{
		void Process(MailItem mail);
	}
}