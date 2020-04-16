using Microsoft.Office.Interop.Outlook;

namespace OlDocPublish.RulesMock
{
	public interface IInternalRules
	{
		void Process(MailItem mail);
	}
}