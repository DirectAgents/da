using System;
public partial class Controls_Page3_FirstName : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AddWatermark(TextBox1, Resources.String.String45);
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
}