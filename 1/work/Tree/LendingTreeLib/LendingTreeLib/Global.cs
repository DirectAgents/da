using System;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using LendingTreeLib.Common;
using LendingTreeLib.Loggers;
using Microsoft.Practices.Unity;

namespace LendingTreeLib
{
    public class Global : HttpApplication, IContainerAccessor
    {
        void Application_Start(object sender, EventArgs e)
        {
            BuildContainer();
        }

        protected static void BuildContainer()
        {
            IUnityContainer container = new UnityContainer();

            container

                .RegisterType<ILogger, DatabaseLogger>()

                .RegisterInstance<LendingTreeConfig>(LendingTreeConfig.Create(System.Web.Hosting.HostingEnvironment.MapPath(
                    "~/App_Data/LendingTreeConfig." + WebConfigurationManager.AppSettings["LendingTreeConfiguration"] + ".xml")))

                .RegisterInstance<string>("ConnectionString",
                    ConfigurationManager.ConnectionStrings["LendingTreeWebConnectionString"].ConnectionString)

                .RegisterInstance<string>("StatesExcludedFromDisclosure",
                    WebConfigurationManager.AppSettings["StatesExcludedFromDisclosure"]);

            Container = container;
        }

        private static IUnityContainer _container;
        public static IUnityContainer Container
        {
            get
            {
                return _container;
            }
            set
            {
                _container = value;
            }
        }

        IUnityContainer IContainerAccessor.Container
        {
            get
            {
                return Container;
            }
        }

        void Application_End(object sender, EventArgs e)
        {
            CleanUp();
        }

        protected static void CleanUp()
        {
            if (Container != null)
            {
                Container.Dispose();
            }
        }
    }
}