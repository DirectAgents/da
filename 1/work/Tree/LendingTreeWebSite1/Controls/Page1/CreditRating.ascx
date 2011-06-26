<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CreditRating.ascx.cs" Inherits="Controls_Page1_CreditRating" %>

<div style="margin-bottom:18px; margin-left:77px; width:350px;">

<%-- Label --%>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String1 %>" />
<%-- Choice List --%>
<div style="padding-left: 10pt; display:inline;">
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>
</div>
<%-- Data Source --%>

</div>

<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='CreditRating']/choice"></asp:XmlDataSource>
