<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhoneNumber.ascx.cs" Inherits="Controls_Shared_PhoneNumber" %>
<%-- Text Box --%>
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<%-- Validation --%>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="*"
    ControlToValidate="TextBox1" runat="server" class="validation" ValidationExpression="<%$ Resources:Regex, PhoneNumber %>">
</asp:RegularExpressionValidator>
