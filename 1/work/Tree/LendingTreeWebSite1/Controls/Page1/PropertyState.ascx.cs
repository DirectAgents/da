using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Xsl;

public partial class Controls_Page1_PropertyState : LendingTreeLib.UserControlBase
{
    public string Value
    {
        get
        {
            return DropDownList1.SelectedValue;
        }
    }
    public string Name
    {
        get
        {
            return DropDownList1.SelectedItem.Text;
        }
    }
}