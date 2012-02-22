<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="da_Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DataList ID="DataList1" runat="server" DataSourceID="SqlDataSource1" BackColor="Black" ForeColor="#00CC66" BorderColor="Red">
            <ItemTemplate>
                <asp:Label ID="TABLE_NAMELabel" runat="server" Text='<%# Eval("TABLE_NAME") %>' BackColor="#FFFFCC" />
                <br />
                <pre>
<asp:Label ID="VIEW_DEFINITIONLabel" runat="server" Text='<%# Eval("VIEW_DEFINITION") %>' BorderStyle="Dashed" BorderColor="Red"
    BackColor="#33CCFF" ForeColor="White" Font-Size="Medium" />
                </pre>
                <br />
            </ItemTemplate>
        </asp:DataList>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
            SelectCommand="
                SELECT 
                    [TABLE_NAME],
                    [VIEW_DEFINITION]
                FROM 
                    [LendingTreeWeb].[INFORMATION_SCHEMA].[VIEWS]"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
