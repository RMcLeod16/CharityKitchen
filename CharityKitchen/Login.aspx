
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CharityKitchen.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager runat="server" />
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <h1>Login</h1>
                <asp:Label ID="lblInfo" Text="Enter user credentials." runat="server" />
                <table>
                    <tr>
                        <td>Username:</td>
                        <td><asp:TextBox ID="txtUsername" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>Password:</td>
                        <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" /></td>
                    </tr>
                    <tr>
                        <td><asp:Button ID="btnLogin" Text="Login" runat="server" OnClick="btnLogin_Click" /></td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
