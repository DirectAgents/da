/*
Copyright (c) 2009 Bill Davidsen (wdavidsen@yahoo.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.Design;

namespace SCS.Web.UI.WebControls.Design
{
    public class SplitterPanelDesigner : ControlDesigner
    {
        public override void Initialize(IComponent Component)
        {
            base.Initialize(Component);
            SetViewFlags(ViewFlags.TemplateEditing, true);
        }

//        public override string GetDesignTimeHtml()
//        {
//            string html =  
//                @"<div class=""SCS_splitter"" style=""display:block;top:0;"">
//	                <div class=""leftSection"" style=""width:300px;display:block;"">
//                		
//	                </div>
//                    <div class=""divider"" style=""left:300px;"">
//		                <div style=""position:absolute;top:50%;""></div>
//	                </div>
//	                <div class=""rightSection"">                		      
//                            
//	                </div>
//                </div>" ;

//            return html;
//        }

        public override string GetDesignTimeHtml()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            Stream resourceStream = assem.GetManifestResourceStream("SCS.Web.UI.WebControls.SplitterPanel.css");
            StreamReader resourceReader = new StreamReader(resourceStream);

            string style = resourceReader.ReadToEnd();

            Regex expr = new Regex(@"(<%=)\s*(WebResource\("")(?<resourceName>.+)\s*(""\)%>)");
            style = string.Format("<style>{0}</style>", expr.Replace(style, new MatchEvaluator(PerformSubstitution)));

            style += base.GetDesignTimeHtml();
            return style;
        }

        private string PerformSubstitution(Match m)
        {
            SplitterPanel splitter = (SplitterPanel)Component;
            
            string replacedString = m.ToString();
            replacedString = replacedString.Replace(m.Value, splitter.Page.ClientScript.GetWebResourceUrl(this.GetType(), m.Groups["resourceName"].Value));
            
            return replacedString;
        }

        public override TemplateGroupCollection TemplateGroups
        {
            get
            {
                TemplateGroupCollection collection = new TemplateGroupCollection();
                TemplateGroup group;                
                SplitterPanel control;

                control = (SplitterPanel)Component;
                group = new TemplateGroup("Splitter Panes");

                TemplateDefinition leftTemplate = new TemplateDefinition(this, "Left Pane", control, "LeftPane", false);
                group.AddTemplateDefinition(leftTemplate);

                TemplateDefinition rightTemplate = new TemplateDefinition(this, "Right Pane", control, "RightPane", false);
                group.AddTemplateDefinition(rightTemplate);

                collection.Add(group);
                return collection;
            }
        }
    }
}
