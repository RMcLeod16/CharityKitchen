<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderMealsHelp.aspx.cs" Inherits="CharityKitchen.Help.OrderMealsHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Order Meals</h1>
    <h2>To Add a new Meal to the Order:</h2>
    <p>
        Click the "New" button.<br />
	    Then select the desired meal from the drop down box, and type in the desired quantity into the "Ordered Qty" textbox.<br />
	    Click the "Save" button.
    </p>
    <h2>To Edit an existing Meal in the Order:</h2>
    <p>
        Click the "Select" link on the row of the Order Meal you want to edit.<br />
		Then select the desired meal from the drop down box, and type in the desired quantity into the "Ordered Qty" textbox.<br />
		Click the "Save" button.
    </p>
    <h2>To Delete a Meal from the Order:</h2>
    <p>
        Click the "Select" link on the row of the Order you want to delete.<br />
		Click the "Delete" button.
    </p>
</asp:Content>
