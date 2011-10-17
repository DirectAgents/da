 <%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register assembly="SCS.Web.UI.WebControls.SplitterPanel" namespace="SCS.Web.UI.WebControls" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="SplitterPanel.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        
        <cc1:SplitterPanel ID="SplitterPanel1" runat="server" 
            LeftPaneMaxPixelWidth="800"
            LeftPaneMinPixelWidth="150"
            LeftPanePixelWidth="450"            
            BehaviorId="splitter" 
            RedrawWhileDragging="false"             
            CssClass="splitter">

            <CssClasses ResizableAreaCssClass="resizableArea" LeftPaneCssClass="leftPane" DividerCssClass="divider" RightPaneCssClass="rightPane"  />
                      
            <RightPane>
                <asp:Button ID="Poster" runat="server" Text="" />
                
                <div style="float:right" id="resizeMsg"></div>
            </RightPane>
            <LeftPane>
                <div id="splitter-trace"></div>  
            </LeftPane>
        </cc1:SplitterPanel>      
        

    <script type="text/javascript">
        var _resizeMsgDiv = $get("resizeMsg");
        
        function pageLoad() {

            var splitter = $find("splitter");

            if (splitter) {

                splitter.add_dragging(splitter_onDragging);
                splitter.add_resizing(splitter_onResizing);
                splitter.add_toggling(splitter_onToggling);

                splitter.add_resized(splitter_onResized);
                splitter.add_toggled(splitter_onToggled);
            }
        }

        function splitter_onDragging(e, args) {

            var maxWidth = e.get_leftMaxWidth();
            var minWidth = e.get_leftMinWidth();
            var newWidth = args.get_leftWidth();
            var msg;

            if (parseInt(maxWidth) == parseInt(newWidth))
                msg = "Cannot exceed maximum of " + newWidth + ".";
            else if (parseInt(minWidth) == parseInt(newWidth))
                msg = "Cannot go below minimum width of " + minWidth + ".";
            else
                msg = "Pending Left Pane Width: " + newWidth;

            _resizeMsgDiv.innerHTML = msg;
        }
        
        function splitter_onResizing(e, args) {

            //args.set_cancel(!confirm("Do you want to resize the left pane to " + args.get_leftWidth()  + "?"));
        }

        function splitter_onToggling(e, args) {

            //args.set_cancel(!confirm("Do you want to toggle?"));
        }

        function splitter_onResized(e, args) {

            //alert("Left pane was resized. ");
        }

        function splitter_onToggled(e, args) {

            //alert("Splitter was toggled.");
        }
    
    </script>  
    
    <asp:scriptmanager ID="Scriptmanager1" runat="server">
        </asp:scriptmanager>
    </form>
</body>
</html>
