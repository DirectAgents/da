<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;

public class Handler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/html";
        string zip = context.Request.Form["zip"];
        string state = ConvertZip(zip);
        if (state != null)
        {
            context.Response.Write(state);
        }
    }
    
    string ConvertZip(string zip)
    {
        string cs = WebConfigurationManager.ConnectionStrings["LendingTreeWebConnectionString"].ConnectionString;
        string query = string.Format("select TOP 1 [City], [State Abbreviation] from zips where [ZIP Code]={0}", zip);
        string result = null;
        using (var con = new SqlConnection(cs))
        using (var cmd = new SqlCommand(query, con))
        {
            con.Open();
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = string.Format("{0}, {1}", reader["City"], reader["State Abbreviation"]);
            }
        }
        return result ?? "";
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}