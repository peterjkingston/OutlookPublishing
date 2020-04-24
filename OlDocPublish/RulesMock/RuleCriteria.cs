using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OlDocPublish.Processors;

namespace OlDocPublish.RulesMock
{
	[Serializable()]
	public class RuleCriteria : IRuleCriteria
	{
		private Application _app;
		private IDocumentProcessor _processor;

		public RuleProperty Property { get; set; }
		public RulePropertyCondition Condition { get; set; }
		public string[] Validation { get; set; }
		public RuleAction[] ResultingAction { get; set; }
		public string ActionArg { get; set; }

		public RuleCriteria(Application app, IDocumentProcessor documentProcessor)
		{
			_app = app;
			_processor = documentProcessor;
		}

		public bool Match(MailItem_ID mailID)
		{
			MailItem mail = (MailItem)_app.Session.GetItemFromID(mailID.Value);
			string strProp = GetPropertyValue(Property, mail);
			Func<string, bool> comparer = GetComparer(strProp);
			bool flagResult = Validation.All(comparer);

			return flagResult;
		}

		private Func<string, bool> GetComparer(string compare)
		{
			Func<string, bool> resultFunction;

			switch (Condition)
			{
				case RulePropertyCondition.EqualTo:
					resultFunction = (x) => { return x == compare; };
					break;

				case RulePropertyCondition.NotEqualTo:
					resultFunction = (x) => { return x != compare; };
					break;

				case RulePropertyCondition.Contains:
					resultFunction = (x) => { return compare.Contains(x); };
					break;

				case RulePropertyCondition.DoesNotContain:
					resultFunction = (x) => { return !compare.Contains(x); };
					break;

				default:
					resultFunction = (x) => { throw new NotImplementedException(); };
					break;
			}

			return resultFunction;
		}

		private string GetPropertyValue(RuleProperty property, MailItem mail)
		{
			string strProp;
			switch (property)
			{
				case RuleProperty.SenderAddress:
					strProp = mail.Sender.Address;
					break;

				case RuleProperty.Body:
					strProp = mail.Body;
					break;

				case RuleProperty.Subject:
					strProp = mail.Subject;
					break;

				case RuleProperty.ToAddress:
					strProp = mail.To;
					break;

				case RuleProperty.CCAddress:
					strProp = mail.CC;
					break;

				default:
					strProp = "";
					break;
			}

			return strProp;
		}

		public void DoAction(MailItem_ID mailID)
		{
			MailItem mail = (MailItem)_app.Session.GetItemFromID(mailID.Value);
			Action<MailItem, string>[] actions = GetActions();

			foreach(Action<MailItem, string> action in actions)
			{
				action(mail, ActionArg);
			}
		}

		private Action<MailItem, string>[] GetActions()
		{
			Action<MailItem, string>[] resultAction = new Action<MailItem, string>[ResultingAction.Length];
			for (int actionIndex = 0; actionIndex < ResultingAction.Length; actionIndex++)
			{
				RuleAction ruleAction = ResultingAction[actionIndex];
				switch (ruleAction)
				{
					case RuleAction.ProcessPDF:
						resultAction[actionIndex] = (mail, a) => { _processor.ProcessMailItem(mail); };
						break;

					case RuleAction.MoveToFolder:
						resultAction[actionIndex] = (mail, a) =>
						{
							try
							{
								MAPIFolder folder = (MAPIFolder)_app.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
								folder = folder.Folders[a];
								mail.Move(folder);
							}
							catch
							{

							}
						};
						break;

					default:
						resultAction[actionIndex] = (mail, a) => { };
						break;
				}
			}


			return resultAction;
		}


		//public Action<MailItem_ID, string[]> Action { get; set; }
		//public Func<MailItem_ID, bool> Match { get; set; }
	}
}
