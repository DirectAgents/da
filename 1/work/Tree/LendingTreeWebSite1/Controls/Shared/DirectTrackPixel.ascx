<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DirectTrackPixel.ascx.cs" Inherits="Controls_Shared_DirectTrackPixel" %>
<script type="text/javascript" src="https://da-tracking.com/lead_third/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>"></script>
<noscript>
    <img alt="" src="https://da-tracking.com/track_lead/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>" />
</noscript>
<asp:Panel ID="ExtraPixelPanel" runat="server">
    <script type="text/javascript" src="http://hop2.ctrhub2.com/lead_third/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>"></script>
    <noscript>
        <img alt="" src="http://hop2.ctrhub2.com/track_lead/<%=Pid%>/OPTIONAL_INFORMATION/<%=OptionalInformation%>" />
    </noscript>
</asp:Panel>
