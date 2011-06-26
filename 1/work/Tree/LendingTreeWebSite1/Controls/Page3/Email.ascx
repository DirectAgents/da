<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Email.ascx.cs" Inherits="Controls_Page3_Email" %>
 <div class="enter-item inline">
<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String30 %>" />
</div>
<%-- Text Box --%>
<div class="answer">
<asp:TextBox ID="TextBox1" Text="<%$ Resources:String, String47 %>" runat="server"></asp:TextBox>

<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
    Display="Dynamic" InitialValue="<%$ Resources:String, String47 %>">
    <div id="validation">
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String60 %>" />
    </div>
</asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="TextBox1"
    Display="Dynamic" runat="server" ValidationExpression="<%$ Resources:Regex, Email %>">
    <div id="validation">
    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:String, String59 %>" />
    </div>
</asp:RegularExpressionValidator>
</div>
</div>