﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 10.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace EomApp1.Formss.PubRep1.Controls
{
    using System.Linq;
    using System;
    
    
    #line 1 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public partial class PubRepTemplate : PubRepTemplateBase
    {
        public virtual string TransformText()
        {
            
            #line 3 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"

var items = Data.Where(c => c.Publisher == Publisher).ToList(); 
var first = items.First();
string currency = first.PayCurrency;

            
            #line default
            #line hidden
            this.Write("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"no\"?>\r\n<!DOCTYPE html PUBLIC \"-/" +
                    "/W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.d" +
                    "td\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n  <head>\r\n    <title>Publishe" +
                    "r Report</title>\r\n    <style type=\"text/css\">\r\nbody {font-size: 80%; font-family" +
                    ": arial}\r\nh2 {border-width: thin; border-style: solid; text-align: center;}\r\n#ou" +
                    "ter {width: 700px; margin: 0px auto;}\r\n#title {font-weight: bold; background-col" +
                    "or:#ffffff; border-width: thin; border-style: none; text-align: center}\r\n#title1" +
                    " {font-size: 200%; font-style: italic; color: #000000}\r\n#title2 {font-size: 200%" +
                    "; color: #000000}\r\n#title3 {font-size: 140%; color: #000000}\r\n#subtitle {text-al" +
                    "ign: center; padding: 1em;}\r\n#info1 {padding: 0.7em;}\r\n#info2 {padding: 0.7em;}\r" +
                    "\n#info3 {padding: 0.7em;}\r\n.lineitems {font-size: 70%; border: 1px solid black; " +
                    "border-collapse:collapse; clear: both}\r\n.lineitems2 {border: 1px solid black; pa" +
                    "dding:1px; width: 387px}\r\n.lineitems3 {border: 1px solid black; padding:1px; wid" +
                    "th: 100px; text-align: center}\r\n.lineitems4 {border: 1px solid black; padding:1p" +
                    "x; width: 100px; text-align: center}\r\n.lineitems5 {border: 1px solid black; padd" +
                    "ing:1px; width: 100px; text-align: center}\r\n.lineitems6 {border: 1px solid black" +
                    "; padding:1px; width: 100px; text-align: center; background-color:#cccccc}\r\n.nos" +
                    "how {display: none;}\r\n#info4 {padding: 0.7em; float: left}\r\n#info5 {padding: 0.7" +
                    "em; float: right}\r\n</style>\r\n  </head>\r\n  <body>\r\n    <div id=\"outer\">\r\n      <d" +
                    "iv id=\"title\">\r\n        <img src=\"http://www.directagents.com/publisherreporthea" +
                    "der.png\" />\r\n      </div>\r\n      <div id=\"subtitle\">\r\n        <span id=\"subtitle" +
                    "1\">Monthly Performance Report</span>\r\n      </div>\r\n      <div id=\"info1\">\r\n    " +
                    "    <span id=\"info11\">");
            
            #line 45 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(first.Publisher));
            
            #line default
            #line hidden
            this.Write(" </span>\r\n      </div>\r\n      <div id=\"info2\">\r\n        <span id=\"info21\">");
            
            #line 48 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(first.MediaBuyer));
            
            #line default
            #line hidden
            this.Write("</span>\r\n      </div>\r\n      <div id=\"info3\">\r\n        <span id=\"info31\">Report F" +
                    "or: ");
            
            #line 51 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FormatDate(FromDate)));
            
            #line default
            #line hidden
            this.Write(" to ");
            
            #line 51 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FormatDate(ToDate)));
            
            #line default
            #line hidden
            this.Write("</span>\r\n      </div>\r\n");
            
            #line 53 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"

foreach(var i in new [] {
    new { q = (items.Where(c=>c.IsCPM=="No" && c.Paid > 0 && c.CampaignStatus == "Verified")), s = "Paid" },
    new { q = (items.Where(c=>c.IsCPM=="No" && c.ToBePaid > 0 && c.CampaignStatus == "Verified")), s= "To Be Paid" },
    new { q = (items.Where(c=>c.IsCPM=="Yes" && c.Paid > 0 && c.CampaignStatus == "Verified")), s = "CPM Paid" },
    new { q = (items.Where(c=>c.IsCPM=="Yes" && c.ToBePaid > 0 && c.CampaignStatus == "Verified")), s = "CPM To Be Paid" }
}) {
	var list = i.q.ToList();
	if(list.Count == 0) continue;
	foreach(var c in i.q) {
	
            
            #line default
            #line hidden
            this.Write("\t<div>\r\n        <br />\r\n        <table class=\"lineitems\">\r\n          <tr class=\"l" +
                    "ineitems1\">\r\n            <td class=\"lineitems2\">Campaign: ");
            
            #line 68 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.CampaignName));
            
            #line default
            #line hidden
            this.Write("</td>\r\n            <td class=\"lineitems3\">Payout</td>\r\n            <td class=\"lin" +
                    "eitems4\">Actions</td>\r\n            <td class=\"lineitems5\">Revenue</td>\r\n        " +
                    "  </tr>\r\n          <tr class=\"lineitems1\">\r\n            <td class=\"lineitems2\">");
            
            #line 74 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.CampaignName));
            
            #line default
            #line hidden
            this.Write(" - ");
            
            #line 74 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.AddCode));
            
            #line default
            #line hidden
            this.Write("</td>\r\n            <td class=\"lineitems3\">");
            
            #line 75 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FormatCurrency(currency, c.CostPerUnit)));
            
            #line default
            #line hidden
            this.Write("</td>\r\n            <td class=\"lineitems4\">");
            
            #line 76 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((int)c.NumUnits));
            
            #line default
            #line hidden
            this.Write("</td>\r\n            <td class=\"lineitems5\">\r\n              <span>");
            
            #line 78 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FormatCurrency(currency, c.Total)));
            
            #line default
            #line hidden
            this.Write("</span>\r\n            </td>\r\n          </tr>\r\n        </table>\r\n\t</div>\r\n\t");
            
            #line 83 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("\t<div id=\"info4\"><span id=\"info51\">Status: ");
            
            #line 84 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i.s));
            
            #line default
            #line hidden
            this.Write("</span></div>\r\n\t<div id=\"info5\"><a id=\"info61\">Total: ");
            
            #line 85 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(FormatCurrency(currency, list.Sum(c => c.Total))));
            
            #line default
            #line hidden
            this.Write("</a></div>\r\n");
            
            #line 86 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 87 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"

foreach(var i in new [] {
	new { q = (items.Where(c=>c.CampaignStatus != "Verified")), s = "Not Yet Final" }
}) {
	var list = i.q.ToList();
	if(list.Count == 0) continue;
	foreach(var c in i.q) {
	
            
            #line default
            #line hidden
            this.Write("    <table class=\"lineitems\"><tr><td>");
            
            #line 95 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(i.s));
            
            #line default
            #line hidden
            this.Write("</td><td>");
            
            #line 95 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(c.CampaignName));
            
            #line default
            #line hidden
            this.Write("</td></tr></table>\r\n\t");
            
            #line 96 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
}
            
            #line default
            #line hidden
            
            #line 97 "C:\zzzNovEOM\vs2\EomApps\EomApp1\Formss\PubRep1\Controls\PubRepTemplate.tt"
}
            
            #line default
            #line hidden
            this.Write("    </div>\r\n  </body>\r\n</html>");
            return this.GenerationEnvironment.ToString();
        }
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "10.0.0.0")]
    public class PubRepTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}