<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HomePhone.ascx.cs" Inherits="Controls_Page3_HomePhone" %>
<%@ Register Src="~/Controls/Shared/PhoneNumber.ascx" TagName="PhoneNumber" TagPrefix="uc1" %>
 <div class="enter-item inline">
 <div class="question">
<%-- Label --%>
<asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String31 %>" />
</div>
<%-- Phone Number --%>
<div class="answer">

<uc1:PhoneNumber ID="PhoneNumber1" runat="server" TextBoxText="<%$ Resources:String, String53 %>"
    ValidatorErrorText="<%$ Resources:String, String42 %>"  WaterMarkText="<%$ Resources:String, String53 %>" />
    
</div>
</div>