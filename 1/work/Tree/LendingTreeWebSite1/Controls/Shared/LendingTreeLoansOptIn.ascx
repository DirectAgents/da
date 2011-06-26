<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LendingTreeLoansOptIn.ascx.cs" Inherits="Controls_Shared_LendingTreeLoansOptIn" %>


<%-- Label --%>
   <div class="bottom-description">
                  <p>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String23 %>" />

<%-- Link --%>
<asp:HyperLink ID="HyperLink1" runat="server" Text="<%$ Resources:String, String24 %>"
    NavigateUrl="http://www.lendingtree.com/stmrc/pdfs/HLCDisclosureForm.pdf" />
<%-- Label --%>

<asp:Label ID="Label2" runat="server" Text="<%$ Resources:String, String25 %>" />
</p>



<%-- Radio List --%>
 <div class="radioContainer">
<asp:RadioButtonList ID="RadioButtonList1" runat="server" >
    <asp:ListItem Text="<%$ Resources:String, String62 %>" Value="Y" Selected="True" />
    <asp:ListItem Text="<%$ Resources:String, String63 %>" Value="N" />
</asp:RadioButtonList>
</div>

</div>