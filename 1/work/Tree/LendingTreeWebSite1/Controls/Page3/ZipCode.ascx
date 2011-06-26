<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ZipCode.ascx.cs" Inherits="Controls_Page3_ZipCode" %>
<div class="enter-item inline">
    <div class="question">
        <%-- Label --%>
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String29 %>" />
    </div>
    <%-- Text Box --%>
    <div class="answer">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <%-- Validation --%>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1"
            runat="server" ValidationExpression="\d{5}(-\d{4})?">
            <div id="validation">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String12 %>" />
            </div>
        </asp:RegularExpressionValidator>
    </div>
</div>
