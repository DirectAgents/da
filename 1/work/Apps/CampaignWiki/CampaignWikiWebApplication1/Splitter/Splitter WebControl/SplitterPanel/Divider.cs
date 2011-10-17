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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Design;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCS.Web.UI.WebControls
{
    internal class Divider : Panel
    {
        public Divider() : base()
        {
            EnableViewState = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (RootControl.IsExpanded)
            {
                Style[HtmlTextWriterStyle.Left] = ((SplitterPanel)Parent).LeftPanePixelWidth.ToString() + "px";
            }
            else
            {
                Style[HtmlTextWriterStyle.Left] = "0px";
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.CssClass = RootControl.CssClasses.DividerCssClass;
            base.Render(writer);
        }

        protected override void RenderChildren(HtmlTextWriter writer)
        {
            base.RenderChildren(writer);

            //writer.AddAttribute(HtmlTextWriterAttribute.Style, "position:absolute;top:50%;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.RenderEndTag();
        }
        
        public SplitterPanel RootControl
        {
            get { return (SplitterPanel)Parent; }
        }
    }
}
