using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Controls_Shared_LendingTreeLoansOptIn : LendingTreeLib.UserControlBase
{
    public bool Value
    {
        get
        {
            return RadioButtonList1.SelectedValue == "Y";
        }
    }
}