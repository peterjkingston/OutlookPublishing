using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OutlookAddInProj;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace OutlookAddInController
{
    public class Program : IProgram
    {
        private Application _app;
        private IInternalRules _rules;

        public Program(Application app, IInternalRules rules)
        {
            _app = app;
            _rules = rules;
        }

        public void Main(string[] args)
        {
            string EntryIDCollection = args[0];
            string[] cdt = EntryIDCollection.Split(',');

            for (int index = 0; index < cdt.Length; index++)
            {
                Outlook.MailItem mail = TryGetMail(cdt[index]);
                if (mail != null)
                {
                    _rules.Process(mail);
                }
            }
        }

        private Outlook.MailItem TryGetMail(string mailID)
        {
            object item = _app.Session.GetItemFromID(mailID);
            return item.GetType() == typeof(Outlook.MailItem) ? (Outlook.MailItem)item : null;
        }
    }
}
