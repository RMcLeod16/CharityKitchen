<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MealsHelp.aspx.cs" Inherits="CharityKitchen.Help.MealsHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Meals</h1>
    <h2>To Add a new Meal:</h2>
    <p>
        Click the "New" button.<br />
		Then, type a Name for the Meal into the "Meal Name" textbox.<br />
		Click the "Save" button.
    </p>
    <h2>To Edit an Existing Meal:</h2>
    <p>
        Click the "Select" link on the row of the Meal you want to edit.<br />
		Then, type a Name for the Meal into the "Meal Name" textbox.<br />
		Click the "Save" button.
    </p>
    <h2>To View/Edit the Contents of a Meal:</h2>
    <p>
        Click the "Select" link on the row of the mealyou want to edit.<br />
        Then click the "View/Edit MealContents" button, and you will be taken to the Meal Ingredients View/Edit page.<br />
    </p>
</asp:Content>
