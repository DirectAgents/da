﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Processing.aspx.cs" Inherits="Processing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <title></title>
    <link rel="stylesheet" href="Styles/style.css" type="text/css" />
    <script type="text/javascript">
        function LoadThankYou() {
            window.location = "<%=FullUrlFor(EPages.ThankYou)%>";
        }
    </script>
</head>
<body onload="LoadThankYou()">
    <div class="website-wrapper">
        <div id="step3" class="board-content-data">
            <div id="page2RailHeader" class="stroke">
                <div style="text-align: center;">
                    <h2 class="green">
                        Please wait while LendingTree processes your request.</h2>
                    <img src="images/loading.gif" />
                </div>
                <form id="form1" runat="server">
                <div>
                </div>
                </form>
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
    </div>
</body>
</html>
