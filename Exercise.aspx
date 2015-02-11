<%@ Page Title="Exercise" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Exercise.aspx.cs" Inherits="CodingLessons.Exercise" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="solution" Visible="true" runat="server">

        <h2><%: _levelData["descriptionHeader"] %>.</h2>
        <p>
            <%= _levelData["task"] %>
        </p>
        
        <asp:Panel runat="server" ID="solutionPanel" BackColor="#d5d5d5">
            <asp:PlaceHolder
                ID="solutionPlaceHolder"
                OnLoad="solutionPlaceHolder_Load"
                runat="server" 
            />
        </asp:Panel>

        <br />
        <asp:Button
            Enabled="True"
            ID="Submit"
            OnClick="Submit_Click"
            runat="server"
            Text="Weiter"
            UseSubmitBehavior="False"
            Visible="True"
        />

    </asp:Panel>

</asp:Content>