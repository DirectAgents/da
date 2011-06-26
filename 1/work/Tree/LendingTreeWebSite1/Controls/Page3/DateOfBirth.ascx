<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateOfBirth.ascx.cs" Inherits="Controls_Page3_DateOfBirth" %>
 <div class="enter-item inline">
  <div class="question">
<%-- Label --%>
<asp:Label ID="Label1" Text="<%$ Resources:String, String33 %>" runat="server" />
</div>
<%-- Text Box --%>
<div class="answer">

<asp:TextBox ID="TextBox1" Text="<%$ Resources:String, String54 %>" runat="server"></asp:TextBox>
<%-- Validation --%>

<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="<%$ Resources:String, String55 %>"
    ControlToValidate="TextBox1" class="validation" runat="server" ValidationExpression="^[0-9]{2}/[0-9]{2}/[0-9]{4}$">

</asp:RegularExpressionValidator>

</div>
</div>