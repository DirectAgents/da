using System;
public partial class Controls_Page3_ZipCode : LendingTreeLib.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
    public bool IsEmpty
    {
        get
        {
            return string.IsNullOrWhiteSpace(TextBox1.Text);
        }
    }
}