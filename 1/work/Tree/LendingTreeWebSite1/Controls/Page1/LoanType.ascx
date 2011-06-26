<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LoanType.ascx.cs" Inherits="Controls_Page1_LoanType" %>
<div style="margin-bottom: 18px; margin-left: 80px; width: 350px;">
    <%-- Label --%>
    <div style="padding-left: 0pt; display: inline;">
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String4 %>" />
    </div>
    <%-- Choice List --%>
    <div style="padding-left: 8pt; display: inline;">
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
            DataTextField="text" DataValueField="value">
        </asp:DropDownList>
    </div>
</div>
<%-- Data Source --%>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='LoanType']/choice">
</asp:XmlDataSource>
