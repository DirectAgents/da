﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_Page1_LoanType : LendingTreeLib.UserControlBase
{
    public string Value
    {
        get
        {
            return DropDownList1.SelectedValue;
        }
    }
}