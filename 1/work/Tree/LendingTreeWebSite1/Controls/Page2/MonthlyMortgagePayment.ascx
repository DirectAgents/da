<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MonthlyMortgagePayment.ascx.cs" Inherits="Controls_Page2_MonthlyMortgagePayment" %>
<%@ Register Src="../Shared/OkToEstimate.ascx" TagName="OkToEstimate" TagPrefix="uc1" %>

<div class="dropdown-item">

<%-- Label --%>
<div class="question">
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String19 %>" />
</div>
<div class="answer">
<%-- Choice List --%>
<asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
    DataTextField="text" DataValueField="value">
</asp:DropDownList>
<div class="ok">
<uc1:OkToEstimate ID="OkToEstimate1" runat="server" />
</div>
<%-- Validation --%>
<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList1">
<div id="validation"> 
    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String15 %>" />
</div>
</asp:RequiredFieldValidator><%-- Data Source --%><asp:XmlDataSource ID="XmlDataSource1"
    runat="server" DataFile="~/App_Data/Data.xml" TransformFile="~/App_Data/Transform.xslt"
    XPath="data/options[@question='MonthlyMortgagePayment']/choice"></asp:XmlDataSource>
</div>
</div>