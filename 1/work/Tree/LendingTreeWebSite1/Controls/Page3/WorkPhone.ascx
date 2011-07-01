<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WorkPhone.ascx.cs" Inherits="Controls_Page3_WorkPhone" %>
<%@ Register Src="~/Controls/Shared/PhoneNumber.ascx" TagName="PhoneNumber" TagPrefix="uc1" %>
<div class="enter-item inline">
    <div class="question">
        <%-- Label --%>
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String32 %>" />
    </div>
    <%-- Phone Number --%>
    <div class="answer">
        <uc1:PhoneNumber ID="PhoneNumber1" runat="server" TextBoxText="<%$ Resources:String, String52 %>"
            ValidatorErrorText="<%$ Resources:String, String43 %>" WaterMarkText="<%$ Resources:String, String52 %>" />
    </div>
</div>
