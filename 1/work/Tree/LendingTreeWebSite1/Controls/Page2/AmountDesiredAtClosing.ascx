<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AmountDesiredAtClosing.ascx.cs" Inherits="Controls_Page2_AmountDesiredAtClosing" %>
<%@ Register src="../Shared/OkToEstimate.ascx" tagname="OkToEstimate" tagprefix="uc1" %>

<div class="dropdown-item">

<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String18 %>" />
</div>
<%-- Choice List --%>
<div class="answer">
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>
<div class="ok">
<uc1:OkToEstimate ID="OkToEstimate1" runat="server" />
</div>
</div>

</div>

<%-- Data Source --%>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='AmountDesiredAtClosing']/choice"></asp:XmlDataSource>
