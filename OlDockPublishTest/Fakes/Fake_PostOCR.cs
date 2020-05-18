using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OlDocPublish.Processors;

namespace OlDockPublishTest.Fakes
{
	class Fake_PostOCR : IPostOCR
	{
		public int GetPageID(int page)
		{
			return 1;
		}

		public string GetSO()
		{
			return "123456";
		}
	}
}
