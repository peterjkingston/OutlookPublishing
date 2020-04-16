using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutlookAddInController;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddInProj
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
						if (criteria.Match(mail))
						{
							Task.Run(()=>criteria.Action(mail, new string[] { }));
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