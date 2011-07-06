<%@ Page Title="" Language="C#" MasterPageFile="~/da/MasterPage.master" AutoEventWireup="true"
    ValidateRequest="false" CodeFile="Fix.aspx.cs" Inherits="da_Fix" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<%--    <div>
        <asp:Panel ID="dialog" runat="server">
            <div id="dialogContents">
                FOO
                <br />
                <asp:Button ID="btnOK" Text="OK" runat="server" />
            </div>
        </asp:Panel>
        <asp:LinkButton ID="btnShow" Text="Modal" runat="server" />
        <act:ModalPopupExtender ID="ModalPopupExtender1" TargetControlID="btnShow" PopupControlID="dialog"
            OkControlID="btnOK" DropShadow="true" BackgroundCssClass="modalBackground" runat="server" />
    </div>--%>
    <asp:DetailsView ID="Details1" runat="server" DefaultMode="Edit" DataSourceID="SqlDataSource1"
        AutoGenerateRows="False">
        <Fields>
            <asp:TemplateField ShowHeader="false">
                <ItemStyle Width="1000px" Height="700px" BorderStyle="None" />
                <ItemTemplate>
                    <asp:TextBox ClientIDMode="Static" ID="PostText" runat="server" Wrap="false" Width="1000px"
                        Height="600px" TextMode="MultiLine" Text='<%# Bind("SubmitData") %>'></asp:TextBox>
                    <asp:TextBox ClientIDMode="Static" ID="ResponseText" runat="server" Wrap="false"
                        Width="1000px" Height="200px" TextMode="MultiLine" Text='<%# Bind("ResponseData") %>'></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
        </Fields>
        <HeaderTemplate>
            <asp:LinkButton ID="Repost" runat="server">Repost</asp:LinkButton>
            <asp:Label runat="server" ID="AppIdLabel" Text='<%=appid%>' />
        </HeaderTemplate>
    </asp:DetailsView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:LendingTreeWebConnectionString %>"
        SelectCommand="SELECT AppId, SubmitData, ResponseData FROM ApplicationLog WHERE AppId = @AppId">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="appid" Name="AppId" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
