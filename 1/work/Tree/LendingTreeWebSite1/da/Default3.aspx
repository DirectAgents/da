<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="da_Default3" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Chart ID="Chart1" runat="server" DataSourceID="SqlDataSource1">
            <Series>
                <asp:Series Name="Series1" XValueMember="Credit" YValueMembers="CountOfAppId">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
            SelectCommand="SELECT Credit, COUNT(AppID) AS CountOfAppId FROM dbo.GetLeads2('2012-02-01', '2012-02-22') AS GetLeads2_1 GROUP BY Credit">
        </asp:SqlDataSource>
        <asp:Chart ID="Chart2" runat="server" DataSourceID="SqlDataSource2">
            <Series>
                <asp:Series Name="Series1" XValueMember="ESourceID" YValueMembers="CountOfAppId">
                </asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1">
                </asp:ChartArea>
            </ChartAreas>
        </asp:Chart>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
            SelectCommand="SELECT ESourceID, COUNT(AppID) AS CountOfAppId FROM dbo.GetLeads2('2012-02-01', '2012-02-22') AS GetLeads2_1 GROUP BY ESourceID">
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
