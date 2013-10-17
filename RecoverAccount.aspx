<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="RecoverAccount.aspx.cs" Inherits="RecoverAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <table  cellpadding="10" cellspacing="10" style="border-collapse:collapse;text-align: center;margin-left: auto;margin-right: auto;text-align: left;" >
    <tr>
        <td align="center" colspan="2" style="background:#F2EBDE; font-weight:bold; ">Having Trouble Siging in?</td>
    </tr>
    
    <tr>
        <td>
        <asp:Panel runat="server" ID="Panel1" >
            <asp:Label runat="server" ID="labEmail" CssClass="smalltext" Text="To reset your password, enter the email address you used to register with the system."></asp:Label><br />
            <asp:Label runat="server" ID="labEmail1" Text="Email address"></asp:Label>
            <asp:TextBox runat="server" ID="txtEmail"></asp:TextBox>
            </asp:Panel></td>
    </tr>
    <tr><td><br /></td></tr>
    <tr>
        <td align="center" colspan="2">
            <asp:Button ID="btnReset" runat="server" Text="Continue" onclick="btnReset_Click" />
        </td>
    </tr>
  </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

