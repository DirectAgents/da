using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class track : System.Web.Services.WebService 
{
    [WebMethod]
    public void lead(string id) 
    {
        var session = HttpContext.Current.Session;
        if (session["Tracked"] == null)
        {
            session["Tracked"] = "1";
            LogLead(id);
        }
    }

    private void LogLead(string id)
    {
        var guid = id == "test" ? Guid.NewGuid() : Guid.Parse(id);
        string cs = LendingTreeWeb.ConnectionStrings.LendingTreeWebConnectionString;
        string sql = "INSERT INTO [dbo].[Log] ([id], [timestamp], [info]) VALUES (@id, @ts, @info)";
        using (var connection = new SqlConnection(cs))
        using (var command = new SqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@id", guid);
            command.Parameters.AddWithValue("@ts", DateTime.Now);
            command.Parameters.AddWithValue("@info", "lead tracked");
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
