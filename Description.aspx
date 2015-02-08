<%@ Page Title="Beschreibung" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Description.aspx.cs" Inherits="CodingLessons.Description" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: _levelData["descriptionHeader"] %>.</h2>
    <p>
        <%= _levelData["description"] %>
    </p>
    <asp:TextBox
        BorderStyle="None"
        ID="exampleBox"
        OnLoad="exampleBox_Load"
        runat="server"
        ReadOnly="True"
        TextMode="MultiLine"
    />
    <br />
    <asp:Button
        ID="submitBtn"
        PostBackUrl="~/Exercise.aspx"
        runat="server"
        Text="Aufgabe starten"
    />
</asp:Content>
