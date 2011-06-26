<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HadForeclosures.ascx.cs" Inherits="Controls_Page2_HadForeclosures" %>

<div class="dropdown-item">

<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String21 %>" />
</div>
<%-- Choice List --%>
<div class="answer">
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>
</div>
<%-- Data Source --%>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='HadForeclosures']/choice"></asp:XmlDataSource>
    
</div>    