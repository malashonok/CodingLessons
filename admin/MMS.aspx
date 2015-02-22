<%@ Page Language="C#" Title="Admin: Mail Management System" AutoEventWireup="true" CodeBehind="MMS.aspx.cs" Inherits="CodingLessons.admin.MMS" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="Messages" OnLoad="Messages_Load" />
</asp:Content>