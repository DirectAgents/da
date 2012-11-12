<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Page3.aspx.cs" Inherits="Page3" %>

<%@ Register Src="Controls/Page3/FirstName.ascx" TagName="FirstName" TagPrefix="uc1" %>
<%@ Register Src="Controls/Page3/LastName.ascx" TagName="LastName" TagPrefix="uc2" %>
<%@ Register Src="Controls/Page3/StreetAddress.ascx" TagName="StreetAddress" TagPrefix="uc3" %>
<%@ Register Src="Controls/Page3/ZipCode.ascx" TagName="ZipCode" TagPrefix="uc4" %>
<%@ Register Src="Controls/Page3/Email.ascx" TagName="Email" TagPrefix="uc5" %>
<%@ Register Src="Controls/Page3/HomePhone.ascx" TagName="HomePhone" TagPrefix="uc6" %>
<%@ Register Src="Controls/Page3/WorkPhone.ascx" TagName="WorkPhone" TagPrefix="uc7" %>
<%@ Register Src="Controls/Page3/DateOfBirth.ascx" TagName="DateOfBirth" TagPrefix="uc8" %>
<%@ Register Src="Controls/Page3/ShowMyResults.ascx" TagName="ShowMyResults" TagPrefix="uc9" %>
<%@ Register Src="Controls/Page3/SocialSecurityNumber.ascx" TagName="SocialSecurityNumber"
    TagPrefix="uc10" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <title>QuickMatch - a service of LendingTree</title>
    <!-- CSS -->
    <link rel="stylesheet" href="Styles/style.css" type="text/css" />
    <!--[if IE 7]>
<link rel="stylesheet" href="../Styles/ie7.css" type="text/css" />
<![endif]-->
    <!-- JS -->
    <script src="Scripts/screen.js" type="text/javascript"></script>
</head>
<body>
    <div id="_pageSmartTip" class="smartTipWindow" onclick="this.style.display='none';">
    </div>
    <form id="form1" runat="server">
    <div class="website-wrapper">
        <div id="topheader" class="png">
            <span id="tagline" class="inline">Get mortgage offers</span>
        </div>
        <div id="step3" class="board-content-data">
            <div class="left-rail inline">
                <div class="step">
                    Step 3 of 3</div>
                <div id="page2RailHeader" class="stroke">
                    <h2 class="green">
                        See if you can
                        <br />
                        lower your rate today
                    </h2>
                </div>
                <div id="page3RailChecklist" class="stroke">
                    <ul>
                        <li class="noSSN">No SSN Required</li>
                        <li>No Obligation</li>
                        <li>Completely Free</li>
                    </ul>
                </div>
                <div id="leftRailSSN">
                    <h4 class="green">
                        Benefits of providing your<br />
                        social security number</h4>
                    <ul>
                        <li>may help us provide more loan options</li>
                        <li>may help us match you with more lenders</li>
                        <li>may provide more accurate quotes</li>
                        <li>may help you get a lower rate</li>
                    </ul>
                </div>
                <div class="leftRailImage png">
                </div>
            </div>
            <div class="form-fields">
                <div id="page3FormHeader" class="png">
                    <h2 class="green">
                        Step 3 of 3</h2>
                </div>
                <div class="form-content png">
                    <div class="entry-row">
                        <div id="form_step3_inner">
                            <uc1:FirstName ID="FirstName1" runat="server" />
                            <test:Automation ID="Automation1" runat="server" Action="SetProperty" Target="FirstName1"
                                Property="Value" Value="Test" Key="TestAutomation" />
                            <uc2:LastName ID="LastName1" runat="server" />
                            <test:Automation ID="Automation2" runat="server" Action="SetProperty" Target="LastName1"
                                Property="Value" Value="Test" Key="TestAutomation" />
                            <uc3:StreetAddress ID="StreetAddress1" runat="server" />
                            <test:Automation ID="Automation3" runat="server" Action="SetProperty" Target="StreetAddress1"
                                Property="Value" Value="Test"
                                Key="TestAutomation" />
                            <uc4:ZipCode ID="ZipCode1" runat="server" />
                            <uc5:Email ID="Email1" runat="server" />
                            <test:Automation ID="Automation4" runat="server" Action="SetProperty" Target="Email1"
                                Property="Value" Value="test@test.com" Key="TestAutomation" />
                            <uc6:HomePhone ID="HomePhone1" runat="server" />
                            <test:Automation ID="Automation5" runat="server" Action="SetProperty" Target="HomePhone1"
                                Property="Value" Value="310-555-1111" Key="TestAutomation" />
                            <uc7:WorkPhone ID="WorkPhone1" runat="server" />
                            <test:Automation ID="Automation6" runat="server" Action="SetProperty" Target="WorkPhone1"
                                Property="Value" Value="310-555-2222" Key="TestAutomation" />
                            <uc8:DateOfBirth ID="DateOfBirth1" runat="server" />
                            <test:Automation ID="Automation7" runat="server" Action="SetProperty" Target="DateOfBirth1"
                                Property="Value" Value="11/05/1955" Key="TestAutomation" />
                            <uc10:SocialSecurityNumber ID="SocialSecurityNumber1" runat="server" Msg="NOT REQUIRED, but providing this final detail could help you save!" />
                            <test:Automation ID="Automation8" runat="server" Action="SetProperty" Target="SocialSecurityNumber1"
                                Property="Value" Value="999-99-9999" Key="TestAutomation" />
                            <asp:Button ID="ShowMyResults1" runat="server" Text="" class="btnPage3" OnClick="ShowMyResults1_ButtonClick" />
                        </div>
                        <div id="submit-0" class="submitboard">
                            <div class="read-detail">
                                <h5 class="small-caption">
                                    By clicking on the "button" above, you consent, acknowledge and agree to the following:</h5>
                                <div class="scroll-content-block">
                                    <div class="accept-detail-panel">
                                        <ul>
                                            <li>Our <a href="javascript:STMRCWindow('http://www1.lendingtree.com/stmrc/termsofuse.asp?bp=v3&nonav=true')">
                                                <span>Terms of Use Agreement</span></a> and <a href="javascript:STMRCWindow('http://www1.lendingtree.com/stmrc/privacy.asp?nonav=true&bp=ltquickmatch', 550, 450)">
                                                    <span>Privacy Policy</span></a></li>
                                            <li>To receive important notices, and other <a href="#">communications</a> at any telephone number or email address (including a mobile device) you entered so 
                                            that LendingTree, up to five (5) lenders with whom you have been matched, or one of its third party associates can reach you regarding this request. 
                                            This authorization removes any previous registration(s) on a federal or state Do-Not-Call (DNC) registry or any internal opt-out/unsubscribe requests you may have previously 
                                            requested with LendingTree or matched lenders.</li>
                                            
                                            
                                            <li>That LendingTree, a lender with whom you have been matched, or one of its third party associates may use an automatic dialing system in connection 
                                            with calls made to any telephone number you entered, even 
                                            if it is a cellular phone number or other service for which the called person(s) could be charged for such call.</li>
                                            <%if (Model.RequiresDisclosure)
                                              {  %>
                                            <li>That I have received and reviewed the <a href="javascript:STMRCWindow('https://secure.lendingtree.com/stmrc/brokerstatedisclosure.asp?state=<%=State%>')">
                                                Mortgage Broker Disclosures</a> for my state <span id="myStateDisclosure">
                                                    <%=State%></span> </li>
                                            <%} %>
                                        </ul>
                                    </div>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/privacy.asp?nonav=true&bp=ltquickmatch', 550, 450)">
                                <img class="privacy-security-logo" src="./images/privacy-security-logo.gif" alt="privacy-security-logo" />
                            </a>
                        </div>
                    </div>
                </div>
                <div class="form-fields-end png">
                    &nbsp;</div>
            </div>
            <!--form fields-->
            <div class="clear">
            </div>
        </div>
        <div id="footer-wrapper">
            <div id="footer" class="board">
                <div class="footer-logo">
                    <a target="_blank" href="http://www.bbb.org/charlotte/business-reviews/online-loans-referral-services/lendingtree-in-charlotte-nc-109412">
                        <img src="./images/bbb-logo.gif" alt="bbb-logo" /></a> <a target="_blank" href="http://quickmatch.lendingtree.com/LT_housing.asp">
                            <img src="./images/house-logo.gif" alt="house-logo" /></a>
                    <!--CERTIFICATE TRUST LOGO-->
                    <span id="siteseal">
                        <script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=2nOOUr9eTPDXWnZhyCuzuV3T0u15tx2nRXhbr6K9zjPWT8FdsYJea78Nk8"></script>
                        <br />
                        <a style="font-family: arial; font-size: 9px" href="http://www.godaddy.com/email/email-hosting.aspx"
                            target="_blank">email hosting</a></span>
                    <!--END CERTIFICATE TRUST LOGO-->
                    <p>
                        Online Security:<a target="_blank" href="http://www.lendingtree.com/about-us/online-security/">
                            <span>Protect Against Fraud</span></a></p>
                </div>
                <!--End .footer-logo-->
                <div class="footer-text">
                  <a href="javascript:STMRCWindow('https://www.lendingtree.com/legal/privacy-policy', 550, 450)">
                        Privacy Policy</a> | <a href="javascript:STMRCWindow('https://www.lendingtree.com/legal/terms-of-use')">
                            Terms of Use</a> | <a href="javascript:STMRCWindow('https://www.lendingtree.com/legal/licenses-and-disclosures',550, 450)">
                                Licenses &amp; Disclosures</a> | <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/promotional_disclosuresv1.asp',550, 450)">
                                Full Advertising Disclosures</a><br />
                    <br />
                    <p style="text-align: left;">
                        LendingTree technology and processes are patented under U.S. Patent Nos. 6,385,594
                        and 6,611,816 and licensed under U.S. Patent Nos. 5,995,947 and 5,758,328. © 1998
                        - 2011 LendingTree, LLC. All Rights Reserved. This site is directed at, and made
                        available to, persons in the continental U.S., Alaska and Hawaii only.</p>
                </div>
                <!--End .footer-text-->
            </div>
            <!--End #footer-->
        </div>
    </div>
    <uc:ViewSession ID="ViewSession1" runat="server" />
    </form>
</body>
</html>
