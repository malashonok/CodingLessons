<%@ Page Title="Exercise" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Exercise.aspx.cs" Inherits="CodingLessons.Exercise" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="success" Visble="false" runat="server">
        <h1> Well Done !!!!</h1>
        <asp:LinkButton runat="server" PostBackUrl="~/Description.aspx" Text="next level" />
    </asp:Panel>

    <asp:Panel ID="failure" Visible="false" runat="server">
        <h1> That was wrong.....</h1>
    </asp:Panel>

    <asp:Panel ID="solution" Visible="true" runat="server">

        <h2><%: _levelData["descriptionHeader"] %>.</h2>
        <p>
            <%= _levelData["task"] %>
        </p>
        
        <asp:Panel runat="server" ID="solutionPanel" BackColor="WhiteSmoke">
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

<script type="text/javascript">
    /*function EnterToTab() {
        //if (event.keyCode==13)
        //    event.keyCode=9;
        alert("thing went ok");
    }*/
</script>

</asp:Content>
