<%@ Page Title="Startseite" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CodingLessons._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server" ID="adminView" Visible="false">
        <div class="megatron">
            <h1>
                Administration
            </h1>
            <ul>
                <li>
                    <p>
                        <a href="admin/CMS">
                            Content Management System
                        </a>
                    </p>
                </li>
                <li>
                    <p>
                        <a href="admin/MMS">
                            Mail Management System
                        </a>
                    </p>
                </li>
            </ul>
        </div>
    </asp:Panel>

    <div class="jumbotron">
        <h2>
            Was bietet Coding Lessons?
        </h2>
        <p>
            Coding Lessons ist eine kostenfreie Online-Platform zum Erlernen der Grundbausteine der Programmiersprache C#.
            Für jedes Thema gibt es eine Seite mit dem Wissen, dass man zur Anwendung braucht, sowie eine Aufgabe, wo es angewendet werden soll.
            <asp:LinkButton runat="server" PostBackUrl="~/About.aspx">Mehr dazu</asp:LinkButton>
        </p>
    </div>

</asp:Content>