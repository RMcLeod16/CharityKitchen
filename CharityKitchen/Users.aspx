<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="CharityKitchen.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:Label ID="lblInfo" runat="server" />
    <h1>Users</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Users List:</h2>
    <asp:GridView ID="gvUsers" runat="server" OnSelectedIndexChanged="gvUsers_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Existing User Edit:</h2>
    <h3>User Data Edit:</h3>
    <table>
        <tr>
            <td>User ID:</td>
            <td><asp:Label ID="lblUserID" runat="server" Text="0" /></td>
        </tr>
        <tr>
            <td>Username:</td>
            <td><asp:TextBox ID="txtUsername" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnUserRolesEdit" runat="server" Text="View/Edit User Roles" OnClick="btnUserRolesEdit_Click" /></td>
        </tr>
    </table>
    <h3>User Password Reset:</h3>
    <table>
        <tr>
            <td>New Password:</td>
            <td><asp:TextBox ID="txtPassReset" runat="server" Text="PASSDefault" TextMode="Password" /></td>
        </tr>
        <tr>
            <td>Confirm New Password:</td>
            <td><asp:TextBox ID="txtPassResetConfirm" runat="server" Text="DefaultPASS" TextMode="Password" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnPasswordReset" runat="server" Text="RESET PASSWORD" OnClick="btnPasswordReset_Click"/></td>
        </tr>
    </table>
    <h2>Add New User:</h2>
    <table>
        <tr>
            <td>Username:</td>
            <td><asp:TextBox ID="txtNewUsername" runat="server" /></td>
        </tr>
        <tr>
            <td>Password:</td>
            <td><asp:TextBox ID="txtNewUserPass" runat="server" Text="PASSDefault" TextMode="Password" /></td>
        </tr>
        <tr>
            <td>Confirm Password:</td>
            <td><asp:TextBox ID="txtNewUserPassConfirm" runat="server" Text="DefaultPASS" TextMode="Password" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnNewUser" runat="server" Text="New" OnClick="btnNewUser_Click" /></td>
            <td><asp:Button ID="btnNewUserCreate" runat="server" Text="Create" OnClick="btnNewUserCreate_Click" /></td>
        </tr>
    </table>
</asp:Content>
