<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="BuildScores.aspx.cs" Inherits="Admin_BuildScores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <script  type="text/javascript"  >

        function closeme() {
            window.open('', '_self', '');
            window.close();

        }

</script>
    
    <asp:LinkButton runat="server" ID="lnkClose" Text="Close Window" OnClientClick="javascript:closeme();"></asp:LinkButton>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

