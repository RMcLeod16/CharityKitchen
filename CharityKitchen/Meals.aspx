<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Meals.aspx.cs" Inherits="CharityKitchen.Meals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblInfo" runat="server" />
    <h1>Meals</h1>
    <asp:Button ID="btnHelp" runat="server" Text="How To Use" OnClick="btnHelp_Click" />
    <h2>Meals List:</h2>
    <asp:GridView ID="gvMeals" runat="server" OnSelectedIndexChanged="gvMeals_SelectedIndexChanged">
        <Columns>
            <asp:CommandField ShowSelectButton="true" />
        </Columns>
    </asp:GridView>
    <h2>Meal Name Edit:</h2>
    <table>
        <tr>
            <td>Meal ID:</td>
            <td><asp:Label ID="lblMealID" runat="server" Text="0" /></td>
        </tr>
        <tr>
            <td>Meal Name:</td>
            <td><asp:TextBox ID="txtMealName" runat="server" /></td>
        </tr>
        <tr>
            <td><asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" /></td>
            <td><asp:Button ID="btnNew" runat="server" Text="New" OnClick="btnNew_Click" /></td>
            <td><asp:Button ID="btnMealIngredientsEdit" runat="server" Text="Edit Meal Contents" OnClick="btnMealIngredientsEdit_Click" /></td>
        </tr>
    </table>
</asp:Content>
