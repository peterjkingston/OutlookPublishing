using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookAddInController;

namespace OlDocPublish.RulesMock
{
	public class RuleBuilder
	{
		private Application _outlookApp;
		private IDocumentProcessor _processor;

		public RuleBuilder(IDocumentProcessor processor, Application olApp)
		{
			_outlookApp = olApp;
			_processor = processor;
		}

		public IRuleCriteria CreateCriteria<T>(Func<MailItem, bool> matchFunction, Action<MailItem,string[]> action) where T :  IRuleCriteria, new()
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
				rule.Match = (MailItem mail) => { return (string)mail.GetType().GetProperty(matchParameter).GetValue(mail) == matchExpected; };
				
				//*********************************************************************
				//Define prebuilt actions
				//*********************************************************************
				Action<MailItem,string[]> process = (MailItem mail, string[] args) => { _processor.ProcessMailItem(mail); };
				
				Action<MailItem,string[]> moveToFolder = (MailItem mail, string[] args) =>
				{
					string folderName = GetFolderNameFromCLICommand(args[0]);
					if (folderName != "") MoveMailToFolder(mail, folderName);
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
						rule.Action = (MailItem mail, string[] args) =>
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
		}
	}
}
