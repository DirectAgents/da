<%@ WebHandler Language="C#" Class="Report" %>

using System;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;

public class Report : IHttpHandler
{
    const string _fromKey = "from";
    const string _toKey = "to";
    const string _textContentType = "text/csv";
    const string _csvContentDisposition = "attachment; filename=lendingtree.csv";
    const string _sql = "SELECT * FROM dbo.GetLeads2('{0:M/d/yyyy HH:mm:ss}','{1:M/d/yyyy HH:mm:ss}')";
    
    HttpContext _context = null;

    bool IsAdmin // dup
    {
        get
        {
            string ip = Request.UserHostAddress;
            string ips = WebConfigurationManager.AppSettings["AdminIps"];
            return ips.Contains(ip);
        }
    }
    
    public void ProcessRequest(HttpContext context)
    {
        _context = context;
        if (IsAdmin)
        {
            try
            {
                GenerateReport();
            }
            catch (Exception e)
            {
                ReportError(e);
            }
        }
    }

    void GenerateReport()
    {
        ContentType = _textContentType;
        ContentDisposition = _csvContentDisposition;
        string query = string.Format(_sql, ReportFromTime, ReportToTime);
        using (var con = new SqlConnection(ConnectionString))
        using (var cmd = new SqlCommand(query, con))
        {
            con.Open();
            var reader = cmd.ExecuteReader();
            bool wroteHeaderRow = false;
            while (reader.Read())
            {
                int fieldCount = reader.FieldCount;
                if (!wroteHeaderRow)
                {
                    for (int i = 0; i < fieldCount; i++)
                    {
                        bool lastIteration = (i == fieldCount - 1);
                        Response.Write(reader.GetName(i));
                        if (!lastIteration)
                        {
                            Response.Write(",");
                        }
                        else
                        {
                            Response.Write("\n");
                        }
                    }
                    wroteHeaderRow = true;
                }
                for (int i = 0; i < fieldCount; i++)
                {
                    bool lastIteration = (i == fieldCount - 1);
                    Response.Write("\"");
                    Response.Write(reader[i].ToString().TrimEnd());
                    Response.Write("\"");
                    if (!lastIteration)
                    {
                        Response.Write(",");
                    }
                    else
                    {
                        Response.Write("\n");
                    }
                }
            }
        }
    }
    
    DateTime ReportFromTime
    {
        get
        {
            return new DateTime(Int64.Parse(_context.Request[_fromKey]));
        }
    }

    DateTime ReportToTime
    {
        get
        {
            return new DateTime(Int64.Parse(_context.Request[_toKey])).AddDays(1); // before the following day
        }
    }
    
    string ConnectionString
    {
        get
        {
            return WebConfigurationManager.ConnectionStrings["LendingTreeWebConnectionString"].ConnectionString;
        }
    }

    string ContentDisposition
    {
        set
        {
            _context.Response.Headers.Add("Content-disposition", value);
        }
    }

    void ReportError(Exception e)
    {
        Response.ContentType = "text/plain";
        Response.Write(e.Message);
    }

    string ContentType
    {
        set
        {
            _context.Response.ContentType = value;
        }
    }

    HttpResponse Response
    {
        get
        {
            return _context.Response;
        }
    }

    HttpRequest Request
    {
        get
        {
            return _context.Request;
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}