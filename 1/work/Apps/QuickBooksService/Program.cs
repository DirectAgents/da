using System;
using AccountingBackupWeb.Models.AccountingBackup;
using DAgents.Common;
using Microsoft.Practices.Unity;
using QuickBooksLibrary;

namespace QuickBooksService
{
    class Boot
    {
        static void Main(string[] args)
        {
            try
            {
                new Program().Run(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                Console.WriteLine("stack trace?");

                ConsoleKeyInfo key = Console.ReadKey(true);

                if(key.KeyChar == 'y')
                    Console.WriteLine(ex.StackTrace);
            }   
        }
    }

    class Program : ConsoleProgramBase
    {
        public override void Configure()
        {
            var myContainer = new MyContainer(this.UnityContainer);
            UnityContainer = myContainer.Container;
        }

        public override IUnityContainer UnityContainer { get; set; }

        // TODO: check preconditions, such as companies not being loaded by the other services having been run...
        public void Run(string[] args)
        {
            if (args.Length != 3)
                throw new Exception("usage: QuickBooksService.exe (load|extract) (us|intl) ([Customer][,Invoice][,Payment][,All])");

            UnityContainer.BuildUp(this);

            IProgramAction driver = null;

            UnityContainer

                .RegisterInstance<Company>(ParseCompany(args[1]))

                .RegisterInstance<eTargets>(ParseTargets(args[2]));

            if (args[0] == "load")
            {
                UnityContainer

                        .RegisterType<ILoader, Loader_Customer>("customer")
                        .RegisterType<ILoader, Loader_Invoice>("invoice")
                        .RegisterType<ILoader, Loader_Payment>("payment")

                        ;

                if (args[1] == "us")

                    UnityContainer

                        .RegisterType<LoaderInputReader>(
                            new InjectionConstructor(
                                (string)Config.ExtractFiles.US, typeof(Company), typeof(eTargets)
                            )
                        )

                    ;

                else if (args[1] == "intl")

                    UnityContainer

                        .RegisterType<LoaderInputReader>(
                            new InjectionConstructor(
                                (string)Config.ExtractFiles.Intl, typeof(Company), typeof(eTargets)
                            )
                        )

                    ;

                else
                    throw new Exception("invalid company");

                driver = UnityContainer.Resolve<LoaderDriver>();
            }
            else if (args[0] == "extract")
            {
                UnityContainer

                    .RegisterType<Extracter>(
                        new InjectionConstructor(
                            typeof(Company), typeof(QuickBooksQuery), (DateTime)Config.FromDate, typeof(eTargets)
                        )
                    )
                ;

                driver = UnityContainer.Resolve<Extracter>();
            }
            else
                throw new Exception("no action");

            if (args[0] != null)
                driver.Execute();
        }

        private static Company ParseCompany(string s)
        {
            var company = Company.ByName(s, true);
            return company;
        }

        private eTargets ParseTargets(string s)
        {
            eTargets res = eTargets.None;

            string[] targets = s.Split(',');
            foreach (var target in targets)
            {
                eTargets add;
                if (Enum.TryParse(target, out add))
                    res |= add;
                else
                    Logger.LogError("invalid target: " + target);
            }

            return res;
        }
    }
}
