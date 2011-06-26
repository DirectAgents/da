<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FirstName.ascx.cs" Inherits="Controls_Page3_FirstName" %>
 <div class="enter-item inline">
<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String26 %>" />
</div>
<%-- Text Box --%>
<div class="answer">
<asp:TextBox ID="TextBox1" Text="<%$ Resources:String, String45 %>" runat="server"></asp:TextBox>
<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
    InitialValue="<%$ Resources:String, String45 %>">
    <div id="validation"> 
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String38 %>" />
    </div>
</asp:RequiredFieldValidator>
</div>
</div>