using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Page2_MonthlyMortgagePayment : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    public string Value
    {
        get
        {
            return DropDownList1.SelectedValue;
        }
        set
        {
            DropDownList1.Items.FindByValue(value).Selected = true;
        }
    }
}