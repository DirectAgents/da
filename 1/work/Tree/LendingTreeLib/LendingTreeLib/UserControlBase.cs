using System.Web.UI;
using System.Web.UI.WebControls;

namespace LendingTreeLib
{
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
        /// It also requires that the rendered page include the CheckValue javascript function.
        /// It causes the text box to have a default value when no data is present.
        /// This default value is typically instructions like 'enter your name here'.
        /// When the text box gets focus (i.e. the user is going to type now) the default message vanishes.
        /// If the text box loses foxus and still has no data, the default value reappears.
        /// </summary>
        /// <param name="webControl"></param>
        /// <param name="waterMarkText"></param>
        protected void AddWatermark(WebControl webControl, string waterMarkText)
        {
            var webControlClientID = webControl.ClientID;
            var varName = "waterMarkTextFor" + webControlClientID;
            var clientScriptKey = "waterMarkScriptFor" + webControlClientID;
            var clientScriptIdentifyingType = this.GetType();
            var clientScript = Page.ClientScript;
            if (!clientScript.IsStartupScriptRegistered(clientScriptIdentifyingType, clientScriptKey))
            {
                string script = string.Format("var {0}='{1}';", varName, waterMarkText);
                clientScript.RegisterStartupScript(clientScriptIdentifyingType, clientScriptKey, script, true);
            }
            var attrivuteValue = "CheckValue(this, " + varName + ")";
            webControl.Attributes.Add("onfocus", attrivuteValue);
            webControl.Attributes.Add("onblur", attrivuteValue);
        }
    }
}
