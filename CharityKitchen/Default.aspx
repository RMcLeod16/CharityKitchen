<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CharityKitchen._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Welcome, <asp:Label ID="lblUsername" runat="server" />!</h1>
        <p class="lead">Click the links at the top to access various parts of the site.</p>
    </div>

</asp:Content>
