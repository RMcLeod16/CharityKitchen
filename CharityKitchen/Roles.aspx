<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Roles.aspx.cs" Inherits="CharityKitchen.Roles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Roles</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Roles List:</h2>
        <asp:GridView ID="gvRoles" runat="server" OnSelectedIndexChanged="gvRoles_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Role Edit:</h2>
    <table>
        <tr>
            <td>Role ID:</td>
            <td><asp:Label ID="lblRoleID" runat="server" Text="0" /></td>
        </tr>
        <tr>
            <td>Role Description:</td>
            <td><asp:TextBox ID="txtRoleDescription" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click"/></td>
            <td><asp:Button ID="btnNew" Text="New" runat="server" OnClick="btnNew_Click" /></td>
        </tr>
    </table>
</asp:Content>
