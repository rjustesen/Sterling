<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
    <asp:Panel ID="pnlAdminMenu" runat="server" >
    <div id="menu">
    	<ul>
		    <li class="first active"><asp:HyperLink runat="server" Text="Home" NavigateUrl="~/Default.aspx"></asp:HyperLink></li>
	    </ul>
      <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

