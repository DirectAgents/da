<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SocialSecurityNumber.ascx.cs"
    Inherits="Controls_Page3_SocialSecurityNumber" %>
<%@ Register Src="../Shared/Optional.ascx" TagName="Optional" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Page3/SSNReadMore.ascx" TagName="SSNReadMore" TagPrefix="uc2" %>
<div class="tipTeaser">
    <h4 class="green" runat="server">
        NOT REQUIRED, but providing this final detail could help you save!</h4>
</div>
<div class="enter-item inline">
    <div class="question">
        <%-- Label --%>
        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:String, String48 %>" />
    </div>
    <%-- Text Box --%>
    <div class="answer">
        <span class="smallLabel lock inline">
            <asp:TextBox ID="TextBox1" runat="server" CssClass="ssnBox"></asp:TextBox>
            <div class="smallLabel_img">
                <img src="./images/small-lock.png" alt="" />
            </div>
            <uc2:SSNReadMore ID="SSNReadMore1" runat="server" Visible="false" />
        </span>
        <div class="ok">
            <uc1:Optional ID="Optional1" runat="server" />
        </div>
        <%-- Validation --%>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ErrorMessage="<%$ Resources:String, String56 %>"
            ControlToValidate="TextBox1" runat="server" ValidationExpression="^\d{3}-\d{2}-\d{4}$">
        </asp:RegularExpressionValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="SSN Required"
            ControlToValidate="TextBox1" runat="server" Enabled="false">
        </asp:RequiredFieldValidator>
    </div>
</div>
