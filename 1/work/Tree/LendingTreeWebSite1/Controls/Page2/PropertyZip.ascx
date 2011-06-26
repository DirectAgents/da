<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PropertyZip.ascx.cs" Inherits="Controls_Page2_PropertyZip" %>
<div class="dropdown-item">
    <div class="question">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String9 %>" />
    </div>
    <%-- Text Box --%>
    <div class="answer">
        <asp:TextBox ID="TextBox1" Text="enter property ZIPCODE" runat="server"></asp:TextBox>
        <%-- Validation --%>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1"
            ErrorMessage="Please enter a 5 digit zip code." runat="server"
            ValidationExpression="\d{5}?">
            <div id="validation">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String12 %>" />
            </div>
        </asp:RegularExpressionValidator>
        <span id="statename"></span>
    </div>
    <script type="text/javascript">
        function zipCheck(component, text) {
            CheckValue(component, text);
            if (document.getElementById('<%=ZipValidatorClientID%>').isvalid) {
                $.post("Handler.ashx", "zip=" + component.value, function (resp) { $("#statename").text(resp); }, "html");
            }
        }
    </script>
</div>
