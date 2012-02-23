<%@ Page Title="" Language="C#" MasterPageFile="~/da/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="da_Default" %>

<%@ Register Src="DateSelector.ascx" TagName="DateSelector" TagPrefix="uc1" %>
<%@ Register Src="SessionContents.ascx" TagName="SessionContents" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="10" cellspacing="0">
        <tr>
            <td>
                <table border="0" cellpadding="10" cellspacing="0">
                    <tr>
                        <td>
                            From:
                        </td>
                        <td>
                            <uc1:DateSelector ID="FromDateSelector" runat="server" SelectedDateSessionKey="fromtime" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            To:
                        </td>
                        <td>
                            <uc1:DateSelector ID="ToDateSelector" runat="server" SelectedDateSessionKey="totime" />
                        </td>
                    </tr>
                    <%--                    <tr>
                        <td colspan="2">
                            <asp:CheckBox runat="server" ID="GraphsCheckBox" AutoPostBack="true" OnCheckedChanged="GraphsCheckBox_CheckChanged" Checked="false"
                                Text="Show Charts" />
                        </td>
                    </tr>--%>
                </table>
            </td>
            <td>
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Text="Get CSV" />
            </td>
            <td>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSource2" ShowHeader="false">
                    <Columns>
                        <asp:CommandField ShowEditButton="True" />
                        <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" ReadOnly="True" SortExpression="id" Visible="False" />
                        <asp:BoundField DataField="name" HeaderText="name" SortExpression="name" />
                        <asp:BoundField DataField="data" HeaderText="data" SortExpression="data" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
                    DeleteCommand="DELETE FROM [Infos] WHERE [id] = @id" InsertCommand="INSERT INTO [Infos] ([name], [data]) VALUES (@name, @data)"
                    SelectCommand="SELECT * FROM [Infos]" UpdateCommand="UPDATE [Infos] SET [name] = @name, [data] = @data WHERE [id] = @id">
                    <DeleteParameters>
                        <asp:Parameter Name="id" Type="Int32" />
                    </DeleteParameters>
                    <InsertParameters>
                        <asp:Parameter Name="name" Type="String" />
                        <asp:Parameter Name="data" Type="String" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="name" Type="String" />
                        <asp:Parameter Name="data" Type="String" />
                        <asp:Parameter Name="id" Type="Int32" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            </td>
        </tr>
    </table>
    <asp:Panel ID="GraphsPanel" runat="server" Visible="false">
        <%--        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource3" Width="300px" Height="400px">
                        <Series>
                            <asp:Series Name="Series1" XValueMember="Credit" YValueMembers="CountOfAppId">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
                        SelectCommand="SELECT Credit, COUNT(AppID) AS CountOfAppId FROM dbo.GetLeads2(@FromTimestamp, @ToTimestamp) AS GetLeads2_1 GROUP BY Credit">
                        <SelectParameters>
                            <asp:SessionParameter Name="FromTimestamp" SessionField="fromtime" DbType="DateTime" />
                            <asp:SessionParameter Name="ToTimestamp" SessionField="totime" DbType="DateTime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
                <td>
                    <asp:Chart ID="Chart2" runat="server" DataSourceID="SqlDataSource4" Width="300px" Height="400px">
                        <Series>
                            <asp:Series Name="Series1" XValueMember="ESourceID" YValueMembers="CountOfAppId">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
                        SelectCommand="SELECT ESourceID, COUNT(AppID) AS CountOfAppId FROM dbo.GetLeads2(@FromTimestamp, @ToTimestamp) AS GetLeads2_1 GROUP BY ESourceID">
                        <SelectParameters>
                            <asp:SessionParameter Name="FromTimestamp" SessionField="fromtime" DbType="DateTime" />
                            <asp:SessionParameter Name="ToTimestamp" SessionField="totime" DbType="DateTime" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </td>
            </tr>
        </table>--%>
    </asp:Panel>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" AllowPaging="True"
        PageSize="100" BackColor="White" BorderColor="#999999" BorderStyle="Solid" BorderWidth="1px" CellPadding="3" Font-Size="Small"
        ForeColor="Black" GridLines="Vertical" AllowSorting="True">
        <AlternatingRowStyle BackColor="#CCCCCC" />
        <Columns>
            <asp:BoundField DataField="AppID" HeaderText="AppID" SortExpression="AppID" />
            <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
            <asp:BoundField DataField="CDNumber" HeaderText="CDNumber" SortExpression="CDNumber" />
            <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
            <asp:BoundField DataField="IP" HeaderText="IP" SortExpression="IP" />
            <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp" />
            <asp:BoundField DataField="ESourceID" HeaderText="ESourceID" SortExpression="ESourceID" />
            <asp:BoundField DataField="Credit" HeaderText="Credit" SortExpression="Credit" />
            <asp:TemplateField HeaderText="Error" SortExpression="Error">
                <ItemTemplate>
                    <asp:HyperLink Target="_blank" ID="FixHyperLink" runat="server" NavigateUrl='<%# Bind("AppID", "~/da/Fix.aspx?appid={0}") %>'>Fix</asp:HyperLink>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Error") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerSettings Mode="Numeric" Position="TopAndBottom" />
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
        SelectCommand="GetLeads3" 
        OnSelecting="SqlDataSource1_Selecting"
        SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="FromDateSelector" Name="FromTimestamp" PropertyName="SelectedDate" />
            <asp:ControlParameter ControlID="ToDateSelector" Name="ToTimestamp" PropertyName="SelectedDate" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:LinkButton ID="DropSessionButton" runat="server" OnClick="DropSessionButton_Click">Drop Session</asp:LinkButton>
</asp:Content>
