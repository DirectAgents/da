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
        <span class="smallLabel lock inline"><span>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="ssnBox3Digits" MaxLength="3" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox1" runat="server" ValidationExpression="^\d{3}$"
                Display="Dynamic" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox1" runat="server" Enabled="false" Display="Dynamic" />
        </span><span>
            <asp:TextBox ID="TextBox2" runat="server" CssClass="ssnBox2Digits" MaxLength="2" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox2" runat="server" ValidationExpression="^\d{2}$"
                Display="Dynamic" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox2" runat="server" Enabled="false" Display="Dynamic" />
        </span><span>
            <asp:TextBox ID="TextBox3" runat="server" CssClass="ssnBox4Digits" MaxLength="4" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox3" runat="server" ValidationExpression="^\d{4}$"
                Display="Dynamic" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" CssClass="ssnBoxRequiredFieldValidatorText"
                Text="*" ControlToValidate="TextBox3" runat="server" Enabled="false" Display="Dynamic" />
        </span>
            <div class="smallLabel_img">
                <img src="./images/small-lock.png" alt="" />
            </div>
            <uc2:SSNReadMore ID="SSNReadMore1" runat="server" Visible="false" />
        </span>
        <div class="ok">
            <uc1:Optional ID="Optional1" runat="server" />
        </div>
    </div>
</div>
