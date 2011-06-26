using System;
public partial class Controls_Page2_PropertyCity : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AddWatermark(TextBox1, Resources.String.String73);
    }

    public string Value
    {
        get
        {
            return TextBox1.Text;
        }
    }
}