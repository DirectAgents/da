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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SCS.Web.UI.WebControls
{
    #region Class Attributes
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:SplitterPanel runat=server></{0}:SplitterPanel>")]
    [ParseChildren(true), PersistChildren(false)]
    [Designer(typeof(SCS.Web.UI.WebControls.Design.SplitterPanelDesigner))]
    [Themeable(true)]
    #endregion

    public class SplitterPanel : CompositeControl
    {
        #region Fields
        private ITemplate _leftTemplateValue;
        private TemplateOwner _leftOwnerValue;

        private ITemplate _rightTemplateValue;
        private TemplateOwner _rightOwnerValue;

        private Pane _leftPanel;
        private Divider _dividerPanel;
        private Pane _rightPanel;
        private StateHolder _stateHolder;

        private SplitterCssClasses _splitterCssClasses;

        private static string createParamsJson = @"{{""id"": ""{0}"", ""leftPosition"": {1}, ""leftSnap"": {2}, ""leftMinWidth"": {3}, ""leftMaxWidth"": {4}, ""redrawWhileDragging"": {5} }}";
     
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            EnsureChildControls();
        }

        protected override void OnPreRender(EventArgs e)
        {
            // add client scripts to page
            SetupClientApi();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            // left pane
            _leftPanel = new Pane(PaneLocationType.Left);
            Controls.Add(_leftPanel);

            // left template
            _leftOwnerValue = new TemplateOwner();

            ITemplate leftTemp = _leftTemplateValue;
            if (leftTemp == null)
                leftTemp = new DefaultTemplate();

            leftTemp.InstantiateIn(_leftOwnerValue);
            _leftPanel.Controls.Add(_leftOwnerValue);

            // divider 
            _dividerPanel = new Divider();
            Controls.Add(_dividerPanel);

            // right pane
            _rightPanel = new Pane(PaneLocationType.Right);
            Controls.Add(_rightPanel);

            _rightOwnerValue = new TemplateOwner();

            ITemplate rightTemp = _rightTemplateValue;
            if (rightTemp == null)
                rightTemp = new DefaultTemplate();

            rightTemp.InstantiateIn(_rightOwnerValue);
            _rightPanel.Controls.Add(_rightOwnerValue);

            _stateHolder = new StateHolder();
            _stateHolder.ID = this.ID + "_state";
            _stateHolder.ServerChange += new EventHandler(_stateHolder_TextChanged);
            Controls.Add(_stateHolder);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
            {
                // the 100% html, body is not supported by the designer.
                // therefore set the height to something high.
                Style[HtmlTextWriterStyle.Height] = "600px"; 
            }
            
            base.Render(writer);
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClasses.ResizableAreaCssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);

            base.RenderContents(writer);

            writer.RenderEndTag();
        }

        protected virtual void SetupClientApi()
        {
//            if (!Util.IsAjaxInstalled)
//            {
//                throw new ApplicationException(@"The Ajax assembly System.Web.Extensions 
//                    is required by the client API but was not found.");
//            }

            ScriptManager manager = Util.GetScriptManager(this.Page);

            if (HttpContext.Current.IsDebuggingEnabled)
                manager.Scripts.Add(new ScriptReference("SCS.Web.UI.WebControls.SplitterBehavior.debug.js", "SCS.Web.UI.WebControls.SplitterPanel"));
            else
                manager.Scripts.Add(new ScriptReference("SCS.Web.UI.WebControls.SplitterBehavior.js", "SCS.Web.UI.WebControls.SplitterPanel"));

            string jsonProps = string.Format(createParamsJson, BehaviorId, LeftPanePixelWidth, 
                LeftSnapPixels, LeftPaneMinPixelWidth, LeftPaneMaxPixelWidth, RedrawWhileDragging.ToString().ToLower());

            string js =
            @"Sys.Application.add_load(splitterLoader);

            function splitterLoader() {{                    
                $create(SCS.Splitter, {0}, null, null, $get(""{1}""));   
            }}";

            js = string.Format(js, jsonProps, this.ClientID);

            ScriptManager.RegisterStartupScript(this, typeof(SplitterPanel), this.UniqueID + "_ClientCode", js, true);            
        }
        private void _stateHolder_TextChanged(object sender, EventArgs e)
        {
            if (_stateHolder.Value.Length > 0)
            {
                string[] data = _stateHolder.Value.Split(',');

                if (data.Length == 2) 
                {
                    LeftPanePixelWidth = int.Parse(data[0]);
                    IsExpanded = bool.Parse(data[1]);
                }
            }
        }

        #region Properties
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Div;
            }
        }

        #region Templates

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplateOwner LeftOwner
        {
            get
            {
                return _leftOwnerValue;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TemplateOwner RightOwner
        {
            get
            {
                return _rightOwnerValue;
            }
        }

        [Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), DefaultValue(typeof(ITemplate), "")]
        [Description("Control template"), TemplateContainer(typeof(SplitterPanel))]
        public virtual ITemplate LeftPane
        {
            get
            {
                return _leftTemplateValue;
            }
            set
            {
                _leftTemplateValue = value;
            }
        }

        [Browsable(false), PersistenceMode(PersistenceMode.InnerProperty), DefaultValue(typeof(ITemplate), "")]
        [Description("Control template"), TemplateContainer(typeof(SplitterPanel))]
        public virtual ITemplate RightPane
        {
            get
            {
                return _rightTemplateValue;
            }
            set
            {
                _rightTemplateValue = value;
            }
        }

        #endregion

        #region Appearance

        [Category("Appearance"), Description("The Tooltip to apply to the the divider bar.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue("")]
        public virtual string DividerToolTip
        {
            get
            {
                return _dividerPanel.ToolTip;
            }
            set
            {
                EnsureChildControls();
                _dividerPanel.ToolTip = value;
            }
        }

        [Category("Appearance"), Description("The style to apply to each button."), Bindable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public SplitterCssClasses CssClasses
        {
            get
            {
                EnsureChildControls();

                if (_splitterCssClasses == null)
                    _splitterCssClasses = new SplitterCssClasses();

                return _splitterCssClasses;
            }
        }

        #endregion

        #region Layout

        [Category("Layout"), Description("The width of the left pane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(300)]
        public virtual int LeftPanePixelWidth
        {
            get
            {
                object o = ViewState["LeftPanePixelWidth"];

                if (o != null)
                    return (int)o;

                return 300;
            }
            set
            {
                ViewState["LeftPanePixelWidth"] = value;
            }
        }

        [Category("Layout"), Description("The top position of the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(typeof(Unit), "")]
        public virtual Unit TopPosition
        {
            get
            {
                if (Style[HtmlTextWriterStyle.Top] != null)
                    return Unit.Parse(Style[HtmlTextWriterStyle.Top]);
                else                    
                    return new Unit();
            }
            set
            {
                Style[HtmlTextWriterStyle.Top] = value.ToString();
            }
        }
        
        #endregion

        #region Behavior

        [Category("Behavior"), Description("The top position of the control.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(30)]
        public virtual int LeftSnapPixels
        {
            get
            {
                object o = ViewState["LeftSnapPixels"];

                if (o != null)
                    return (int)o;

                return 30;
            }
            set
            {
                ViewState["LeftSnapPixels"] = value;
            }
        }

        [Category("Behavior"), Description("The minimum width of the left pane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(0)]
        public virtual int LeftPaneMinPixelWidth
        {
            get
            {
                object o = ViewState["LeftPaneMinPixelWidth"];

                if (o != null)
                    return (int)o;

                return 0;
            }
            set
            {
                ViewState["LeftPaneMinPixelWidth"] = value;
            }
        }

        [Category("Behavior"), Description("The maximum width of the left pane.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(-1)]
        public virtual int LeftPaneMaxPixelWidth
        {
            get
            {
                object o = ViewState["LeftPaneMaxPixelWidth"];

                if (o != null)
                    return (int)o;

                return -1;
            }
            set
            {
                ViewState["LeftPaneMaxPixelWidth"] = value;
            }
        }

        [Category("Behavior"), Description("Whether or not to redraw screen while dragging divider.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(false), DefaultValue(false)]
        public virtual bool RedrawWhileDragging
        {
            get
            {
                object o = ViewState["RedrawWhileDragging"];

                if (o != null)
                    return (bool)o;

                return false;
            }
            set
            {
                ViewState["RedrawWhileDragging"] = value;
            }
        }

        [Category("Appearance"), Description("Whether or not panel is in its expanded state.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [NotifyParentProperty(true), PersistenceMode(PersistenceMode.Attribute), Themeable(true), DefaultValue(true)]
        public virtual bool IsExpanded
        {
            get
            {
                object o = ViewState["IsExpanded"];

                if (o != null)
                    return (bool)o;

                return true;
            }
            set
            {
                ViewState["IsExpanded"] = value;
            }
        }

        #endregion

        [Category("Misc"), Description("The ID used to reference the toolbar on the client."), Bindable(false), DefaultValue("ToolbarClient")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.Attribute)]
        public string BehaviorId
        {
            get
            {
                object o = ViewState["BehaviorId"];

                if (o != null)
                    return (string)o;

                return "ToolbarClient";
            }
            set
            {
                ViewState["BehaviorId"] = value;
            }
        }

        #endregion
    }
}
