using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OlDocPublish.RulesMock
{
	public class InternalRules : IInternalRules
	{
		readonly static string CRITERIA_PATH = $"ExtensionCriteria.json";
		private readonly List<IRuleCriteria> _criterium;

		public InternalRules(IRuleReader ruleReader)
		{
			_criterium = ruleReader.GetCriteria(CRITERIA_PATH);
		}

		public void Process(Outlook.MailItem mail)
		{
			//TODO: Lock the email if possible
			try
			{
				if (mail.Sender != null)
				{
					foreach (IRuleCriteria criteria in _criterium)
					{
						MailItem_ID mailID = new MailItem_ID(mail);
						if (criteria.Match(mailID))
						{
							Task.Run(() => criteria.DoAction(mailID));
						}
					}
				}
			}
			catch (System.Exception)
			{
				throw;
			}
		}
	}
}