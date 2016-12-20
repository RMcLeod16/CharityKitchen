<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderEdit.aspx.cs" Inherits="CharityKitchen.OrderEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Orders Edit</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Meals in Order: <asp:Label ID="lblOrderIDName" runat="server" /></h2>
    <asp:GridView ID="gvOrderMeals" runat="server" OnSelectedIndexChanged="gvOrderMeals_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Order Meal Edit:</h2>
    <table>
        <tr>
            <td>ID:</td>
            <td><asp:Label ID="lblOrderMealID" Text="0" runat="server" /></td>
        </tr>
        <tr>
            <td>Meal:</td>
            <td><asp:DropDownList ID="ddlMeals" runat="server" /></td>
        </tr>
        <tr>
            <td>Ordered Qty:</td>
            <td><asp:TextBox ID="txtOrdQty" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" /></td>
            <td><asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" /></td>
        </tr>
    </table>
</asp:Content>
