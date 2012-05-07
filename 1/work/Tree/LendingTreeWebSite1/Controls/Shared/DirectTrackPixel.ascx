<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DirectTrackPixel.ascx.cs"
    Inherits="Controls_Shared_DirectTrackPixel" %>
<script type="text/javascript" src="https://da-tracking.com/lead_third/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>"></script>
<noscript><img alt="" src="https://da-tracking.com/track_lead/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>" /></noscript>
<!-- Offer Conversion: Lending Tree SSN Required -->
<img src="https://directagents.go2cloud.org/SL4?adv_sub=<%=OptionalInformation%>" width="1" height="1" />
<!-- // End Offer Conversion -->
<asp:Panel ID="ExtraPixelPanel" runat="server">
    <script src="https://hop2.ctrhub2.com/lead_third/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>"></script>
    <noscript><img src="https://hop2.ctrhub2.com/track_lead/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>" /></noscript>
</asp:Panel>
<iframe src="https://show.cappersms.com/SL2o" scrolling="no" frameborder="0" width="1" height="1"></iframe>
<iframe src="https://show.cappersms.com/SL2m" scrolling="no" frameborder="0" width="1" height="1"></iframe>
<iframe src="https://smarttrk.com/p.ashx?o=1708&t=TRANSACTION_ID" height="1" width="1" frameborder="0"></iframe>
<iframe src="https://smarttrk.com/p.ashx?o=1734&t=TRANSACTION_ID" height="1" width="1" frameborder="0"></iframe>
