using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using LendingTreeLib;

public partial class da_Fix : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Details1.DataBound += new EventHandler(Details1_DataBound);
        RepostButton.Click += new EventHandler(RepostButton_Click);

        ValidateResponseText();
    }

    void RepostButton_Click(object sender, EventArgs e)
    {
        DoRepost();
    }

    /// <summary>
    /// Re-post data and save response in <see cref="ResponseText"/>.
    /// </summary>
    private void DoRepost()
    {
        var config = base.Resolve<LendingTreeConfig>();

        var request = (HttpWebRequest)WebRequest.Create(new Uri(config.PostUrl));
        request.ContentType = "text/xml";
        request.Method = "POST";

        using (var writer = new StreamWriter(request.GetRequestStream()))
        {
            var xDoc = XDocument.Parse(SubmitTextBox.Text);
            writer.Write(xDoc.Root.ToString());
        }

        string result = string.Empty;
        using (var response = request.GetResponse() as HttpWebResponse)
        {
            var reader = new StreamReader(response.GetResponseStream());
            result = XElement.Parse(reader.ReadToEnd()).ToString();
        }

        ResponseText = result;
    }

    void Details1_DataBound(object sender, EventArgs e)
    {
        FixXmlFormatting();
    }

    [QueryString]
    public string appid { get; set; }

    /// <summary>
    /// Encapsulate DetailsView inner controls.
    /// </summary>
    LinkButton RepostButton { get { return Details1.FindControl("Repost") as LinkButton; } }
    TextBox ResponseTextBox { get { return Details1.FindControl("ResponseText") as TextBox; } }
    TextBox SubmitTextBox { get { return Details1.FindControl("PostText") as TextBox; } }

    /// <summary>
    /// Encapsulate response text resulting from re-post.
    /// </summary>
    string ResponseText
    {
        set
        {
            ResponseTextBox.Text = value;
            ValidateResponseText();
        }
    }

    /// <summary>
    /// Determine if response represents success/failure and give visual feedback.
    /// </summary>
    private void ValidateResponseText()
    {
        bool valid = ResponseTextBox.Text.Contains("<ReturnURL>");

        if (valid)
        {
            ResponseTextBox.BackColor = Color.LightGreen;
        }
        else
        {
            ResponseTextBox.BackColor = Color.LightPink;
        }
    }

    /// <summary>
    /// Format XML with indentation to make it more readable.
    /// </summary>
    private void FixXmlFormatting()
    {
        ResponseTextBox.FormatTextAsXml();
        SubmitTextBox.FormatTextAsXml();
    }
}
