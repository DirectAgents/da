<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DateSelector.ascx.cs" Inherits="DateSelector" %>
<asp:Panel runat="server" ID="DateSelectorPanel">
    <asp:TextBox runat="server" ID="DateSelectorTextBox" AutoPostBack="true" OnTextChanged="DateSelectorTextBox_TextChanged" />
    <act:CalendarExtender runat="server" ID="DateSelectorTextBoxCalendarExtender" TargetControlID="DateSelectorTextBox" />
</asp:Panel>
