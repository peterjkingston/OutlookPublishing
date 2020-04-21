using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OlDocPublish.Processors;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OlDocPublish.RulesMock
{
	public class RuleBuilder<T> where T:IRuleCriteria, new()
	{

		public IRuleCriteria CreateRule(RuleProperty property, RulePropertyCondition condition, string expectedText, RuleAction action)
		{
			T rule = new T();
			//{
			//	rule.Property = property;
			//	rule.Condition = condition;
			//	rule.Validation = expectedText;
			//	rule.ResultingAction = action;
			//}

			return rule;
		}




	/***********************************OLD VERSION BELOW**************************************/
	/*
	public class RuleBuilder
	{
		
		
		private Application _outlookApp;
		private IDocumentProcessor _processor;

		public RuleBuilder(IDocumentProcessor processor, Application olApp)
		{
			_outlookApp = olApp;
			_processor = processor;
		}

		public IRuleCriteria CreateCriteria<T>(Func<MailItem_ID, bool> matchFunction, Action<MailItem_ID, string[]> action) where T :  IRuleCriteria, new()
		{
			IRuleCriteria rule = new T();
			{
				rule.Action = action;
				rule.Match = matchFunction;
			}
			return rule;
		}

		public IRuleCriteria CreateCriteria<T>(string matchParameter, string matchExpected, RuleAction ruleAction) where T : IRuleCriteria, new()
		{
			IRuleCriteria rule = new T();
			{
				rule.Match = (MailItem_ID mail) => { return (string)mail.GetType().GetProperty(matchParameter).GetValue(mail) == matchExpected; };
				
				//*********************************************************************
				//Define prebuilt actions
				//*********************************************************************
				Action<MailItem_ID,string[]> process = (MailItem_ID mail, string[] args) => { _processor.ProcessMailItem((MailItem)_outlookApp.Session.GetItemFromID(mail.Value)); };
				
				Action<MailItem_ID, string[]> moveToFolder = (MailItem_ID mail, string[] args) =>
				{
					string folderName = GetFolderNameFromCLICommand(args[0]);
					if (folderName != "") MoveMailToFolder((MailItem)_outlookApp.Session.GetItemFromID(mail.Value), folderName);
				};

				//*********************************************************************
				//End define prebuilt actions
				//*********************************************************************


				switch (ruleAction)
				{
					case RuleAction.ProcessPDF:
						rule.Action = process;
						break;


					case RuleAction.MoveToFolder:
						rule.Action = moveToFolder;
						break;

					case RuleAction.ProcessPDF_AND_MoveToFolder:
						rule.Action = (MailItem_ID mail, string[] args) =>
						{
							process(mail, args);
							moveToFolder(mail, args);
						};
						break;


					default:
						break;
				}
			}
			return rule;
		}

		private void MoveMailToFolder(MailItem mail, string folderName)
		{
			Outlook.Folder folder = (Folder)_outlookApp.Session.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
			folder = (Folder)folder.Folders[folderName];
			mail.Move(folder);
		}

		//Call this starting at "-m" or "--moveToFolder" followed by the switch arguments.
		private string GetFolderNameFromCLICommand(string switchArguments)
		{
			//CLI
			//rulebuilder <MAIL_ID> [-m | --moveToFolder <DESTINATION MAPI_FOLDER NAME>]
			if(switchArguments.Contains("-m"))
			{
				return GetCLIArgument("-m", switchArguments);
			}
			else if (switchArguments.Contains("--moveToFolder"))
			{
				return GetCLIArgument("--moveToFolder", switchArguments);
			}
			else
			{
				return "";
			}
		}

		private string GetCLIArgument(string cliSwitch, string inputCommand)
		{
			int argStart = inputCommand.LastIndexOf(cliSwitch) + cliSwitch.Length;
			
			//Probably a better way to do this, but for now, just pick the start of the next switch-like character
			int nextSwitchStart = inputCommand.IndexOf("-",argStart) < inputCommand.IndexOf("--",argStart)?
					inputCommand.LastIndexOf("-"):
					inputCommand.LastIndexOf("--");

			int argLength = nextSwitchStart - argStart;
			string argument = inputCommand.Substring(argStart, argLength);
			return argument.Trim();
		}*/
	}
}
