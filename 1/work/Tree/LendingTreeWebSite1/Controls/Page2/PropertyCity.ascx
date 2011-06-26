<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PropertyCity.ascx.cs" Inherits="Controls_Page2_PropertyCity" %>

<div class="dropdown-item">

<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String71 %>" />
</div>
<%-- Text Box --%>
<div class="answer">
<asp:TextBox ID="TextBox1" runat="server" Text="<%$ Resources:String, String73 %>"></asp:TextBox>
<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1">
<div id="validation"> 
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String72 %>" />
</div>
</asp:RequiredFieldValidator>
</div>
</div>