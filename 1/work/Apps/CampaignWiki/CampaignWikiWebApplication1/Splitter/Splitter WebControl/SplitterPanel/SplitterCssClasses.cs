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
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using SCS.Web.UI.WebControls.Design;

namespace SCS.Web.UI.WebControls
{
    [TypeConverter(typeof(SplitterCssClassConverter))]
    public class SplitterCssClasses : IStateManager
    {
        #region Fields

        private bool _isTrackingViewState = false;
        private StateBag _viewState;

        private string _resizableArea;
        private string _leftPaneCssClass;
        private string _dividerCssClass;
        private string _rightPaneCssClass;

        #endregion

        public SplitterCssClasses()
        {
        }
        public SplitterCssClasses(string resizableAreaCssClass, string leftPaneCssClass, string dividerCssClass, string rightPaneCssClass)
        {
            ResizableAreaCssClass = resizableAreaCssClass;
            LeftPaneCssClass = leftPaneCssClass;
            DividerCssClass = dividerCssClass;
            RightPaneCssClass = rightPaneCssClass;
        }

        #region IStateManager Members

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return _isTrackingViewState;
            }
        }
        void IStateManager.LoadViewState(object state)
        {
            if (state != null)
            {
                if (state != null)
                    ((IStateManager)ViewState).LoadViewState(state);

                object resizableAreaCssClass = ViewState["ResizableAreaCssClass"];

                if (resizableAreaCssClass != null)
                    this.ResizableAreaCssClass = (string)resizableAreaCssClass;

                object leftPaneCssClass = ViewState["LeftPaneCssClass"];

                if (leftPaneCssClass != null)
                    this.LeftPaneCssClass = (string)leftPaneCssClass;

                object dividerCssClass = ViewState["DividerCssClass"];

                if (dividerCssClass != null)
                    this.DividerCssClass = (string)dividerCssClass;

                object rightPaneCssClass = ViewState["RightPaneCssClass"];

                if (rightPaneCssClass != null)
                    this.RightPaneCssClass = (string)rightPaneCssClass;
            }
        }
        object IStateManager.SaveViewState()
        {
            string currentResizableAreaCssClass = this.LeftPaneCssClass;
            string initialResizableAreaCssClass = (ViewState["ResizableAreaCssClass"] == null) ? "" : (string)ViewState["ResizableAreaCssClass"];

            if (currentResizableAreaCssClass.Equals(initialResizableAreaCssClass) == false)
            {
                ViewState["ResizableAreaCssClass"] = currentResizableAreaCssClass;
            }

            string currentLeftPaneCssClass = this.LeftPaneCssClass;
            string initialLeftPaneCssClass = (ViewState["LeftPaneCssClass"] == null) ? "" : (string)ViewState["LeftPaneCssClass"];

            if (currentLeftPaneCssClass.Equals(initialLeftPaneCssClass) == false)
            {
                ViewState["LeftPaneCssClass"] = currentLeftPaneCssClass;
            }

            string currentDividerCssClass = this.DividerCssClass;
            string initialDividerCssClass = (ViewState["DividerCssClass"] == null) ? "" : (string)ViewState["DividerCssClass"];

            if (currentDividerCssClass.Equals(initialDividerCssClass) == false)
            {
                ViewState["DividerCssClass"] = currentDividerCssClass;
            }

            string currentRightPaneCssClass = this.RightPaneCssClass;
            string initialRightPaneCssClass = (ViewState["RightPaneCssClass"] == null) ? "" : (string)ViewState["RightPaneCssClass"];

            if (currentRightPaneCssClass.Equals(initialRightPaneCssClass) == false)
            {
                ViewState["RightPaneCssClass"] = currentRightPaneCssClass;
            }

            if (_viewState != null)
            {
                return ((IStateManager)_viewState).SaveViewState();
            }
            return null;
        }
        void IStateManager.TrackViewState()
        {
            if (ResizableAreaCssClass.Length > 0)
                ViewState["ResizableAreaCssClass"] = _resizableArea;

            if (LeftPaneCssClass.Length > 0)
                ViewState["LeftPaneCssClass"] = _leftPaneCssClass;

            if (DividerCssClass.Length > 0)
                ViewState["DividerCssClass"] = _dividerCssClass;

            if (RightPaneCssClass.Length > 0)
                ViewState["RightPaneCssClass"] = _rightPaneCssClass;

            _isTrackingViewState = true;

            if (_viewState != null)
            {
                ((IStateManager)_viewState).TrackViewState();
            }
        }

        #endregion

        #region Properties

        protected StateBag ViewState
        {
            get
            {
                if (_viewState == null)
                {
                    _viewState = new StateBag(false);

                    if (_isTrackingViewState)
                        ((IStateManager)_viewState).TrackViewState();
                }
                return _viewState;
            }
        }
        internal void SetDirty()
        {
            if (_viewState != null)
            {
                ICollection Keys = _viewState.Keys;
                foreach (string key in Keys)
                {
                    _viewState.SetItemDirty(key, true);
                }
            }
        }

        [NotifyParentProperty(true), DefaultValue("")]
        public string ResizableAreaCssClass
        {
            get
            {
                return _resizableArea;
            }
            set
            {
                _resizableArea = value;
            }
        }

        [NotifyParentProperty(true), DefaultValue("")]
        public string LeftPaneCssClass
        {
            get
            {
                return _leftPaneCssClass;
            }
            set
            {
                _leftPaneCssClass = value;
            }
        }

        [NotifyParentProperty(true), DefaultValue("")]
        public string DividerCssClass
        {
            get
            {
                return _dividerCssClass;
            }
            set
            {
                _dividerCssClass = value;
            }
        }

        [NotifyParentProperty(true), DefaultValue("")]
        public string RightPaneCssClass
        {
            get
            {
                return _rightPaneCssClass;
            }
            set
            {
                _rightPaneCssClass = value;
            }
        }

        #endregion
    }
}
