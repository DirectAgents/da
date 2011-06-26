using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class _Default : QuickMatchPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //string cs = ConfigurationManager.ConnectionStrings["DatabaseLoggerConnectionString"].ConnectionString;
        //var  db = new LendingTreeLib.Common.DatabaseLoggerDatabase(cs);     
        //if (db.DatabaseExists())
        //    db.DeleteDatabase();
        //db.CreateDatabase();
        //Response.Write("<h1>database created</h1>");

        Response.Redirect(Resources.Url.Page1);
    }
}