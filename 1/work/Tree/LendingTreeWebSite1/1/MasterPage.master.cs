using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;

public partial class _1_MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public string RefinanceRate
    {
        get
        {
            string s = GetInfo("RefinanceRate");
            return s;
        }
    }

    public string APR
    {
        get
        {
            string s = GetInfo("APR");
            return s;
        }
    }
    
    private string GetInfo(string s)
    {
        string result;
        string key = "Info_" + s;
        object o = Session[key];
        // todo: sql injection?
        if (o == null)
        {
            try
            {
                string q = string.Format("select data from Infos where name='{0}'", s); // todo: use entlib data block here?
                string cs = ConfigurationManager.ConnectionStrings["LendingTreeWebConnectionString"].ConnectionString;
                using(var con = new SqlConnection(cs))
                using (var cmd = new SqlCommand(q, con))
                {
                    con.Open();
                    o = cmd.ExecuteScalar(); // todo: what can execute scalar return?
                }
            }
            catch (Exception)
            {
                // todo: log error
                o = string.Empty;
            }
            Session[key] = o;
        }
        result = (string)o;
        return result;
    }
}
