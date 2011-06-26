using System;
public partial class Controls_Page3_Email : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AddWatermark(TextBox1, Resources.String.String47);
    }
    public string Value
    {
        get
        {
            return TextBox1.Text;
        }
    }
}