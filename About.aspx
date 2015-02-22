<%@ Page Title="Informationen" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="CodingLessons.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
        <h3>Was bietet Coding Lessons?</h3>
    <p>
        Coding Lessons ist eine kostenfreie Online-Platform zum Erlernen der Grundbausteine der Programmiersprache C#.
        Für jedes Thema gibt es eine Seite mit dem Wissen, dass man zur Anwendung braucht, sowie eine Aufgabe, wo es angewendet werden soll.
    </p>
    <asp:Image ImageUrl="~/Content/images/lvl1Description.png" runat="server" Width="400px" />
    <asp:Image ImageUrl="~/Content/images/lvl1Exercise.png" runat="server" Width="400px" />
    <p>
        Mit jeder Aufgabe vertieft man sein Wissen Schritt für Schritt in einem bestimmten Bereich und sammelt für das
        Bewältigen der auf einander aufbauenden Aufgaben Punkte.
    </p>
</asp:Content>
