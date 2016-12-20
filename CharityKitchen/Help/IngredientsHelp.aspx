<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IngredientsHelp.aspx.cs" Inherits="CharityKitchen.Help.IngredientsHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Ingredients</h1>
    <h2>To Add a new Ingredient:</h2>
    <p>
        Click the "New" button.<br />
		Then, type in the Ingredient's Name, the Available Quantity, and the Cost Per Unit (in $) for the Ingredient into the respective textboxes.<br />
		Click the "Save" button.
    </p>
    <h2>To Edit an existing Ingredient:</h2>
    <p>
        Click the "Select" link on the row of the Ingredient you want to edit.<br />
		Then, type in the Ingredient's Name, the Available Quantity, and the Cost Per Unit (in $) for the Ingredient into the respective textboxes.<br />
		Click the "Save" button.
    </p>
</asp:Content>
