<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PurchasePrice.ascx.cs" Inherits="Controls_Page2_PurchasePrice" %>

<div class="dropdown-item">

<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String67 %>" />
</div>
<%-- Choice List --%>
<div class="answer">
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>

<%-- Data Source --%>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='PurchasePrice']/choice"></asp:XmlDataSource>
<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList1">
<div id="validation"> 
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String68 %>" />
</div>
</asp:RequiredFieldValidator>

</div>


</div>