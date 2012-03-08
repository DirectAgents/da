<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default4.aspx.cs" Inherits="da_Default4" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="ObjectDataSource1" AllowSorting="True">
            <Columns>
                <asp:BoundField DataField="AppID" HeaderText="AppID" SortExpression="AppID" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp" />
                <asp:BoundField DataField="CDNumber" HeaderText="CDNumber" SortExpression="CDNumber" />
                <asp:BoundField DataField="IP" HeaderText="IP" SortExpression="IP" />
                <asp:BoundField DataField="ESourceID" HeaderText="ESourceID" SortExpression="ESourceID" />
                <asp:BoundField DataField="State" HeaderText="State" SortExpression="State" />
                <asp:BoundField DataField="DateOfBirth" HeaderText="DateOfBirth" SortExpression="DateOfBirth" />
                <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                <asp:CheckBoxField DataField="IsVetran" HeaderText="IsVetran" SortExpression="IsVetran" />
                <asp:BoundField DataField="Credit" HeaderText="Credit" SortExpression="Credit" />
            </Columns>
        </asp:GridView>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetLeads" TypeName="Leads.LeadsContainer" SortParameterName="sortParameter">
        </asp:ObjectDataSource>
    
    </div>
    </form>
</body>
</html>
