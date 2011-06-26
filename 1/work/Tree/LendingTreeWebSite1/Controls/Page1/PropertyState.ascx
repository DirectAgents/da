<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PropertyState.ascx.cs"
    Inherits="Controls_Page1_PropertyState" %>
<div style="margin-bottom: 0px; margin-left: 67px; width: 300px;">
    <%-- Label --%>
    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String3 %>" />
    <%-- Choice List --%>
    <div style="padding-left: 9pt; display: inline;">
        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="XmlDataSource1"
            DataTextField="text" DataValueField="value">
        </asp:DropDownList>
    </div>
    <%-- Validation --%>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="DropDownList1">
        <div id="validation">
            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:String, String5 %>" />
        </div>
    </asp:RequiredFieldValidator>
    <%-- Data Source --%>
</div>
<asp:XmlDataSource ID="XmlDataSource1" runat="server" DataFile="~/App_Data/Data.xml"
    TransformFile="~/App_Data/Transform.xslt" XPath="data/options[@question='PropertyState']/choice">
</asp:XmlDataSource>
