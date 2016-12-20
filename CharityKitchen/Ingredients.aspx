<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ingredients.aspx.cs" Inherits="CharityKitchen.Ingredients" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Ingredients</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Ingredients List:</h2>
        <asp:GridView ID="gvIngredients" runat="server" OnSelectedIndexChanged="gvIngredients_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Ingredient Edit:</h2>
    <table>
        <tr>
            <td>Ingredient ID:</td>
            <td><asp:Label ID="lblIngredientID" runat="server" Text="0" /></td>
        </tr>
        <tr>
            <td>Ingredient Name:</td>
            <td><asp:TextBox ID="txtIngredientName" runat="server" /></td>
        </tr>
        <tr>
            <td>Available Qty:</td>
            <td><asp:TextBox ID="txtAvailableQty" runat="server" /></td>
        </tr>
        <tr>
            <td>Cost Per Unit: $</td>
            <td><asp:TextBox ID="txtCostPerUnit" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" Text="Save" runat="server" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" Text="New" runat="server" OnClick="btnNew_Click" /></td>
        </tr>
    </table>
</asp:Content>
