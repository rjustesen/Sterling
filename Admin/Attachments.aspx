<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Attachments.aspx.cs" Inherits="Admin_Attachments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
 <asp:Panel ID="pnlAdminMenu" runat="server" >
    <div id="menu">
        <ul>
	        <li  class="first active"><asp:HyperLink ID="HyperLink1" runat="server" Text="Home" NavigateUrl="~/Default.aspx"></asp:HyperLink></li>
	        <li><asp:HyperLink ID="HyperLink2" runat="server" Text="Account" NavigateUrl="~/Profile.aspx"></asp:HyperLink></li>
        </ul>
        <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
  
  <script src="../assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
  <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
  <script src="../assets/javascript/spin.min.js" type="text/javascript"></script>
  
  
    <script type="text/javascript">
        //<!--
        $(function() {
        $("input[type=submit], button").button();
        $("#<%=btnSearch.ClientID%>").click(function() {
                $("#loading").fadeIn();
                var opts = {
                    lines: 12, // The number of lines to draw
                    length: 7, // The length of each line
                    width: 4, // The line thickness
                    radius: 10, // The radius of the inner circle
                    color: '#000', // #rgb or #rrggbb
                    speed: 1, // Rounds per second
                    trail: 60, // Afterglow percentage
                    shadow: false, // Whether to render a shadow
                    hwaccel: false // Whether to use hardware acceleration
                };
                var target = document.getElementById('loading');
                var spinner = new Spinner(opts).spin(target);
            });
        });
        //-->
    </script>
    
   <table width="40%">
        <tr>
            <td><asp:HyperLink runat="server" ID="lnkImage" ImageUrl="~/assets/images/ico-back.png" Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink>
            <asp:HyperLink runat="server" ID="lnkBack"  Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink></td>
        </tr>
    </table>
    <br />
    
 <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
    <table>
        <tr>
            <td>Attachment Category:</td>
            <td><asp:DropDownList runat="server" ID="ddlCategory">
                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Document" Value="Document"></asp:ListItem>
                <asp:ListItem Text="Image" Value="Image"></asp:ListItem>
                <asp:ListItem Text="Media" Value="Media"></asp:ListItem>
                <asp:ListItem Text="Url" Value="Url"></asp:ListItem>
                <asp:ListItem Text="Personal Photo" Value="PersonalPhoto"></asp:ListItem>
                <asp:ListItem Text="Transcript" Value="Transcript"></asp:ListItem>
                <asp:ListItem Text="Letter Of Recomendation" Value="LetterOfRecomendation"></asp:ListItem>
                <asp:ListItem Text="Category" Value="Category"></asp:ListItem>
                <asp:ListItem Text="Category Description" Value="CategoryDescription"></asp:ListItem>
                <asp:ListItem Text="Leadership" Value="Leadership"></asp:ListItem>
                <asp:ListItem Text="Leadership Description" Value="LeadershipDescription"></asp:ListItem>
                <asp:ListItem Text="Citizenship" Value="Citizenship"></asp:ListItem>
                <asp:ListItem Text="Citizenship Description" Value="CitizenshipDescription"></asp:ListItem>
                <asp:ListItem Text="GroupPhoto" Value="GroupPhoto"></asp:ListItem>
            </asp:DropDownList></td>
            <td>Attachment Type:</td>
            <td><asp:DropDownList runat="server" ID="ddlType">
                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                <asp:ListItem Text="User" Value="User"></asp:ListItem>
                <asp:ListItem Text="Portfolio" Value="Portfolio"></asp:ListItem>
                <asp:ListItem Text="Application" Value="Application"></asp:ListItem>
                <asp:ListItem Text="FinalPortfolio" Value="FinalPortfolio"></asp:ListItem>
            </asp:DropDownList></td>
        </tr>
        <tr>
            <td>Description:</td>
            <td><asp:TextBox runat="server" ID="txtDescription" ></asp:TextBox></td>
            <td>Object Id:</td>
            <td><asp:TextBox runat="server" ID="txtObjectRow" ></asp:TextBox></td>
            <td>Name:</td>
            <td><asp:TextBox runat="server" ID="txtName" ></asp:TextBox></td>
        </tr>
        <tr>
            <td style="padding-top: 10px; padding-bottom: 10px;">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Width="100px" />
                  <div id="loading">
                    <div id="loadingcontent">
                        <p id="loadingspinner">
                            Searching, Please Wait...
                        </p>
                    </div>
                </div>
            </td>
            <td><asp:Label runat="server" ID="lblRecCount" ></asp:Label></td>
        </tr>
    </table>
    <br />
    </asp:Panel>
    
 <asp:GridView ID="gvAttachments" runat="server" 
        AutoGenerateColumns="False" 
        CssClass="grid"
        DataKeyNames="Id"
        onrowcommand="gvAttachments_RowCommand" 
        OnRowDataBound="gvAttachments_RowDataBound" 
        AlternatingRowStyle-CssClass="alt">
    <Columns> 
      <asp:BoundField DataField="Id" HeaderText="Id"></asp:BoundField>
      <asp:BoundField DataField="Category" HeaderText="Category"></asp:BoundField>
      <asp:BoundField DataField="Type" HeaderText="Type"></asp:BoundField>
      <asp:BoundField DataField="Name" HeaderText="Name"></asp:BoundField>
      <asp:BoundField DataField="Description" HeaderText="Description"></asp:BoundField>
      <asp:BoundField DataField="ObjectRowId" HeaderText="ObjectRowId"></asp:BoundField>
      <asp:TemplateField>
        <ItemTemplate>
          <asp:HyperLink runat="server" ID="lblView" Target="_blank" Text="View" NavigateUrl='<%# "~/DocView.aspx?id=" + Eval("Id").ToString() %>'></asp:HyperLink>
        </ItemTemplate>
      </asp:TemplateField>
   </Columns>
  </asp:GridView>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

