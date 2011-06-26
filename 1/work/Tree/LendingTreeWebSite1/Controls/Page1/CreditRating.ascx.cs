using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Page1_CreditRating : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            DropDownList1.DataBind();
            DropDownList1.Items.FindByText("Good").Selected = true;
        }
    }

    public string Value
    {
        get
        {
            return DropDownList1.SelectedValue;
        }
    }
}