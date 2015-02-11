<%@ Page Title="Beschreibung" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Description.aspx.cs" Inherits="CodingLessons.Description" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: _levelData["descriptionHeader"] %>.</h2>
    <p>
        <%= _levelData["description"] %>
    </p>
    <asp:PlaceHolder runat="server" ID="given" />
    <br />
    <asp:Button
        ID="submitBtn"
        PostBackUrl="~/Exercise.aspx"
        runat="server"
        Text="Aufgabe starten"
    />
</asp:Content>