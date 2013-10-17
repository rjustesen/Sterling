<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>

<asp:Content ID="ContentMenu" ContentPlaceHolderID="menuPlaceHolder" runat="server">
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

<asp:Content ID="Content5" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
   <script src="../assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
   <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
   <script src="../assets/javascript/spin.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //<!--
        $(document).ready(function() {
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
        });    // Document Ready   
        //-->
    </script>

    <table width="40%">
    <tr>
        <td><asp:LinkButton runat="server" ID="lnkNew" Text="Create User" OnClick="lnkNew_Click"></asp:LinkButton></td>
        <td><asp:HyperLink runat="server" id="lnkScore" Text="Edit Scoring" NavigateUrl="~/Admin/Scoring.aspx"></asp:HyperLink></td>
        <td><asp:HyperLink runat="server" id="lnkAttachments" Text="Attachments" NavigateUrl="~/Admin/Attachments.aspx"></asp:HyperLink></td>
        <td><asp:HyperLink runat="server" id="lnkNominees" Text="Nominees" NavigateUrl="~/Admin/Nominees.aspx"></asp:HyperLink></td>
        <td><asp:HyperLink runat="server" id="lnkPhotos" Text="Photos" NavigateUrl="~/Admin/Photos.aspx"></asp:HyperLink></td>
    </tr>
    </table>
    <br />
  
    <asp:Panel runat="server" ID="pnlSearch" DefaultButton="btnSearch">
    <table>
        <tr>
            <td>User Type:</td>
            <td><asp:DropDownList runat="server" ID="ddlUserType">
                <asp:ListItem Text="All" Value="-1"></asp:ListItem>
                <asp:ListItem Text="Administrator" Value="0"></asp:ListItem>
                <asp:ListItem Text="Region Administrator" Value="7"></asp:ListItem>
                <asp:ListItem Text="School Coordinator" Value="1"></asp:ListItem>
                <asp:ListItem Text="Area Judge" Value="2"></asp:ListItem>
                <asp:ListItem Text="Region Judge" Value="3"></asp:ListItem>
                <asp:ListItem Text="Nominee" Value="4"></asp:ListItem>
                <asp:ListItem Text="Principal" Value="5"></asp:ListItem>
            </asp:DropDownList></td>
            <td>Region:</td>
            <td><asp:DropDownList runat="server" ID="ddlSearchRegion" 
                    onselectedindexchanged="ddlSearchRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
            <td>Area:</td>
            <td><asp:DropDownList runat="server" ID="ddlSearchArea" 
                    onselectedindexchanged="ddlSearchArea_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>School:</td>
            <td><asp:DropDownList runat="server" ID="ddlSearchSchool"></asp:DropDownList></td>
            <td>Categories:</td>
            <td><asp:DropDownList runat="server" ID="ddlCategories"></asp:DropDownList></td>
            <td colspan="2"><ajaxToolKit:TextBoxWatermarkExtender runat="server" ID="txtSearch" TargetControlID="txtSearchUserName" WatermarkText="Enter Search Filter"></ajaxToolKit:TextBoxWatermarkExtender>
                <asp:TextBox ID="txtSearchUserName" runat="server" Width="250px" MaxLength="50" /> </td>
        </tr>
        <tr>
            <td style="padding-top: 10px; padding-bottom: 10px;">
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" Width="100px"  />
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
   
     <asp:Panel runat="server" ID="pnlResults" Visible="true"> 
        <asp:GridView ID="gvUsers" runat="server" 
            AllowSorting="true"
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            onrowcreated="gvUsers_RowCreated" 
            OnRowCommand="gvUsers_RowCommand"
            onrowdatabound="gvUsers_RowDataBound" 
            OnSorting="gvUsers_Sorting">
            <Columns>
                 <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnEdit" CommandName="Select" Text="Edit" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Id" DataField="Id" />
                <asp:BoundField HeaderText="Full Name" DataField="FullName" SortExpression="FullName" />
                <asp:BoundField HeaderText="Email" DataField="EMail"  SortExpression="EMail"/>
                <asp:BoundField HeaderText="Phone" DataField="PhoneNumber" />
                <asp:BoundField HeaderText="User Type" DataField="Role"  SortExpression="Role"/>
                <asp:TemplateField HeaderText="User Type" SortExpression="Role">
                    <ItemTemplate><asp:Label runat="server" ID="lblRole" Text='<%#Eval("Role") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="School" DataField="School"  SortExpression="School"/>
                <asp:BoundField HeaderText="Category" DataField="Category"   SortExpression="Category"/>
                <asp:TemplateField HeaderText="Portfolio Status" SortExpression="Status">
                    <ItemTemplate><asp:Label runat="server" ID="labStatus">   </asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnDelete" CommandName="DeleteUser" Text="Delete" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnStatus" CommandName="ChangeStatus" Text="Disable" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnPassword" CommandName="ResetPassword" Text="Reset Password" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:HyperLink runat="server" ID="btnView" Text="View Portfolio" Target="_blank"></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnUnlock" CommandName="Unlock" Text="Unlock" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
    </asp:Panel>
    
    <uc1:ModalDialog ID="modalDialog" runat="server"  Width="500" Height="350" >  
         <Content>
            <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
            <fieldset>
            <legend>Edit User</legend>
		        <table>
                    <tr>
                      <td>Full Name</td>
                        <td><asp:textbox id="txtFullName" Runat="server" Width="250px" MaxLength="50"></asp:textbox>
                          <asp:RequiredFieldValidator ID="FullNameRequired" runat="server" 
                                        ControlToValidate="txtFullName" ErrorMessage="Name is required." 
                                        ToolTip="Name is required." ValidationGroup="UserEdit">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" AssociatedControlID="txtEmail" Text="Email"></asp:Label></td>
                        <td><asp:textbox id="txtEmail" Runat="server" Width="250px"></asp:textbox>
                          <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                        ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                                        ToolTip="Email is required." ValidationGroup="UserEdit">*</asp:RequiredFieldValidator>
                          <asp:RegularExpressionValidator ID="EmailRegEx" runat="server" ControlToValidate="txtEmail" 
                                ErrorMessage="Incorrect email address entered" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="UserEdit">*</asp:RegularExpressionValidator></td>
                    </tr>
                     <tr>
                        <td>Phone</td>
                        <td><asp:textbox id="txtPhone" Runat="server" Width="250px"></asp:textbox></td>
                    </tr>
                    <tr>
                     <td>Comment</td>
                     <td colspan="2"><asp:textbox id="txtComment" Runat="server" TextMode="MultiLine" Width="250px" Wrap="true"></asp:textbox></td>
                    </tr>
                     <tr>       
                        <td><asp:Label runat="server" ID="labRole" Text="User Role" AssociatedControlID="ddlRoleType"></asp:Label></td>                 
                        <td><asp:dropdownlist id="ddlRoleType" Runat="server" OnSelectedIndexChanged="ddlRoleType_OnSelectedIndexChanged" AutoPostBack="true">
                                    
                                </asp:dropdownlist>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labRegion" Text="Region" AssociatedControlID="ddlRegion"></asp:Label></td>             
                        <td><asp:dropdownlist id="ddlRegion" Runat="server" OnSelectedIndexChanged="ddlRegion_OnSelectedIndexChanged" AutoPostBack="true">
                                </asp:dropdownlist></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labArea" Text="Area" AssociatedControlID="ddlArea"></asp:Label></td>                          
                        <td><asp:dropdownlist id="ddlArea" Runat="server" OnSelectedIndexChanged="ddlArea_OnSelectedIndexChanged" AutoPostBack="true"></asp:dropdownlist></td>
                    </tr>
                     <tr>
                        <td><asp:Label runat="server" ID="labSchool" Text="School" AssociatedControlID="ddlSchool"></asp:Label></td>             
                        <td><asp:dropdownlist id="ddlSchool" Runat="server" >
                                </asp:dropdownlist></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labCategory" Text="Category" AssociatedControlID="ddlCategory"></asp:Label></td>                                   
                        <td><asp:dropdownlist id="ddlCategory" Runat="server"></asp:dropdownlist></td>
                    </tr>
                    <tr>
                        <td colspan="2"><asp:Label runat="server" ID="labCustomError" CssClass="validationtext" Text="" Visible="false"></asp:Label>
                          <asp:ValidationSummary runat="server" ID="MyValidationSummary" ValidationGroup="UserEdit" 
                                ShowSummary="false"   ShowMessageBox="true" /></td>
                    </tr>
                </table>
                </fieldset>
            <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true"  ValidationGroup="UserEdit" onclick="btnSave_Click"/>
                <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="false"  onclick="btnClose_Click" />
            </div>                    
        </Content>
    </uc1:ModalDialog>   
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="BottomCenterPlaceHolder" runat="server">
</asp:Content>
