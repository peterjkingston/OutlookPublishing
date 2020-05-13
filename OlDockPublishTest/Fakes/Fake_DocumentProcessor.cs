using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Outlook;
using OlDocPublish.Processors;
using OlDocPublish.Snipping;

namespace OlDockPublishTest.Fakes
{
	class Fake_DocumentProcessor : IDocumentProcessor
	{
		public string SO => _so;
		private string _so = "";

		public event EventHandler<SnipInfo> SnipRequested;
		public event EventHandler<SnipInfo> SnipIterated;
		public event EventHandler SnippingComplete;
		public event EventHandler<SnipInfo> SnipAfter;
		public event EventHandler<SnipInfo> BeforeSnipping;

		public bool ProcessMailItem(MailItem mail)
		{
			_so = "123456";
			return true;
		}
	}
}
