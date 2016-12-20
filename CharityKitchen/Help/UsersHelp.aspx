<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UsersHelp.aspx.cs" Inherits="CharityKitchen.Help.UsersHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Users</h1>
    <h2>To Edit a User:</h2>
    <p>
        Click the "Select" link on the row of the User you want to edit.<br />
        Then, in the "Username" textbox under "User Data Edit" type in the new Username for that User, then click the "Save" button.
    </p>
    <h2>To View/Edit a User's roles:</h2>
    <p>
        Click the "Select" link on the row of the User you want to edit.<br />
        Then click the "View/Edit User Roles" buttoon, and you will be taken to the User Roles View/Edit page.<br />
    </p>
    <h2>To Reset a User's Password:</h2>
    <p>
        Click the "Select" link on the row of the User you want to edit.<br />
        Then, get the User to type in a new password in both the "New Password" and "Confirm New Password" textboxes.<br />
        Click the "RESET PASSWORD" button.
    </p>
    <h2>To Add a new User:</h2>
    <p>
        Click the "New" button.<br />
        Type the Username for the new User in the "Username" textbox under "Add New User".<br />
        Get the User to type their new password in both the "Password" and "Confirm Password" textboxes.<br />
        Click the "Create" button.
    </p>
</asp:Content>
