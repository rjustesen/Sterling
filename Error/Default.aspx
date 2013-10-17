<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Error_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
 <div id="App_Error" runat="server" class="messageError" visible="true" enableviewstate="false">
           <asp:Label ID="lblErrorMessage" runat="server" CssClass="Message" />
 </div>
 <div>
    <a href="../Default.aspx" >Click Here to Return to the System</a>
 </div>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

