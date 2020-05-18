using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Acrobat;
using OlDocPublish.Factory;

namespace OlDocPublish.ManagedCOM
{
	/// <summary>
	/// Managed instance of acrobat.
	/// </summary>
	public class Acrobat_COM : IDisposable
	{
		public CAcroApp Application { get; private set; }
		public CAcroAVDoc[] AVDocs = new CAcroAVDoc[] { };
		public CAcroPDDoc[] PDDocs = new CAcroPDDoc[] { };
		public AcrobatLoadError LoadError = AcrobatLoadError.NoError;

		public Acrobat_COM(bool openApplication = false)
		{
			if (openApplication) { OpenApplication(); }
		}

		public Acrobat_COM(string openPDFPath, bool openApplication = false)
		{
			if (File.Exists(openPDFPath))
			{
				if (openApplication) { OpenApplication(); }
				OpenNewPDDoc(openPDFPath);
			}
			else
			{
				LoadError = AcrobatLoadError.FileNotFound;
			}
		}

		public CAcroApp OpenApplication()
		{
			return Application = new AcroApp();
		}

		public void ClearLoadError()
		{
			LoadError = AcrobatLoadError.NoError;
		}

		public CAcroPDDoc OpenNewPDDoc(string openPDFPath)
		{
			if (File.Exists(openPDFPath))
			{
				List<CAcroPDDoc> pdDocList = new List<CAcroPDDoc>(PDDocs);

				CAcroPDDoc pDDoc = new AcroPDDoc();
				if (pDDoc.Open(openPDFPath))
				{
					pdDocList.Add(pDDoc);
					PDDocs = pdDocList.ToArray();
				}
				else
				{
					LoadError = AcrobatLoadError.PDFInvalid;
				}
				return pDDoc;
			}
			else
			{
				LoadError = AcrobatLoadError.FileNotFound;
				return null;
			}
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					//Free any pointers, etc. here.
				}

				// Free unmanaged resources (unmanaged objects) and override a finalizer below.
				foreach (CAcroPDDoc pDDoc in PDDocs) { if(pDDoc != null) pDDoc.Close(); }
				foreach (CAcroAVDoc aVDoc in AVDocs){ if (aVDoc != null) aVDoc.Close(-1);}
				if (Application != null)
				{
					Application.Hide();
					Application.CloseAllDocs();
					Application.Exit();
				}

				// Set large fields to null.
				Application = null;
				AVDocs = null;
				PDDocs = null;

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		~Acrobat_COM()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
