using Microsoft.Office.Interop.Outlook;

namespace OlDocPublish.RulesMock
{
	public struct MailItem_ID
	{
		public string Value { get; private set; }
		public MailItem_ID(MailItem mail)
		{
			Value = mail.EntryID;
		}

		public MailItem_ID(string EntryID)
		{
			Value = EntryID;
		}
	}
}