<%@ Page validateRequest="false" Language="C#" Title="Admin: Content Managment System" AutoEventWireup="true" CodeBehind="CMS.aspx.cs" Inherits="CodingLessons.admin.CMS" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="Exercises" OnLoad="Exercises_Load" />
    An <asp:TextBox runat="server" TextMode="Number" Columns="2" ID="newPosition" Text="1" />. Stelle eine neue Aufgabe <asp:Button runat="server" Text="einfügen" OnClick="AddLevel" />.
</asp:Content>