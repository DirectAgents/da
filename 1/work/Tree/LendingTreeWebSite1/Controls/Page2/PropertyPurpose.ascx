<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PropertyPurpose.ascx.cs" Inherits="Controls_Page2_PropertyPurpose" %>
<%-- Label --%>
<div class="dropdown-item">
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String8 %>" />
</div>
<%-- Choice List --%>
<div class="answer">
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>
</div>
<%-- Data Source --%>
</div>

<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='PropertyPurpose']/choice"></asp:XmlDataSource>