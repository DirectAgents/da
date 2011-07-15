<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Page2.aspx.cs" Inherits="Page2" %>

<%@ Register Src="Controls/Page2/PropertyType.ascx" TagName="PropertyType" TagPrefix="uc" %>
<%@ Register Src="Controls/Page2/PropertyPurpose.ascx" TagName="PropertyPurpose"
    TagPrefix="uc1" %>
<%@ Register Src="Controls/Page2/PropertyZip.ascx" TagName="PropertyZip" TagPrefix="uc2" %>
<%@ Register Src="Controls/Page2/ApproximatePropertyValue.ascx" TagName="ApproximatePropertyValue"
    TagPrefix="uc3" %>
<%@ Register Src="Controls/Page2/Continue.ascx" TagName="Continue" TagPrefix="uc4" %>
<%@ Register Src="Controls/Page2/MortgageBalance.ascx" TagName="MortgageBalance"
    TagPrefix="uc5" %>
<%@ Register Src="Controls/Page2/AmountDesiredAtClosing.ascx" TagName="AmountDesiredAtClosing"
    TagPrefix="uc6" %>
<%@ Register Src="Controls/Page2/MonthlyMortgagePayment.ascx" TagName="MonthlyMortgagePayment"
    TagPrefix="uc7" %>
<%@ Register Src="Controls/Page2/HadBakruptcy.ascx" TagName="HadBakruptcy" TagPrefix="uc8" %>
<%@ Register Src="Controls/Page2/HadForeclosures.ascx" TagName="HadForeclosures"
    TagPrefix="uc9" %>
<%@ Register Src="Controls/Shared/LendingTreeLoansOptIn.ascx" TagName="LendingTreeLoansOptIn"
    TagPrefix="uc10" %>
<%@ Register Src="Controls/Shared/VetranStatus.ascx" TagName="VetranStatus" TagPrefix="uc11" %>
<%@ Register Src="Controls/Page2/PurchasePrice.ascx" TagName="PurchasePrice" TagPrefix="uc12" %>
<%@ Register Src="Controls/Page2/DownPayment.ascx" TagName="DownPayment" TagPrefix="uc13" %>
<%@ Register Src="Controls/Page2/PropertyCity.ascx" TagName="PropertyCity" TagPrefix="uc14" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="Scripts/JScript.js" type="text/javascript"></script>
    <title>QuickMatch - a service of LendingTree</title>
    <!-- CSS -->
    <link rel="stylesheet" href="Styles/style.css" type="text/css" />
    <!--[if IE 7]>
<link rel="stylesheet" href="../Styles/ie7.css" type="text/css" />
<![endif]-->
    <!-- JS -->
    <script src="Scripts/screen.js" type="text/javascript"></script>
    <script src="Controls/Page2/Scripts/script.js" type="text/javascript"></script>

<%--    <script src="Scripts/blackbird/blackbird.js" type="text/javascript"></script>
    <link href="Scripts/blackbird/blackbird.css" rel="stylesheet" type="text/css" />--%>

</head>
<body>
    <form id="form1" runat="server">
<%--    <div id="ltvmon" style="position: fixed; left: 10px; top: 10px; width: 200px; height: 1.5em; background: red; color: White">
        LTV: <span id="ltvmon_val">-</span>
    </div>--%>
    <script type="text/javascript">
            $(document).ready(function() {
                <%=GenerateJSON()%>
            });
    </script>
    <div id="_pageSmartTip" class="smartTipWindow" onclick="this.style.display='none';">
    </div>
    <div class="website-wrapper">
        <div id="topheader" class="png">
            <span id="tagline" class="inline">Get instant mortgage offers </span>
            <div id="promoHeader" class="header inline hide">
            </div>
            <div id="promoSubHeader" class="subheader">
            </div>
            <div id="telephone">
            </div>
        </div>
        <div id="step2" class="board-content-data">
            <div class="left-rail inline">
                <div class="step">
                    Step 2 of 3</div>
                <div id="page2RailHeader" class="stroke">
                    <h2 class="green">
                        Lock in Your
                        <br />
                        Lowest Rate Today
                    </h2>
                </div>
                <div id="page2RailChecklist" class="stroke">
                    <ul>
                        <li class="noSSN">No SSN Required</li>
                        <li>No Obligation</li>
                        <li>Completely Free</li>
                    </ul>
                </div>
                <div class="leftRailImage png">
                </div>
            </div>
            <div class="form-fields">
                <div id="page2FormHeader" class="png">
                    <h2 class="green">
                        Just Two Quick Steps for Low Rates in
                        <%=PropertyStateName%></h2>
                </div>
                <div class="form-content png">
                    <div id="form_step2_inner">
                        <uc12:PurchasePrice ID="PurchasePrice1" runat="server" />
                        <uc13:DownPayment ID="DownPayment1" runat="server" />
                        <uc:PropertyType ID="PropertyType1" runat="server" />
                        <uc1:PropertyPurpose ID="PropertyPurpose1" runat="server" />
                        <uc14:PropertyCity ID="PropertyCity1" runat="server" />
                        <uc2:PropertyZip ID="PropertyZip1" runat="server" />
                        <uc3:ApproximatePropertyValue ID="ApproximatePropertyValue1" runat="server" />
                        <uc5:MortgageBalance ID="MortgageBalance1" runat="server" />
                        <uc6:AmountDesiredAtClosing ID="AmountDesiredAtClosing1" runat="server" />
                        <uc7:MonthlyMortgagePayment ID="MonthlyMortgagePayment1" runat="server" />
                        <uc8:HadBakruptcy ID="HadBakruptcy1" runat="server" />
                        <uc9:HadForeclosures ID="HadForeclosures1" runat="server" />
                        <uc11:VetranStatus ID="VetranStatus1" runat="server" />
                    </div>
                    <div>
                        <uc10:LendingTreeLoansOptIn ID="LendingTreeLoansOptIn1" runat="server" />
                    </div>
                    <div style="width: 400px; padding-right: 150px;">
                        <div class="security">
                        </div>
                        <asp:Button ID="Button1" runat="server" Text="" class="btnPage2" OnClick="Continue1_ButtonClick" />
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
