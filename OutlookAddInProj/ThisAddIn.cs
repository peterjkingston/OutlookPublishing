using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;
using OutlookAddInController;

namespace OutlookAddInProj
{
    public partial class ThisAddIn
    {
        private IContainer _container {get; set;}
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            _container = ContainerFactory.CreateContainer(this.Application);
            this.Application.NewMailEx += Application_NewMailEx;
        }

        private void Application_NewMailEx(string EntryIDCollection)
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                string[] args = new string[] {EntryIDCollection};
                var p = scope.Resolve<IProgram>();
                p.Main(args);
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Note: Outlook no longer raises this event. If you have code that 
            //    must run when Outlook shuts down, see https://go.microsoft.com/fwlink/?LinkId=506785
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
