<%@ Page Title="" Language="C#" MasterPageFile="~/da/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="da_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td>
                <asp:Calendar ID="CalendarFrom" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana"
                    Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px" OnSelectionChanged="CalendarFrom_SelectionChanged">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Calendar ID="CalendarTo" runat="server" BackColor="White" BorderColor="White" BorderWidth="1px" Font-Names="Verdana"
                    Font-Size="9pt" ForeColor="Black" Height="190px" NextPrevFormat="FullMonth" Width="350px" OnSelectionChanged="CalendarFrom_SelectionChanged2">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True" Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Button ID="LinkButton1" runat="server" OnClick="LinkButton1_Click" Text="Get CSV" />
            </td>
            <td>
                <asp:ListView ID="ListView1" runat="server" DataKeyNames="id" DataSourceID="SqlDataSource2">
                    <EditItemTemplate>
                        <li style="background-color: #FFCC66; color: #000080;">
                            <asp:TextBox ID="nameTextBox" runat="server" Text='<%# Bind("name") %>' />
                            <br />
                            <asp:TextBox ID="dataTextBox" runat="server" Text='<%# Bind("data") %>' />
                            <br />
                            <asp:Button ID="UpdateButton" runat="server" CommandName="Update" Text="Update" />
                            <asp:Button ID="CancelButton" runat="server" CommandName="Cancel" Text="Cancel" />
                        </li>
                    </EditItemTemplate>
                    <EmptyDataTemplate>
                        No data was returned.
                    </EmptyDataTemplate>
                    <ItemSeparatorTemplate>
                        <br />
                    </ItemSeparatorTemplate>
                    <ItemTemplate>
                        <li style="background-color: #FFFBD6; color: #333333;">
                            <asp:Label ID="nameLabel" runat="server" Text='<%# Eval("name") %>' />
                            <br />
                            <asp:Label ID="dataLabel" runat="server" Text='<%# Eval("data") %>' />
                            <br />
                            <asp:Button ID="EditButton" runat="server" CommandName="Edit" Text="Edit" />
                        </li>
                    </ItemTemplate>
                    <LayoutTemplate>
                        <ul id="itemPlaceholderContainer" runat="server" style="font-family: Verdana, Arial, Helvetica, sans-serif;">
                            <li runat="server" id="itemPlaceholder" />
                        </ul>
                        <div style="text-align: center; background-color: #FFCC66; font-family: Verdana, Arial, Helvetica, sans-serif; color: #333333;">
                        </div>
                    </LayoutTemplate>
                </asp:ListView>
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
        <table border="0" cellpadding="0" cellspacing="0">
            <thead>
                <tr>
                    <th>
                        Credit
                    </th>
                    <th>
                        ESourceID
                    </th>
                </tr>
            </thead>
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
        </table>
    </asp:Panel>
    <asp:CheckBox runat="server" ID="GraphsCheckBox" AutoPostBack="true" OnCheckedChanged="GraphsCheckBox_CheckChanged" Checked="false"
        Text="Show Charts" />
    <div>
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
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
        SelectCommand="SELECT * FROM dbo.GetLeads2(@FromTimestamp, @ToTimestamp)" CacheDuration="300" EnableCaching="True">
        <SelectParameters>
            <asp:SessionParameter Name="FromTimestamp" SessionField="fromtime" DbType="DateTime" />
            <asp:SessionParameter Name="ToTimestamp" SessionField="totime" DbType="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
