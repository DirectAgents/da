<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SocialSecurityNumber.ascx.cs"
    Inherits="Controls_Page3_SocialSecurityNumber" %>
<%@ Register Src="../Shared/Optional.ascx" TagName="Optional" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Page3/SSNReadMore.ascx" TagName="SSNReadMore" TagPrefix="uc2" %>
<div class="tipTeaser">
    <h4 class="green" runat="server"><%=Msg%></h4>
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
                
                
                <a href="#" class="tip"><img src="./images/small-lock.png" alt="" /><span>
   <div id="tip-list">
    <ul>

    <li>Our <div class="terms_of_use_agreement" onclick="agreement();">Terms of Use Agreement</div> and <div class="privacy_policy" onclick="privacy_policy();">Privacy Policy.</div></li>

    <li>

    To receive important notices, and other communications<br /> at any telephone number or email address (including a <br />mobile device)

    you entered so that LendingTree, up to <br />five (5) lenders with whom you have been matched, or <br />one of its third party associates can

    reach you regarding <br />this request. This authorization removes any previous <br />registration(s) on a federal or state Do-Not-Call (DNC) <br>

    registry or any internal opt-out/unsubscribe requests <br />you may have previously requested with LendingTree <br />or matched lenders.

    </li>

    <li>

    That LendingTree, a lender with whom you have been <br />matched, or one of its third party associates may use an <br />automatic dialing system

    in connection with calls made to <br />any telephone number you entered, even if it is a cellular <br />phone number or other service for which the <br>

    called person(s) could be charged for such call.

    </li>

    <li>

   That I have received and reviewed the <br /><div class="mortgage_broker_disclosure" onclick="mortgage_broker_disclosure();">Mortgage Broker Disclosures</div> for my state.

    </li>

    </ul>
 
 </div>   
    </span></a>
                
                
                
            </div>
            <uc2:SSNReadMore ID="SSNReadMore1" runat="server" Visible="false" />
        </span>
        <div class="ok">
            <uc1:Optional ID="Optional1" runat="server" />
        </div>
    </div>
</div>
