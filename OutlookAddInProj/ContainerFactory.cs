using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;
using OutlookAddInProj;
using OlDocPublish;
using OlDocPublish.SAPTools;
using Resources;
using OlDocPublish.Snipping;
using OlDocPublish.Logging;

namespace OutlookAddInController
{
	public class ContainerFactory
	{
		public static IContainer CreateContainer(Outlook.Application app)
		{
			var builder = new ContainerBuilder();

			///<!--START PRIMARY PROCESSING -->

			//Entry Point
			builder.RegisterType<Program>().As<IProgram>();
			builder.RegisterInstance(app).As<Outlook.Application>();

			//Outlook rule mocks
			builder.RegisterType<InternalRules>().As<IInternalRules>();
			builder.RegisterType<RuleReader>().As<IRuleReader>();

			//Document processing
			builder.RegisterType<AcrobatAutoProcessor>().As<IDocumentProcessor>();
			builder.RegisterType<AcrobatPostOCR>().As<IPostOCR>();

			//Data retreival
			builder.RegisterType<SAPOrderDataProvider>().As<IOrderDataProvider>();
			builder.RegisterType<SAPTextDataReader>().As<IDataReader>();
			builder.RegisterInstance(new ResourceRetreiver()).As<IResourceRetreiver>();

			//PDF Host App Instancing
			builder.RegisterType<Factory>().As<IAcrobatTypeProvider>();

			///<!--END PRIMARY PROCESSING -->
		


			///<!--START EVENT HOOK ADD-INS -->

			//Snipping
			builder.RegisterType<AcrobatPublisher>().As<IPublisher>();

			//Logging
			builder.RegisterType<SalesOrderLogger>().As<ISalesOrderLogger>();

			///<!--END EVENT HOOK ADD-INS -->

			return builder.Build();
		}
	}
}
