<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SocialSecurityNumber.ascx.cs" Inherits="Controls_Page3_SocialSecurityNumber" %>
<%@ Register src="../Shared/Optional.ascx" tagname="Optional" tagprefix="uc1" %>


 <div class="tipTeaser">
    <h4 class="green">NOT REQUIRED, but providing this final detail could help you save!</h4>
 </div>

 <div class="enter-item inline">
 
 
 
  <div class="question">
<%-- Label --%>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String48 %>" />
</div>
<%-- Text Box --%>
<div class="answer">
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
<span class="smallLabel lock inline"><img src="./images/small-lock.png" /></span>
<div class="ok">
<uc1:Optional ID="Optional1" runat="server" />
</div>
<%-- Validation --%>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="<%$ Resources:String, String56 %>"
    ControlToValidate="TextBox1" runat="server" ValidationExpression="^\d{3}-\d{2}-\d{4}$">
</asp:RegularExpressionValidator>
</div>
</div>