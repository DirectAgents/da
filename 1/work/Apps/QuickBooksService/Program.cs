using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using EomApp1.Formss.AB2.Model;

namespace QuickBooksService
{
    public class Program
    {
        private EomApp1.QuickBooks.QuickBooksService _service;
        private string[] _args;

        public Program(string[] args)
        {
            this._args = args;
        }

        static void Main(string[] args)
        {
            try
            {
                new Program(args).Go();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Log(string format, params object[] formatArgs)
        {
            if (formatArgs.Length > 0)
            {
                Console.WriteLine(format, formatArgs);
            }
            else
            {
                Console.WriteLine(format);
            }
        }

        /// <summary>
        /// Main Logic
        /// </summary>
        public void Go()
        {
            if (_args[0] == "-query")
            {
                ConnectToQuickBooks();
                DataTable queryResultTable = QueryQuickbooks(_args[1], _args[2]);
                WriteResultFiles(_args[1], _args[2], queryResultTable);
            }
            else if (_args[0] == "-push")
            {
                Save((RecordType)Enum.Parse(typeof(RecordType),_args[1]), _args[2]);
            }
        }

        private void ConnectToQuickBooks()
        {
            _service = new EomApp1.QuickBooks.QuickBooksService();
            _service.Connect();
            if (_service.ErrorMessage != String.Empty)
                Log(_service.ErrorMessage);
            else
                Log(_service.CompanyFile);
        }

        enum RecordType
        {
            Customers,
            ReceivedPayments
        }

        private void Save(RecordType recordType, string xmlDataFile)
        {
            XDocument data = XDocument.Load(xmlDataFile);
            switch (recordType)
            {
                case RecordType.Customers:
                    SaveCustomers(data);
                    break;
                case RecordType.ReceivedPayments:
                    SaveReceivedPayments(data);
                    break;
                default:
                    throw new Exception("invalid record type");
            }
        }

        private void SaveReceivedPayments(XDocument data)
        {
            var receivedPayments = data.Root.Elements("GetReceivedPayments");

            using (var model = new DirectAgentsEntities())
            {
                foreach (var receivedPayment in receivedPayments)
                {
                    // These are always present in the XML element for a customer
                    string companyFileName = receivedPayment.Element("QuickBooksCompanyFileName").Value;
                    string txnID = receivedPayment.Element("TxnID").Value;

                    // Check customer ref exists in the XML element
                    string customerRefListID;
                    if (receivedPayment.Element("CustomerRefListID") == null)
                    {
                        Log("{0} is missing customer ref, skipping", txnID);
                        continue;
                    }
                    else
                    {
                        customerRefListID = receivedPayment.Element("CustomerRefListID").Value;
                    }

                    // Check customer record exists in database
                    var qbCustomer = model.QuickBooksCustomers.FirstOrDefault(c => c.ListId == customerRefListID && c.QuickBooksCompanyFile.Name == companyFileName);
                    if (qbCustomer == null)
                    {
                        Log("{0} is missing customer record, skipping", txnID);
                        continue;
                    }

                    Log("TxnID: {0}", txnID);

                    // The company file record already exists
                    var companyFile = model.QuickBooksCompanyFiles.First(c => c.Name == companyFileName);

                    // The received payment may or may not exist
                    var qbReceivedPayment = model.QuickBooksReceivedPayments.FirstOrDefault(c => c.TxnId == txnID && c.QuickBooksCompanyFile.Name == companyFileName);

                    if (qbReceivedPayment == null)
                    {
                        qbReceivedPayment = new QuickBooksReceivedPayment();
                        model.QuickBooksReceivedPayments.AddObject(qbReceivedPayment);
                    }

                    // Populate fields
                    qbReceivedPayment.TxnId = txnID;
                    qbReceivedPayment.TxnNumber = receivedPayment.Element("TxnNumber").Value;
                    qbReceivedPayment.ARAccountRefFullName = receivedPayment.Element("ARAccountRefFullName").Value;
                    qbReceivedPayment.TxnDate = DateTime.Parse(receivedPayment.Element("TxnDate").Value);
                    qbReceivedPayment.Memo = receivedPayment.Element("Memo") != null ? receivedPayment.Element("Memo").Value : null;
                    qbReceivedPayment.PaymentMethodRefFullName = receivedPayment.Element("PaymentMethodRefFullName") != null ? receivedPayment.Element("PaymentMethodRefFullName").Value : "unknown";
                    qbReceivedPayment.QuickBooksCustomer = qbCustomer;
                    qbReceivedPayment.QuickBooksCompanyFile = companyFile;
                }
                model.SaveChanges();
            }
        }

        private void SaveCustomers(XDocument data)
        {
            var customers = data.Root.Elements("GetCustomers");

            using (var model = new DirectAgentsEntities())
            {
                foreach (var customer in customers)
                {
                    Log("ListID: {0}", customer.Element("ListID"));

                    // These are always present in the XML element for a customer
                    string companyFileName = customer.Element("QuickBooksCompanyFileName").Value;
                    string listID = customer.Element("ListID").Value;

                    // The company file record already exists
                    var companyFile = model.QuickBooksCompanyFiles.First(c => c.Name == companyFileName);

                    // The customer may or may not exist
                    var qbCustmer = model.QuickBooksCustomers.FirstOrDefault(c => c.ListId == listID && c.QuickBooksCompanyFile.Name == companyFileName);

                    if (qbCustmer == null)
                    {
                        qbCustmer = new QuickBooksCustomer();
                        model.QuickBooksCustomers.AddObject(qbCustmer);
                    }

                    // Populate fields
                    qbCustmer.ListId = listID;
                    qbCustmer.FullName = customer.Element("FullName").Value;
                    qbCustmer.CompanyName = customer.Element("CompanyName") != null ? customer.Element("CompanyName").Value : "unknown";
                    qbCustmer.Phone = customer.Element("Phone") != null ? customer.Element("Phone").Value : "unknown";
                    qbCustmer.Email = customer.Element("Email") != null ? customer.Element("Email").Value : "unknown";
                    qbCustmer.TermsRefFullName = customer.Element("TermsRefFullName") != null ? customer.Element("TermsRefFullName").Value : "unknown";
                    qbCustmer.QuickBooksCompanyFile = companyFile;
                }
                model.SaveChanges();
            }
        }

        DataTable QueryQuickbooks(string queryName, string companyName)
        {
            DataTable dataTable = new DataTable(queryName);
            XElement query = XDocument.Load("QodbcQueries.xml")
                                    .Root
                                    .Elements()
                                    .FirstOrDefault(c => c.Attribute("name").Value == queryName);
            if (query != null)
            {  
                Log(query.Value);
                if (_service.FillData(dataTable, query.Value))
                {
                    // Add a column to identify the rows by company name
                    var addCol = dataTable.Columns.Add("QuickBooksCompanyFileName");
                    foreach (DataRow item in dataTable.Rows)
                    {
                        item.SetField<string>(addCol, companyName);
                    }
                    dataTable.AcceptChanges();
                }
                else throw new Exception(_service.ErrorMessage);
            }
            else throw new Exception("query name not found");

            return dataTable;
        }

        private void WriteResultFiles(string queryName, string companyName, DataTable queryResultTable)
        {
            queryResultTable.WriteXmlSchema(companyName + "_" + queryName + ".xsd");
            queryResultTable.WriteXml(companyName + "_" + queryName + ".xml");
        }
    }
}
