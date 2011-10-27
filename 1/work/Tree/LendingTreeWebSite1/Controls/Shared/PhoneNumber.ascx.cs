using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Controls_Shared_PhoneNumber : LendingTreeLib.UserControlBase
{
    public string TextBoxText 
    { 
        get 
        { 
            return TextBox1.Text;
        }
        set 
        { 
            TextBox1.Text = value; 
        }
    }

    public string ValidatorErrorText 
    { 
        get
        { 
            return RegularExpressionValidator1.ErrorMessage; 
        } 
        set
        {
            RegularExpressionValidator1.ErrorMessage = value; 
        }
    }

    public string WaterMarkText
    { 
        get; 
        set; 
    }

    public string Value
    { 
        get 
        {
            return TextBox1.Text; 
        }
        set
        {
            TextBox1.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        AddWatermark(TextBox1, WaterMarkText);
    }
}
