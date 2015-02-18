<%@ Page Title="Kontakt" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="CodingLessons.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %></h2>
    <h3>Haben Sie uns etwas zu sagen?</h3>
    
    <h4>Betreff</h4>
    <asp:TextBox runat="server" ID="topic" TextMode="SingleLine" Columns="40" /> <br />
    <h4>Email-Adresse:</h4>
    <asp:TextBox runat="server" ID="email" TextMode="Email" Columns="40" /> <br />
    <h4>Ihre Nachricht:</h4>
    <asp:TextBox runat="server" ID="message" TextMode="MultiLine" Rows="10" Columns="40" /> <br />
    <asp:Button runat="server" ID="submit" Text="Absenden" OnClick="Send" />
</asp:Content>
