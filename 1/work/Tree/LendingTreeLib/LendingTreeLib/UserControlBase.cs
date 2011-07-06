using System.Web.UI;
using System.Web.UI.WebControls;

namespace LendingTreeLib
{
    /// <summary>
    /// 
    /// </summary>
    public class UserControlBase : System.Web.UI.UserControl
    {
        public PageBase PageBase
        {
            get
            {
                return (PageBase)Page;
            }
        }

        /// <summary>
        /// This method is designed to be used with a TextBox web control.
        /// 
        /// The rendered page must include the CheckValue javascript function.
        /// 
        /// The text box will have a default value when no data is present, typically 
        /// instructions like 'enter your name here'.
        /// 
        /// When the text box receives the focus the default message clears out so
        /// s/he can type.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="defaultText"></param>
        protected void AddWatermark(WebControl target, string defaultText)
        {
            var webControlClientID = target.ClientID;
            var varName = "waterMarkTextFor" + webControlClientID;
            
            var clientScriptKey = "waterMarkScriptFor" + webControlClientID;
            var clientScriptIdentifyingType = this.GetType();
            var clientScript = Page.ClientScript;    
            if (!clientScript.IsStartupScriptRegistered(clientScriptIdentifyingType, clientScriptKey))
            {
                var script = string.Format("var {0}='{1}';", varName, defaultText);
                clientScript.RegisterStartupScript(clientScriptIdentifyingType, clientScriptKey, script, true);
            }

            var attrivuteValue = "CheckValue(this, " + varName + ")";
            target.Attributes.Add("onfocus", attrivuteValue);
            target.Attributes.Add("onblur", attrivuteValue);
        }
    }
}
