<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Photos.aspx.cs" Inherits="Admin_Photos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" media="screen" />
     <style type="text/css">
    img {
        border: none;
    }
    </style>
    
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
            $("#<%=btnSearch.ClientID%>")
            .button()
            .click(function() {
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

            $('div#dialog-message').dialog({ autoOpen: false })
            $("#<%=lnkFinalist.ClientID%>").click(function(e) {
                //   var area = $("#<%=ddlArea.ClientID%>").val();
                var status = $("#<%=ddlStatus.ClientID%>").val();
                //if (area == "-1" || status == "-1") {
                if (status == "-1") {
                    e.preventDefault();
                    $('div#dialog-message').dialog({ buttons: [{ text: "Ok", click: function() { $(this).dialog("close"); } }] });
                    $('div#dialog-message').dialog("option", "modal", true);
                    $('div#dialog-message').dialog('open');
                }
            });
        });
        //-->
    </script>
    
    <div id="dialog-message" title="Warning">
      <p><span class="ui-icon ui-icon-circle-check" style="float: left; margin: 0 7px 50px 0;"></span>
        To run a Finalist Report you must first select a valid status.</p>
    </div>
    
    <table width="40%">
        <tr>
            <td><asp:HyperLink runat="server" ID="lnkImage" ImageUrl="~/assets/images/ico-back.png" Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink>
            <asp:HyperLink runat="server" ID="lnkBack"  Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink></td>
            <td>Photo Type:</td>
            <td>
                <asp:DropDownList runat="server" ID="ddlType" 
                    onselectedindexchanged="ddlType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="Nominee" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Group" Value="2"></asp:ListItem>
                </asp:DropDownList></td>
        </tr>
    </table>
    <br />
   
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
    <table width="80%">
        <tr>
            <td align="right">Region:</td>
            <td><asp:dropdownlist id="ddlRegion" Runat="server" OnSelectedIndexChanged="ddlRegion_OnSelectedIndexChanged" AutoPostBack="true"></asp:dropdownlist></td>
            <td align="right">Area:</td>
            <td><asp:dropdownlist id="ddlArea" Runat="server" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true"></asp:dropdownlist></td>
            <td align="right">School:</td>
            <td><asp:DropDownList runat="server" ID="ddlSchools"></asp:DropDownList></td>
       </tr>
       <tr>
            <td align="right">Categories:</td>
            <td><asp:DropDownList runat="server" ID="ddlCategories"></asp:DropDownList></td>
            <td align="right">Status:</td>
            <td><asp:DropDownList runat="server" ID="ddlStatus">
                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Incomplete" Value="Incomplete"></asp:ListItem>
                <asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
                <asp:ListItem Text="Submited" Value="Submited"></asp:ListItem>
                <asp:ListItem Text="Certified" Value="Certified"></asp:ListItem>
                <asp:ListItem Text="Area" Value="Area"></asp:ListItem>
                <asp:ListItem Text="Region" Value="Region"></asp:ListItem>
                <asp:ListItem Text="Final Score" Value="FinalScore"></asp:ListItem>
                <asp:ListItem Text="Canceled" Value="Canceled"></asp:ListItem>
                <asp:ListItem Text="Elimated" Value="Elimated"></asp:ListItem>
            </asp:DropDownList></td>
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
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlNominees">
      <table width="40%">
        <tr>
            <td><asp:LinkButton runat="server" ID="btnDownloadAll" Text="Download All Individual Photos" OnClick="btnDownloadAll_Click"  /></td>
            <td><asp:LinkButton runat="server" ID="lnkFinalist" Text="Create Finalist Report" OnClick="btnFinal_Click"  /></td>
        </tr>
    </table>
  
    <asp:GridView ID="gvPhotos" runat="server" 
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" onrowdatabound="gvPhotos_RowDataBound" 
            onrowcommand="gvPhotos_RowCommand" >
            <Columns>
                <asp:TemplateField HeaderText="Id"  >
                    <ItemTemplate><asp:Label runat="server" ID="labId" Text='<%#Eval("Id") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Name" >
                    <ItemTemplate><asp:Label runat="server" ID="labName" Text='<%#Eval("User.FullName") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="School">
                    <ItemTemplate><asp:Label runat="server" ID="labSchool" Text='<%#Eval("School.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category" >
                    <ItemTemplate><asp:Label runat="server" ID="labCategory" Text='<%#Eval("Category.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Status" >
                    <ItemTemplate><asp:Label runat="server" ID="labStatus" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgPhoto"  Width="100px" Height="100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="lnkDownLoad" Text="Download" CommandName="Download" ></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlCoordinators" Visible="false">
     <table>
      <tr>
            <td><asp:LinkButton runat="server" ID="lnkGroupDownload" Text="Download All Group Photos" OnClick="lnkGroupDownload_Click"  /></td>
        </tr>
        </table>
        <asp:GridView ID="gvCoordinators" runat="server" 
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            onrowdatabound="gvCoordinators_RowDataBound"
            onrowcommand="gvPhotos_RowCommand" >
            <Columns>
                <asp:TemplateField HeaderText="Id" SortExpression="a.Id" >
                    <ItemTemplate><asp:Label runat="server" ID="labId" Text='<%#Eval("Id") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Name" >
                    <ItemTemplate><asp:Label runat="server" ID="labName" Text='<%#Eval("FullName") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="School" >
                    <ItemTemplate><asp:Label runat="server" ID="labSchool" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Caption" >
                    <ItemTemplate><asp:Label runat="server" ID="labCaption" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <asp:Image runat="server" ID="imgGroupPhoto"  Width="100px" Height="100px"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="lnkDownLoad" Text="Download" CommandName="Download" ></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
    </asp:Panel>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

