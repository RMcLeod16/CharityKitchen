<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdersHelp.aspx.cs" Inherits="CharityKitchen.Help.OrdersHelp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Help - Orders</h1>
    <h2>To Add a new Order:</h2>
    <p>
        Click the "New" button.<br />
		Then, type in the Order Name, Email, Address, Suburb, and Postcode for the Order into the respective textboxes<br />
		Click the "Save" button.
    </p>
    <h2>To Edit an existing Order:</h2>
    <p>
        Click the "Select" link on the row of the Order you want to edit.<br />
		Then, type in the Order Name, Email, Address, Suburb, and Postcode for the Order into the respective textboxes<br />
		Click the "Save" button.
    </p>
    <h2>To View/Edit the Contents of an Order:</h2>
    <p>
        Click the "Select" link on the row of the Order you want to edit.<br />
        Then click the "View/Edit Order Contents" button, and you will be taken to the Order Meals View/Edit page.<br />
    </p>
</asp:Content>
