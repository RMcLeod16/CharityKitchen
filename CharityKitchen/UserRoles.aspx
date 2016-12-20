<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserRoles.aspx.cs" Inherits="CharityKitchen.UserRoles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>User Roles Edit</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click"/>
    <h2>Roles for User: <asp:Label ID="lblUserIDName" runat="server" /></h2>
    <asp:GridView ID="gvUserRoles" runat="server" OnSelectedIndexChanged="gvUserRoles_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>User Role Edit:</h2>
    <table>
        <tr>
            <td>ID:</td>
            <td><asp:Label ID="lblUserRoleID" Text="0" runat="server" /></td>
        </tr>
        <tr>
            <td>Role:</td>
            <td><asp:DropDownList ID="ddlRoles" runat="server" /></td>
        </tr>
        <tr>
            <td>AccessLevel:</td>
            <td><asp:DropDownList ID="ddlAccesLevel" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" /></td>
        </tr>
    </table>
</asp:Content>
