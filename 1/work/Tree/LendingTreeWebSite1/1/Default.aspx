<%@ Page Title="" Language="C#" MasterPageFile="~/1/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_1_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <uc:LoanType ID="LoanType1" runat="server" />
    </div>
    <div>
        <uc:PropertyState ID="PropertyState1" runat="server" />
    </div>
    <div>
        <uc:CreditRating ID="CreditRating1" runat="server" />
    </div>
    <div>
        <uc:GetQuoteNow ID="GetQuoteNow1" runat="server" OnButtonClick="GetQuoteNow1_ButtonClick" />
    </div>
</asp:Content>
