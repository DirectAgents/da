using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CampaignWikiWebApplication1.Templates;

namespace CampaignWikiWebApplication1.WebUserControls
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        string _pid;

        public string PID
        {
            get { return _pid; }
            set 
            {
                PreTextTemplate1 template = new PreTextTemplate1();

                _pid = value;

                //string contentFormat = "<b>{0}</b> bar";
                //string content = string.Format(contentFormat, this.PID);

                string content = template.TransformText();

                //content = content.Replace("\r\n", "<br/>");
                content = content.Replace("\n", "<br/>");

                Literal1.Text = content;
                
            }
        }
    }
}