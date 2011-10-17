/// <reference name="MicrosoftAjax.js"/>

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

Type.registerNamespace("SCS");

SCS.Splitter = function (element) {
    SCS.Splitter.initializeBase(this, [element]);

    this._isInInit = true;
    this._dragging = false;
    this._redrawWhileDragging = false;
    this._isExpanded = true;
    this._leftPosition = 300;
    this._prevLeftPosition;
    this._leftSnap = 30;
    this._leftMinWidth = 0;
    this._leftMaxWidth = -1;

    this._paneContainer;
    this._leftPane;
    this._rightPane;
    this._divider;
    this._stateHolder;

    this._mouseUpHandler;
    this._mouseDownHandler;
    this._doubleClickHandler;

    this._dragHandler;
    this._selectStartHandler;

    this._docMouseUpHandler;
    this._docMouseMoveHandler;
    this._docKeyPressHandler;
    this._resizeHandler;
};

SCS.Splitter.prototype = {
    initialize: function () {
        SCS.Splitter.callBaseMethod(this, 'initialize');

        this._paneContainer = this._getFirstChild(this.get_element());
        this._leftPane = this._getFirstChild(this._paneContainer);
        this._divider = this._getNextSibling(this._leftPane);
        this._rightPane = this._getNextSibling(this._divider);
        this._stateHolder = this._getNextSibling(this._rightPane);

        this._docKeyPressHandler = Function.createDelegate(this, this._onDocKeyPress);
        $addHandler(document, "keypress", this._docKeyPressHandler);

        this._mouseUpHandler = Function.createDelegate(this, this._onMouseUp);
        $addHandler(this._divider, "mouseup", this._mouseUpHandler);

        this._mouseDownHandler = Function.createDelegate(this, this._onMouseDown);
        $addHandler(this._divider, "mousedown", this._mouseDownHandler);

        this._doubleClickHandler = Function.createDelegate(this, this._onDoubleClick);
        $addHandler(this._divider, "dblclick", this._doubleClickHandler);

        this._dragHandler = Function.createDelegate(this, this._onDrag);
        $addHandler(document.body, "drag", this._dragHandler);

        this._selectStartHandler = Function.createDelegate(this, this._onStart);
        $addHandler(document.body, "selectstart", this._selectStartHandler);

        this._resizeHandler = Function.createDelegate(this, this._onResize);
        $addHandler(window, "resize", this._resizeHandler);

        this._onResize();
        this._redaw(this._leftPosition);

        this._isInInit = false;
    },
    dispose: function () {

        try {

            $addHandler(document, "keypress", this._docKeyPressHandler);
            this._docKeyPressHandler = null;

            $removeHandler(this._divider, "mouseup", this._mouseUpHandler);
            this._mouseUpHandler = null;

            $removeHandler(this._divider, "mousedown", this._mouseDownHandler);
            this._mouseDownHandler = null;

            $removeHandler(this._divider, "dblclick", this._doubleClickHandler);
            this._doubleClickHandler = null;

            $removeHandler(document.body, "drag", this._dragHandler);
            this._dragHandler = null;

            $removeHandler(document.body, "selectstart", this._selectStartHandler);
            this._selectStartHandler = null;

            $removeHandler(window, "resize", this._resizeHandler);
            this._resizeHandler = null;

            SCS.Splitter.callBaseMethod(this, 'dispose');
        }
        catch (e) {

        }
    },
    _onMouseUp: function (e) {

        //Sys.Debug.trace("Mouse Up");
        this._stopDragging();
    },
    _onMouseDown: function (e) {

        e.preventDefault();

        //Sys.Debug.trace("Mouse Down - Start dragging");
        if (this._dragging) {

            //Sys.Debug.trace("Mouse Down - Already dragging - Exit");
            this._stopDragging();
            return;
        }

        this._dragging = true;
        this._divider.parrentOffsetX = this._getElementLeft(this._divider.parentNode) + e.clientX - this._getElementLeft(this._divider);

        this._docMouseMoveHandler = Function.createDelegate(this, this._onDocMouseMove);
        $addHandler(document, "mousemove", this._docMouseMoveHandler);

        //Sys.Debug.trace("Wired mousemove event");

        this._docMouseUpHandler = Function.createDelegate(this, this._onDocMouseUp);
        $addHandler(document, "mouseup", this._docMouseUpHandler);

        //Sys.Debug.trace("Wired mouseup event");
    },
    _onDocKeyPress: function (e) {

        if (e.charCode == 116) {
            var target = e.target;

            if (target.tagName.toLowerCase() != "input" && target.tagName.toLowerCase() != "textarea")
                this.toggleDivider();

            e.stopPropagation();
        }
        else if (e.charCode == 27) {

            this._stopDragging();
        }
    },
    _onDoubleClick: function (e) {
        this.toggleDivider();
    },
    _onDrag: function (e) {
        return !this._dragging;
    },
    _onStart: function (e) {
        return !this._dragging;
    },
    _onDocMouseUp: function (e) {
        this._onMouseUp(e);
    },
    _onDocMouseMove: function (e) {
        this._mouseMove(e);
    },
    _onResize: function (e) {

        var browser = Sys.Browser;

        if (browser.InternetExplorer) {

            var ver = browser.version;

            if (ver >= 7 && ver < 8) {

                if (this._paneContainer.offsetHeight == document.documentElement.clientHeight)
                    this._paneContainer.style.height = document.documentElement.clientHeight;
                else
                    this._paneContainer.style.height = this.get_element().offsetHeight;
            }
            //else if (ver >= 6 && ver < 7) {
            // handle ie6 issues here
            //}
        }
    },
    _updateState: function () {

        this._stateHolder.value = String.format("{0},{1}", this._leftPosition, this._isExpanded);
    },
    toggleDivider: function () {

        var eventArgs = new SCS.SplitterTogglingEventArgs(this._isExpanded);
        this.raiseToggling(eventArgs);

        if (eventArgs.get_cancel())
            return;

        this._openCollapse();

        this.raiseToggled(Sys.EventArgs.Empty);
    },

    _stopDragging: function () {

        if (this._docMouseMoveHandler) {
            $removeHandler(document, "mousemove", this._docMouseMoveHandler);
            this._docMouseMoveHandler = null;

            //Sys.Debug.trace("Removed mousemove event");
        }

        if (this._docMouseUpHandler) {
            $removeHandler(document, "mouseup", this._docMouseUpHandler);
            this._docMouseUpHandler = null;

            //Sys.Debug.trace("Removed mouseup event");
        }

        this._dragging = false;
        var leftPixels = this._divider.offsetLeft;

        var eventArgs = new SCS.SplitterResizingEventArgs(leftPixels);
        this.raiseResizing(eventArgs);

        if (eventArgs.get_cancel())
            leftPixels = this._leftPosition;

        this._redaw(leftPixels);

        if (!eventArgs.get_cancel())
            this.raiseResized(Sys.EventArgs.Empty);
    },

    _redaw: function (width) {

        //Sys.Debug.trace("Redraw");

        if (this._leftMinWidth < 1 && width <= this._leftSnap) {
            this._leftPane.style.display = "none";
            this._divider.style.left = "0px";
            this._isExpanded = false;
        }
        else {
            this._leftPane.style.display = (width > 0) ? "block" : "none";
            this._divider.style.left = width + "px";
            this._leftPane.style.width = width + "px";
            this._isExpanded = true;
        }

        this._prevLeftPosition = this._leftPosition;
        this._leftPosition = width;
        this._updateState();
    },
    _mouseMove: function (e) {

        //Sys.Debug.trace("Mouse Move - Dragging divider");

        var pixels = e.clientX - this._divider.parrentOffsetX;

        if (this._leftMinWidth < 1 && pixels <= this._leftSnap) {
            pixels = 0;
        }
        else if (pixels <= this._leftMinWidth) {
            pixels = this._leftMinWidth;
        }
        else if (this._leftMaxWidth != -1 && pixels >= this._leftMaxWidth) {
            pixels = this._leftMaxWidth;
        }

        var eventArgs = new SCS.SplitterDraggingEventArgs(pixels);
        this.raiseDragging(eventArgs);

        if (eventArgs.get_cancel())
            return;

        this._divider.style.left = pixels + "px";

        if (this._redrawWhileDragging)
            this._redaw(this._divider.offsetLeft);

        return false;
    },
    _getFirstChild: function (obj) {

        obj = obj.firstChild;
        while (obj.nodeType != 1)
            obj = obj.nextSibling;

        return obj;
    },
    _getNextSibling: function (obj) {

        obj = obj.nextSibling;
        while (obj.nodeType != 1)
            obj = obj.nextSibling;

        return obj;
    },
    _getElementLeft: function (obj) {
        var x = 0;

        while (obj != null) {
            x += obj.offsetLeft;
            obj = obj.offsetParent;
        }
        return x;
    },
    _openCollapse: function () {

        this._redaw((this._divider.offsetLeft == 0) ? this._prevLeftPosition : 0);
    },

    _trace: function (msg) {

        var output = $get("splitter-trace");

        if (output)
            output.innerHTML = msg + "<br/><br/>" + output.innerHTML;
    },

    changeTopPosition: function (value) {

        if (!value.toString().toLowerCase().endsWith("px"))
            value = value + "px";

        this.get_element().style.top = value;
    },

    // Toggle Events
    add_toggling: function (handler) {

        this.get_events().addHandler('toggling', handler);
    },
    remove_toggling: function (handler) {

        this.get_events().removeHandler('toggling', handler);
    },
    raiseToggling: function (eventArgs) {

        var handler = this.get_events().getHandler('toggling');
        if (handler) {
            handler(this, eventArgs);
        }
    },
    add_toggled: function (handler) {

        this.get_events().addHandler('toggled', handler);
    },
    remove_toggled: function (handler) {

        this.get_events().removeHandler('toggled', handler);
    },
    raiseToggled: function (eventArgs) {

        var handler = this.get_events().getHandler('toggled');
        if (handler) {
            handler(this, eventArgs);
        }
    },
    // Dragging Event
    add_dragging: function (handler) {

        this.get_events().addHandler('dragging', handler);
    },
    remove_dragging: function (handler) {

        this.get_events().removeHandler('dragging', handler);
    },
    raiseDragging: function (eventArgs) {

        var handler = this.get_events().getHandler('dragging');
        if (handler) {
            handler(this, eventArgs);
        }
    },
    // Resize Events
    add_resizing: function (handler) {

        this.get_events().addHandler('resizing', handler);
    },
    remove_resizing: function (handler) {

        this.get_events().removeHandler('resizing', handler);
    },
    raiseResizing: function (eventArgs) {

        var handler = this.get_events().getHandler('resizing');
        if (handler) {
            handler(this, eventArgs);
        }
    },
    add_resized: function (handler) {

        this.get_events().addHandler('resized', handler);
    },
    remove_resized: function (handler) {

        this.get_events().removeHandler('resized', handler);
    },
    raiseResized: function (eventArgs) {

        var handler = this.get_events().getHandler('resized');
        if (handler) {
            handler(this, eventArgs);
        }
    },
    // Properties
    get_leftPosition: function () {
        return this._leftPosition;
    },
    set_leftPosition: function (value) {

        if (!this._isInInit && (value != this._leftPosition) && (value < this._leftMaxWidth) && (value > this._leftMinWidth))
            this._redaw(value);

        this._prevLeftPosition = this._leftPosition;
        this._leftPosition = value;
    },
    get_leftSnap: function () {
        return this._leftSnap;
    },
    set_leftSnap: function (value) {
        this._leftSnap = value;
    },
    get_leftMinWidth: function () {
        return this._leftMinWidth;
    },
    set_leftMinWidth: function (value) {
        this._leftMinWidth = value;
    },
    get_leftMaxWidth: function () {
        return this._leftMaxWidth;
    },
    set_leftMaxWidth: function (value) {
        this._leftMaxWidth = value;
    },
    set_isExpanded: function (value) {

        if (!this._isInInit && value != this._isExpanded) {
            this._openCollapse();
        }

        this._isExpanded = value;
    },
    get_isExpanded: function () {
        return this._isExpanded;
    },
    get_redrawWhileDragging: function () {
        return this._redrawWhileDragging;
    },
    set_redrawWhileDragging: function (value) {
        this._redrawWhileDragging = value;
    }
};

SCS.Splitter.registerClass('SCS.Splitter', Sys.UI.Behavior);

SCS.SplitterTogglingEventArgs = function (expanded) {

    SCS.SplitterTogglingEventArgs.initializeBase(this);

    this._expanded = expanded;
};

SCS.SplitterTogglingEventArgs.prototype = {
    get_expanded: function () {

        return this._expanded;
    },
    set_expanded: function (value) {
        this._expanded = value;
    }
};

SCS.SplitterTogglingEventArgs.registerClass('SCS.SplitterTogglingEventArgs', Sys.CancelEventArgs);

SCS.SplitterResizingEventArgs = function (leftWidth) {

    SCS.SplitterResizingEventArgs.initializeBase(this);

    this._leftWidth = leftWidth;
};

SCS.SplitterResizingEventArgs.prototype = {

    get_leftWidth: function () {

        return this._leftWidth;
    },
    set_leftWidth: function (value) {

        this._leftWidth = value;
    }
};

SCS.SplitterResizingEventArgs.registerClass('SCS.SplitterResizingEventArgs', Sys.CancelEventArgs);

SCS.SplitterDraggingEventArgs = function (leftWidth) {

    SCS.SplitterDraggingEventArgs.initializeBase(this);

    this._leftWidth = leftWidth;
};

SCS.SplitterDraggingEventArgs.prototype = {

    get_leftWidth: function () {

        return this._leftWidth;
    },
    set_leftWidth: function (value) {

        this._leftWidth = value;
    }
};

SCS.SplitterDraggingEventArgs.registerClass('SCS.SplitterDraggingEventArgs', Sys.CancelEventArgs);

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
