using System;
using System.Web.Services;
using System.Web;

[WebService(Namespace = "http://mortgate-rate-offers.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    [WebMethod]
    public LendingTreeDataSet.EntryDataTable GetWebLogEntries(DateTime fromDate, DateTime toDate)
    {
        var adapter = new LendingTreeDataSetTableAdapters.EntryTableAdapter();
        return adapter.GetDataByDateRange(fromDate, toDate);
    }

    [WebMethod]
    public LendingTreeDataSet.ResponseFileDataTable GetWebLogEntries2(DateTime fromDate, DateTime toDate)
    {
        string fileName = GetDataFile(fromDate, toDate);
        var a = new LendingTreeDataSet.ResponseFileDataTable();
        var b = a.NewResponseFileRow();
        b.FileName = GetUrl(fileName);
        a.AddResponseFileRow(b);
        //a.AcceptChanges();
        return a;
    }

    [WebMethod]
    public ResponseUrl GetWebLogEntries3(DateTime fromDate, DateTime toDate)
    {
        string fileName = GetDataFile(fromDate, toDate);
        string url = GetUrl(fileName);
        return new ResponseUrl(url);
    }

    [WebMethod]
    public string GetWebLogEntries4(DateTime fromDate, DateTime toDate)
    {
        string fileName = GetDataFile(fromDate, toDate);
        string url = GetUrl(fileName);
        return url;
    }

    private string GetUrl(string fileName)
    {
        string path1 = Context.Request.Url.AbsoluteUri.Replace("/Service.asmx", "");
        return path1 + "/responses/" + fileName;
    }

    private string GetDataFile(DateTime fromDate, DateTime toDate)
    {
        var adapter = new LendingTreeDataSetTableAdapters.EntryTableAdapter();
        LendingTreeDataSet.EntryDataTable data = adapter.GetDataByDateRange(fromDate, toDate);
        string fileName = Guid.NewGuid() + ".xml";
        data.WriteXml(Server.MapPath("~/da/Data/responses/" + fileName));
        return fileName;
    }
}

public class ResponseUrl
{
    public ResponseUrl()
    {
    }
    public ResponseUrl(string fileName)
    {
        this.FileName = fileName;
    }
    public string FileName { get; set; }
}