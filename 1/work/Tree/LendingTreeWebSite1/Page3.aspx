﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="~/Page3.aspx.cs" Inherits="Page3" %>

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
    <div class="website-wrapper">
        <div id="topheader" class="png">
            <span id="tagline" class="inline">Get instant mortgage offers </span>
        </div>
        <div id="step3" class="board-content-data">
            <div class="left-rail inline">
                <div class="step">
                    Step 3 of 3</div>
                <div id="page2RailHeader" class="stroke">
                    <h2 class="green">
                        Lock in Your
                        <br />
                        Lowest Rate Today
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
                        The Last Step...</h2>
                </div>
                <div class="form-content png">
                    <div class="entry-row">
                        <form id="form1" runat="server">
                        <div id="form_step3_inner">
                            <uc1:FirstName ID="FirstName1" runat="server" />
                            <uc2:LastName ID="LastName1" runat="server" />
                            <uc3:StreetAddress ID="StreetAddress1" runat="server" />
                            <uc4:ZipCode ID="ZipCode1" runat="server" />
                            <uc5:Email ID="Email1" runat="server" />
                            <uc6:HomePhone ID="HomePhone1" runat="server" />
                            <uc7:WorkPhone ID="WorkPhone1" runat="server" />
                            <uc8:DateOfBirth ID="DateOfBirth1" runat="server" />
                            <uc10:SocialSecurityNumber ID="SocialSecurityNumber1" runat="server" />
                            <asp:Button ID="ShowMyResults1" runat="server" Text="" class="btnPage3" OnClick="ShowMyResults1_ButtonClick" />
                        </div>
                        <div id="submit-0" class="submitboard">
                            <div class="read-detail">
                                <h5 class="small-caption">
                                    By clicking on the "show my results" button above, you agree and accept:</h5>
                                <div class="scroll-content-block">
                                    <div class="accept-detail-panel">
                                        <ul>
                                            <li>our <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/termsofuse.asp?bp=v3&nonav=true')">
                                                <span>Terms of Use Agreement</span></a> and <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/privacy.asp?nonav=true&bp=ltquickmatch', 550, 450)">
                                                    <span>Privacy Policy</span></a></li>
                                            <li>to receive important notices, disclosures and other communications ("Disclosures
                                                and Communications") in electronic form (either email or via the Internet) as provided
                                                for in the <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/LTLEConsent.asp')">
                                                    <span>Consent for Electronic Disclosures and Communications from LendingTree Loans</span></a>
                                                and / or <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/NetworkEConsent.asp')">
                                                    <span>Network Lenders</span></a>. In addition, if you proceed with a Loan Application
                                                with LendingTree Loans, LendingTree Loans can provide you with Disclosures and Communications
                                                in Electronic form. You understand that you will need an email address, Internet
                                                access and PDF software to review the Disclosures and Communications.</li>
                                            <li>that we may access your credit file even if your social security number is not provided.</li>
                                            <%if (Model.RequiresDisclosure)
                                              {  %>
                                            <li>that I have received and reviewed the <a href="javascript:STMRCWindow('https://secure.lendingtree.com/stmrc/brokerstatedisclosure.asp?state=<%=State%>')">
                                                Mortgage Broker Disclosures</a> for <span id="myStateDisclosure">
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
                    <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/privacy.asp?nonav=true&bp=ltquickmatch', 550, 450)">
                        Privacy Policy</a> | <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/termsofuse.asp?bp=v3&nonav=true')">
                            Terms of Use</a> | <a href="javascript:STMRCWindow('http://www.lendingtree.com/stmrc/disclosure.asp?nonav=true&bp=ltquickmatch',550, 450)">
                                Licenses &amp; Disclosures</a><br />
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
