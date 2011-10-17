using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Odbc;
using System.Xml.Linq;

namespace EomApp1.QuickBooks
{
    public class QuickBooksService
    {
        public bool Connected { get { return _connected; } }
        public string CompanyFile { get { return _companyFile; } }
        public string ErrorMessage { get { return _errorMessage; } }

        static void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void Connect()
        {
            try
            {
                using (var adapter = new OdbcDataAdapter("SP_QBFILENAME", _conStr))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    var result = table.AsEnumerable().FirstOrDefault();
                    if (result != null)
                    {
                        _companyFile = result.Field<string>("QBFileName");
                        _connected = true;
                    }
                }
            }
            catch (Exception e)
            {
                _errorMessage = e.Message;
            }
            
        }

        public bool FillData(DataTable table, string query)
        {
            bool ret = true;
            try
            {
                using (var adapter = new OdbcDataAdapter(query, _conStr))
                {
                    adapter.Fill(table);
                }
            }
            catch (Exception e)
            {
                _errorMessage = e.Message;
                ret = false;
            }
            return ret;
        }

        private string _conStr = @"DSN=QuickBooks Data;SERVER=QODBC;OptimizerDBFolder=%UserProfile%\QODBC Driver for QuickBooks\Optimizer;OptimizerAllowDirtyReads=N;SyncFromOtherTables=Y;IAppReadOnly=Y";
        private bool _connected = false;
        private string _companyFile = string.Empty;
        private string _errorMessage = string.Empty;
    }
}
