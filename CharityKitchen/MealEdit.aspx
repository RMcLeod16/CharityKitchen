<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MealEdit.aspx.cs" Inherits="CharityKitchen.MealEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Meals Edit</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Ingredients in Meal: <asp:Label ID="lblMealName" runat="server" /></h2>
    <asp:GridView ID="gvMealIngredients" runat="server" OnSelectedIndexChanged="gvMealIngredients_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Meal Ingredient Edit:</h2>
    <table>
        <tr>
            <td>ID:</td>
            <td><asp:Label ID="lblID" Text="0" runat="server" /></td>
        </tr>
        <tr>
            <td>Ingredient:</td>
            <td><asp:DropDownList ID="ddlIngredients" runat="server" /></td>
        </tr>
        <tr>
            <td>Req. Qty:</td>
            <td><asp:TextBox ID="txtReqQty" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" /></td>
            <td><asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click" /></td>
        </tr>
    </table>
</asp:Content>
