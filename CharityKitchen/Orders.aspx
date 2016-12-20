<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="CharityKitchen.Orders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Orders</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Order List:</h2>
        <asp:GridView ID="gvOrders" runat="server" OnSelectedIndexChanged="gvOrders_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="true" />
            </Columns>
        </asp:GridView>
    <h2>Order Edit:</h2>
    <table>
        <tr>
            <td>Order ID:</td>
            <td><asp:Label ID="lblOrderID" Text="0" runat="server" /></td>
        </tr>
        <tr>
            <td>Order Name:</td>
            <td><asp:TextBox ID="txtOrderName" runat="server" /></td>
        </tr>
        <tr>
            <td>Email:</td>
            <td><asp:TextBox ID="txtEmail" runat="server" /></td>
        </tr>
        <tr>
            <td>Address:</td>
            <td><asp:TextBox ID="txtAddress" runat="server" /></td>
        </tr>
        <tr>
            <td>Suburb:</td>
            <td><asp:TextBox ID="txtSuburb" runat="server" /></td>
        </tr>
        <tr>
            <td>Postcode:</td>
            <td><asp:TextBox ID="txtPostcode" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" /></td>
            <td><asp:Button ID="btnOrderMealEdit" runat="server" Text="View/Edit Order Contents" OnClick="btnOrderMealEdit_Click"/></td>
        </tr>
    </table>
</asp:Content>
