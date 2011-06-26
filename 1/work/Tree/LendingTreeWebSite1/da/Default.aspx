<%@ Page Title="" Language="C#" MasterPageFile="~/da/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="da_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <asp:RadioButtonList ID="SearchType" runat="server" AutoPostBack="True" RepeatDirection="Horizontal"
        OnSelectedIndexChanged="SearchType_SelectedIndexChanged">
        <asp:ListItem Selected="True"></asp:ListItem>
        <asp:ListItem>From/To</asp:ListItem>
    </asp:RadioButtonList>--%>
    <table>
        <tr>
            <td>
                <asp:Calendar ID="CalendarFrom" runat="server" BackColor="White" BorderColor="White"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px"
                    NextPrevFormat="FullMonth" Width="350px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True"
                        Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Calendar ID="CalendarTo" runat="server" BackColor="White" BorderColor="White"
                    BorderWidth="1px" Font-Names="Verdana" Font-Size="9pt" ForeColor="Black" Height="190px"
                    NextPrevFormat="FullMonth" Width="350px">
                    <DayHeaderStyle Font-Bold="True" Font-Size="8pt" />
                    <NextPrevStyle Font-Bold="True" Font-Size="8pt" ForeColor="#333333" VerticalAlign="Bottom" />
                    <OtherMonthDayStyle ForeColor="#999999" />
                    <SelectedDayStyle BackColor="#333399" ForeColor="White" />
                    <TitleStyle BackColor="White" BorderColor="Black" BorderWidth="4px" Font-Bold="True"
                        Font-Size="12pt" ForeColor="#333399" />
                    <TodayDayStyle BackColor="#CCCCCC" />
                </asp:Calendar>
            </td>
            <td>
                <asp:Button ID="LinkButton1" runat="server" onclick="LinkButton1_Click" Text="Get CSV" />
            </td>
        </tr>
    </table>
    <div>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="True" DataSourceID="SqlDataSource1"
            AllowPaging="true" PageSize="100" BackColor="White" BorderColor="#999999" BorderStyle="Solid"
            BorderWidth="1px" CellPadding="3" Font-Size="Small" ForeColor="Black" GridLines="Vertical"
            AllowSorting="true">
            <AlternatingRowStyle BackColor="#CCCCCC" />
            <PagerSettings Mode="Numeric" Position="TopAndBottom" />
        </asp:GridView>
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
        SelectCommand="SELECT * FROM dbo.GetLeads() WHERE Timestamp >= @FromTimestamp and Timestamp <= @ToTimestamp">
        <SelectParameters>
            <asp:ControlParameter Name="FromTimestamp" ControlID="CalendarFrom" PropertyName="SelectedDate" />
            <asp:ControlParameter Name="ToTimestamp" ControlID="CalendarTo" PropertyName="SelectedDate" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
