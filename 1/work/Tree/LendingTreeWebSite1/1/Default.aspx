<%@ Page Title="" Language="C#" MasterPageFile="~/1/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_1_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <uc:loantype id="LoanType1" runat="server" />
    </div>
    <div>
        <uc:propertystate id="PropertyState1" runat="server" />
    </div>
    <div>
        <uc:creditrating id="CreditRating1" runat="server" />
    </div>
    <div>
        <uc:getquotenow id="GetQuoteNow1" runat="server" onbuttonclick="GetQuoteNow1_ButtonClick" />
    </div>
</asp:Content>
