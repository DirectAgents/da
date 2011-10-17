using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EomApp1.Formss.AB2.Model;

namespace DirectAgentsService
{
    public enum Operation
    {
        None,
        Insert
    }

    public enum RecordType
    {
        None,
        PayTerms,
        SqlServerDatabases,
        Units,
        UnitConversions,
        Advertisers,
        StartingBalances
    }

    public class Program
    {
        static Operation Operation = Operation.None;
        static RecordType RecordType = RecordType.None;
        static string OutputFile = string.Empty;
        static string ExternalDatabaseName = string.Empty;

        public static void Main(string[] args)
        {
            //try
            //{
            CheckArgs(args);
            ProcessArgs(args);
            DoOperation();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    Console.WriteLine(e.StackTrace);
            //    Console.WriteLine(e.InnerException != null ? e.InnerException.Message : string.Empty);
            //}
            //finally
            //{
            //    Console.ReadKey(true);
            //}
        }

        private static void CheckArgs(string[] args)
        {
            if (args.Length < 4)
                throw new Exception("Wrong number of arguments. \nTry: DirectAgentsService.exe /op <operation> /rt <return-type>");
        }

        private static void ProcessArgs(string[] args)
        {

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "/op":
                    case "/Operation":
                        Operation = args[++i].AsEnum<Operation>();
                        break;
                    case "/rt":
                    case "/RecordType":
                        RecordType = args[++i].AsEnum<RecordType>();
                        break;
                    case "/o":
                        OutputFile = args[++i];
                        break;
                    case "/db":
                        ExternalDatabaseName = args[++i];
                        break;
                }
            }
        }

        private static void DoOperation()
        {
            switch (Operation)
            {
                case Operation.None:
                    break;
                case Operation.Insert:
                    DoInsert();
                    break;
                default:
                    break;
            }
        }

        private static void DoInsert()
        {
            using (var model = new DirectAgentsEntities())
            {
                var records = new RecordSource(ExternalDatabaseName, model);
                Log("inserting {0}", RecordType.ToString());
                switch (RecordType)
                {
                    case RecordType.None:
                        break;
                    case RecordType.PayTerms:
                        records.PayTerms.ToList().ForEach(c =>
                            {
                                model.PayTerms.AddObject(c);
                            });
                        break;
                    case RecordType.SqlServerDatabases:
                        records.SqlServerDatabases.ToList().ForEach(c =>
                            {
                                model.SqlServerDatabases.AddObject(c);
                            });
                        break;
                    case RecordType.Units:
                        records.UnitNames.ToList().ForEach(c =>
                            {
                                model.Units.AddObject(new Unit { Name = c });
                            });
                        break;
                    case RecordType.UnitConversions:
                        records.UnitConversions.ToList().ForEach(c =>
                            {
                                model.UnitConversions.AddObject(c);
                            });
                        break;
                    case RecordType.Advertisers:
                        records.Advertisers.ToList().ForEach(c =>
                            {
                                model.Advertisers.AddObject(c);
                            });
                        break;
                    case RecordType.StartingBalances:
                        records.StartingBalances.ToList().ForEach(startingBalance =>
                        {
                        });
                        break;
                    default:
                        break;
                }
                Log("saving");
                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        private static void Log(string p, params string[] args)
        {
            Console.WriteLine(p, args);
        }
    }

    public static class Ext
    {
        public static T AsEnum<T>(this string source)
        {
            return (T)Enum.Parse(typeof(T), source);
        }
    }
}
