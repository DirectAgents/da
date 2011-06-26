<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LastName.ascx.cs" Inherits="Controls_Page3_LastName" %>
 <div class="enter-item inline">
<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String27 %>" />
</div>
<%-- Text Box --%>
<div class="answer">
<asp:TextBox ID="TextBox1" Text="<%$ Resources:String, String57 %>" runat="server"></asp:TextBox>
<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBox1"
    InitialValue="<%$ Resources:String, String57 %>">
    <div id="validation">
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String39 %>" />
    </div>
</asp:RequiredFieldValidator>
</div>
</div>