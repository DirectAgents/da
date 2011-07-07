<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SSNReadMore.ascx.cs" Inherits="Controls_Page3_SSNReadMore" %>
<div class="smallLabel_blurb">
    <asp:Panel ID="Panel1" runat="server">
        Submitting your SSN wil <u>NOT Affect</u> your credit score.
        <br />
        <asp:HyperLink runat="server" ID="ReadMoreAboutSsnLinkButton" Text="Read More" Target="_blank"
            NavigateUrl=" http://offers.lendingtree.com/templates/LF-Refinance/SSN.htm" />
    </asp:Panel>
</div>
