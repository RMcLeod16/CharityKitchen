<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MealIngredientsHelp.aspx.cs" Inherits="CharityKitchen.Help.MealIngredientsHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Meal Ingredients</h1>
    <h2>To Add a new Ingredient to the Meal:</h2>
    <p>
        Click the "New" button.<br />
		Then, select the desired Ingredient from the drop down box and type the Quantity Required into the "Req. Qty" textbox.<br />
		Click the "Save" button.
    </p>
    <h2>To Edit an existing Ingredient in the Meal:</h2>
    <p>
        Click the "Select" link on the row of the Meal Ingredient you want to edit.<br />
		Then, select the desired Ingredient from the drop down box and type the Quantity Required into the "Req. Qty" textbox.<br />
		Click the "Save" button.
    </p>
    <h2>To Delete an Ingredient from the Meal:</h2>
    <p>
        Click the "Select" link on the row of the Meal Ingredient you want to delete.<br />
		Click the "delete" button.
    </p>
</asp:Content>
