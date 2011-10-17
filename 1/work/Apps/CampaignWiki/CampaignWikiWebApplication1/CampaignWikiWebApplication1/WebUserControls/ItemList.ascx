<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemList.ascx.cs" Inherits="CampaignWikiWebApplication1.WebUserControls.ItemList" %>
<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>
<%@ Register Src="WebUserControl1.ascx" TagName="WebUserControl1" TagPrefix="uc1" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="OuterPanel" runat="server">
            <!-- +++++++++++++ -->
            <!-- TOP MENU BAR  -->
            <!-- +++++++++++++ -->
            <table id="alv" class="campaignListMenu" runat="server">
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <!-- +++++++++++++ -->
                                    <!-- FILTER        -->
                                    <!-- +++++++++++++ -->
                                    <asp:Label ID="CampaignsFilterDropDownListLabel" runat="server" Text="Filter" />
                                    <asp:DropDownList ID="CampaignsFilterDropDownList" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="CampaignsFilterDropDownList_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <!-- +++++++++++++ -->
                                    <!-- PAGE SIZE     -->
                                    <!-- +++++++++++++ -->
                                    <asp:Label ID="CampaignsPageSizeDropDownListLabel" runat="server" Text="Page size" />
                                    <asp:DropDownList ID="CampaignsPageSizeDropDownList" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="CampaignsPageSizeDropDownList_SelectedIndexChanged">
                                        <asp:ListItem Text="10" />
                                        <asp:ListItem Selected="True" Text="20" />
                                        <asp:ListItem Text="40" />
                                        <asp:ListItem Text="80" />
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <!-- +++++++++++++ -->
                        <!-- VIEW MODE     -->
                        <!-- +++++++++++++ -->
                        <asp:Label ID="CampaignsDisplayLabel" runat="server" Text="Display Mode" />
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                            <asp:ListItem Text="Wiki" Selected="True" />
                            <asp:ListItem Text="Table" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisExt" runat="server" TargetControlID="alv"
                VerticalSide="Top" VerticalOffset="10" HorizontalSide="Left" HorizontalOffset="10"
                ScrollEffectDuration=".1" />
            <!-- +++++++++++++ -->
            <!-- SELECTED      -->
            <!-- +++++++++++++ -->
            <table border="0" cellpadding="0" cellspacing="0" runat="server" id="alv2" class="campaignListTable">
                <tr>
                    <td>
                        <asp:Panel runat="server" ID="SelectedCampaignsOuterPanel" CssClass="selectedCampaignsOuter">
                            <asp:Panel runat="server" ID="SelectedCampaignsCollapsiblePanelHandle">
                                <asp:Label ID="HideOrShowLabel" CssClass="hideOrShowLabel" runat="server" />
                            </asp:Panel>
                            <asp:Panel ID="SelectedCampaignsPanel" runat="server" Style="z-index: 1;">
                                <asp:LinkButton ID="SendEmailButton" runat="server" Text="Send" />
                                <asp:LinkButton ID="ClearSelectedCampaignsButton" runat="server" Text="Clear" OnClick="ClearSelectedCampaignsButton_Click" />
                                <asp:GridView ID="SelectedCampaignsGridView" runat="server" DataSourceID="SelectedCampaignsDataSource"
                                    AutoGenerateColumns="False" AllowPaging="True" DataKeyNames="PID" GridLines="Horizontal"
                                    OnSelectedIndexChanged="SelectedCampaignsGridView_SelectedIndexChanged" ShowHeader="false">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="SelectButton" runat="server" CausesValidation="False" CommandName="Select"
                                                    Text="Unselect" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        No Selection
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <asp:ObjectDataSource ID="SelectedCampaignsDataSource" runat="server" SelectMethod="GetSelectedCampaigns"
                                    SelectCountMethod="CountSelectedCampaigns" TypeName="CampaignWikiWebApplication1.Data.Campaigns"
                                    EnablePaging="true" MaximumRowsParameterName="max" StartRowIndexParameterName="start" />
                            </asp:Panel>
                            <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtender" runat="Server"
                                TargetControlID="SelectedCampaignsPanel" ExpandControlID="SelectedCampaignsCollapsiblePanelHandle"
                                CollapseControlID="SelectedCampaignsCollapsiblePanelHandle" Collapsed="false"
                                TextLabelID="HideOrShowLabel" ImageControlID="Image1" ExpandedText="(Hide Selected Campaigns)"
                                CollapsedText="(Show Selected Campaigns)" SuppressPostBack="true" />
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:AlwaysVisibleControlExtender ID="AlwaysVisibleControlExtender2" runat="server"
                TargetControlID="alv2" VerticalSide="Top" VerticalOffset="10" HorizontalSide="Right"
                HorizontalOffset="10" ScrollEffectDuration=".1" />
            <table border="1px" width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="Panel2" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <%--                
                <tr>
                    <td>
                        <asp:TextBox runat="server" ID="CampaignTemplateTextBox" TextMode="MultiLine" Width="90%" Rows="25"
                            Text="Hello <b>world!</b>" /><br />
                        <ajaxToolkit:HtmlEditorExtender ID="htmlEditorExtender1" TargetControlID="CampaignTemplateTextBox"
                            runat="server">
                        </ajaxToolkit:HtmlEditorExtender>
                    </td>
                </tr>
                --%>
                <tr>
                    <td>
                        <!-- +++++++++++++ -->
                        <!-- CAMPAIGN LIST -->
                        <!-- +++++++++++++ -->
                        <asp:Panel runat="server" ID="CampaignsOuterPanel" ScrollBars="Auto">
                            <asp:GridView ID="CampaignsGridView" runat="server" DataSourceID="CampaignsDataSource"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="20" DataKeyNames="PID"
                                OnSelectedIndexChanged="CampaignsGridView_SelectedIndexChanged" OnRowDataBound="CampaignsGridView_RowDataBound"
                                ShowHeader="False" GridLines="None" Width="100%" OnPageIndexChanged="CampaignsGridView_PageIndexChanged">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <%--
                                            <asp:Panel CssClass="popupMenu" ID="PopupMenu" runat="server">
                                                <div class="popupSelectLink">
                                                    <div>
                                                        <asp:LinkButton ID="SelectButton1" runat="server" CommandName="Select" Text="Select" />
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                            --%>
                                            <asp:Panel ID="CampaignListPanel" runat="server">
                                                <table class="campaignItem" width="100%">
                                                    <tr style='background-color: <%# (bool)Eval("IsSelected") == false ? "#fff" : "#eeeeff" %>'>
                                                        <td width="50px">
                                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select"
                                                                Text="Select"></asp:LinkButton>
                                                        </td>
                                                        <td width="50px">
                                                            <asp:Label Font-Bold="true" ID="PIDColumn" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("PID"))) %>' />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="NameColumn" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("Name"))) %>'
                                                                Font-Size='<%#RadioButtonList1.SelectedValue=="Wiki"?System.Web.UI.WebControls.FontUnit.Large:System.Web.UI.WebControls.FontUnit.Small%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="Panel1" runat="server" Visible='<%#RadioButtonList1.SelectedValue=="Wiki"?true:false%>'>
                                                <div style='width: 100%'>
                                                    <uc1:WebUserControl1 ID="WebUserControl11" PID='<%# "Z" + HttpUtility.HtmlEncode(Convert.ToString(Eval("PID"))) %>'
                                                        runat="server" />
                                                    <hr />
                                                </div>
                                            </asp:Panel>
                                            <%--<ajaxToolkit:HoverMenuExtender ID="HoverMenu" runat="Server" HoverCssClass="popupHover"
                                                PopupControlID="PopupMenu" PopupPosition="Left" TargetControlID="CampaignListPanel"
                                                PopDelay="25" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:ObjectDataSource ID="CampaignsDataSource" runat="server" SelectMethod="GetCampaigns"
                                SelectCountMethod="CountCampaigns" TypeName="CampaignWikiWebApplication1.Data.Campaigns"
                                EnablePaging="true" MaximumRowsParameterName="max" StartRowIndexParameterName="start" />
                        </asp:Panel>
                    </td>
                    <%--                    <td>
                        <asp:GridView ID="GridView1" runat="server" DataSourceID="CampaignsDataSource" AutoGenerateColumns="False"
                            AllowPaging="True" PageSize="20" DataKeyNames="PID" ShowHeader="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label Font-Bold="true" ID="PIDColumn" runat="server" Text='<%# HttpUtility.HtmlEncode(Convert.ToString(Eval("PID"))) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>--%>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
