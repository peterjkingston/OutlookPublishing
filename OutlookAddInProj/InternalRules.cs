using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OutlookAddInController;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddInProj
{
	public class InternalRules : IInternalRules
	{
		readonly static string CRITERIA_PATH = $"C:\\Users\\{System.Environment.UserName}\\AppData\\Roaming\\Microsoft\\Outlook\\ExtensionCriteria.json";
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
							Task.Run(()=>criteria.Action(mail));
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