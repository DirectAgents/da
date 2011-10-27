using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using LendingTreeLib;

public partial class Controls_Automation : UserControlBase
{
    public string Target { get; set; }
    public string Property { get; set; }
    public string Value { get; set; }
    public string Action { get; set; }
    public string Key { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PreRender += new EventHandler(Controls_Automation_PreRender);
    }

    void Controls_Automation_PreRender(object sender, EventArgs e)
    {
        if (this.Enabled)
        {
            if (this.IsSetPropertyAction)
            {
                Object control = this.Parent.FindControl(this.Target);
                PropertyInfo pi = control.GetType().GetProperty(this.Property);
                pi.SetValue(control, this.Value, null);
            }
        }
    }

    bool Enabled
    {
        get
        {
            bool result = this.PageBase.IsSessionValueEnabled(this.Key);
            return result;
        }
    }

    bool IsSetPropertyAction
    {
        get
        {
            return this.Action == "SetProperty";
        }
    }
}
