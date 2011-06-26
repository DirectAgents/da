<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VetranStatus.ascx.cs" Inherits="Controls_Shared_VetranStatus" %>
<div class="dropdown-item">
<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String64 %>" />
</div>
<%-- Radio List --%>
<div class="answer">

<asp:RadioButtonList ID="RadioButtonList1" runat="server" >

   <asp:ListItem Text="<%$ Resources:String, String65 %>" Value="Y" />
   <asp:ListItem Text="<%$ Resources:String, String66 %>" Value="N" Selected="True" />
   
</asp:RadioButtonList>


</div>

</div>