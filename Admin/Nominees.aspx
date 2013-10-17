<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Nominees.aspx.cs" Inherits="Admin_Nominees" %>

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
        $(document).ready(function() {
        $("input[type=submit], button").button();
        $("#<%=gvNominees.ClientID%> input[id*='chkNominee']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);
        $("#<%=gvNominees.ClientID%> input[id*='chkAll']:checkbox").click(function() {
                if ($(this).is(':checked'))
                    $("#<%=gvNominees.ClientID%> input[id*='chkNominee']:checkbox").attr('checked', true);
                else
                    $("#<%=gvNominees.ClientID%> input[id*='chkNominee']:checkbox").attr('checked', false);
            });
        });

        function CheckUncheckAllCheckBoxAsNeeded() {
            var totalCheckboxes = $("#<%=gvNominees.ClientID%> input[id*='chkNominee']:checkbox").size();
            var checkedCheckboxes = $("#<%=gvNominees.ClientID%> input[id*='chkNominee']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {
                $("#<%=gvNominees.ClientID%> input[id*='chkAll']:checkbox").attr('checked', true);
            }
            else {
                $("#<%=gvNominees.ClientID%> input[id*='chkAll']:checkbox").attr('checked', false);
            }
        }
      
        function switchViews(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "inline";
                if (row == 'alt') {
                    img.src = "../assets/images/close.gif";
                }
                else {
                    img.src = "../assets/images/close.gif";
                }
                img.alt = "Click to Collapse";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "../assets/images/detail.gif";
                }
                else {
                    img.src = "../assets/images/detail.gif";
                }
                img.alt = "Click to Expand";
            }
        }

        $(function() {
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
            <td><asp:Button runat="server" id="btnAdvance" Text="Advance" 
                        OnClientClick="return confirm('Are you sure you want to advance the scores for all selected nominees?\n(Please check to make sure all judges have entered their scores)');" 
                        Visible="false"  
                        OnClick="btnAdvance_Click" 
                        ToolTip="Advance selected nominees to the next phase" /></td>
            <td><asp:Button runat="server" id="btnCancel" Text="Cancel" 
                        OnClientClick="return confirm('Are you sure you want set a status of Canceled for the selected nominees?');" 
                        Visible="false"  
                        onclick="btnCancel_Click" 
                        ToolTip="Cancel selected nominees" /></td>               
             <td><asp:Button runat="server" id="btnEliminate" Text="Eliminate" 
                        OnClientClick="return confirm('Are you sure you want set a status of Eliminated for the selected nominees?');" 
                        Visible="false"  
                        OnClick="btnEliminate_Click" 
                        ToolTip="Eliminate selected nominees" /></td>                
        </tr>
    </table>
    </asp:Panel>
   
    <asp:Panel runat="server" ID="pnlResults" Visible="true">      
    <asp:GridView ID="gvNominees" runat="server" 
            AllowSorting="true"
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            onrowcreated="gvNominees_RowCreated" 
            OnRowCommand="gvNominees_RowCommand"
            OnRowDataBound="gvNominees_RowDataBound" 
            OnRowEditing="gvNominees_RowEditing" 
            OnRowUpdating="gvNominees_RowUpdating" 
             OnRowCancelingEdit="gvNominees_RowCancelingEdit"  
            OnSorting="gvNominees_Sorting">
            <Columns>
                <asp:TemplateField>
                           <ItemTemplate>
                               <a href="javascript:switchViews('div<%# Eval("Id") %>', 'one');">
                                   <img id="imgdiv<%# Eval("Id") %>" alt="Click to Expand" src="../assets/images/detail.gif" />
                               </a>
                           </ItemTemplate>
                           <AlternatingItemTemplate>
                               <a href="javascript:switchViews('div<%# Eval("Id") %>', 'alt');">
                                   <img id="imgdiv<%# Eval("Id") %>" alt="Click to Expand" src="../assets/images/detail.gif" />
                               </a>
                           </AlternatingItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox runat="server" ID="chkAll" Text="All" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkNominee" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" /> 
                <asp:TemplateField HeaderText="Id" SortExpression="p.Id" >
                    <ItemTemplate><asp:Label runat="server" ID="labId" Text='<%#Eval("Id") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Name" SortExpression="p.User.FullName" >
                    <ItemTemplate><asp:Label runat="server" ID="labName" Text='<%#Eval("User.FullName") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="School" SortExpression="p.School.Name">
                    <ItemTemplate><asp:Label runat="server" ID="labSchool" Text='<%#Eval("School.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category" SortExpression="p.Category.Name">
                    <ItemTemplate><asp:Label runat="server" ID="labCategory" Text='<%#Eval("Category.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" SortExpression="p.Status">
                    <ItemTemplate><asp:Label runat="server" ID="labStatus"  ></asp:Label></ItemTemplate>
                    <EditItemTemplate> 
                          <asp:DropDownList runat="server" ID="ddlEditStatus" >
                            <asp:ListItem Text="Incomplete" Value="Incomplete"></asp:ListItem>
                            <asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
                            <asp:ListItem Text="Submited" Value="Submited"></asp:ListItem>
                            <asp:ListItem Text="Certified" Value="Certified"></asp:ListItem>
                            <asp:ListItem Text="Area Pre-Interview" Value="AreaPreInterview"></asp:ListItem>
                            <asp:ListItem Text="Area Interview" Value="AreaInterview"></asp:ListItem>
                            <asp:ListItem Text="Region Pre-Interview" Value="RegionPreInterview"></asp:ListItem>
                            <asp:ListItem Text="Region Interview" Value="RegionInterview"></asp:ListItem>
                            <asp:ListItem Text="Final Score" Value="FinalScore"></asp:ListItem>
                            <asp:ListItem Text="Canceled" Value="Canceled"></asp:ListItem>
                            <asp:ListItem Text="Elimated" Value="Elimated"></asp:ListItem>
                          </asp:DropDownList>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Score" SortExpression="p.TotalScore">
                    <ItemTemplate><asp:Label runat="server" ID="labTotalScore" Text='<%#Eval("TotalScore") %>' ></asp:Label></ItemTemplate>
                    <EditItemTemplate> 
                          <asp:TextBox ID="txtEditScore" runat="server" Text='<%# Bind("TotalScore") %>'></asp:TextBox> 
                          <asp:RangeValidator ID="rangeValidator1" runat="server" ControlToValidate="txtEditScore" Type="Double"
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                          <asp:RegularExpressionValidator ID="regValidator1" ControlToValidate="txtEditScore" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(\d+\.\d+)|(\d+\.)|(\.\d+)|(\d+)" Runat="server"></asp:RegularExpressionValidator>
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ranking" SortExpression="p.Ranking">
                    <ItemTemplate><asp:Label runat="server" ID="labRanking" Text='<%#Eval("Ranking") %>' ></asp:Label></ItemTemplate>
                    <EditItemTemplate> 
                          <asp:TextBox ID="txtEditRanking" runat="server" Text='<%# Bind("Ranking") %>'></asp:TextBox>   
                          <asp:RangeValidator ID="rangeValidator2" runat="server" ControlToValidate="txtEditRanking" Type="Double"
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                          <asp:RegularExpressionValidator ID="regValidator2" ControlToValidate="txtEditRanking" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(\d+\.\d+)|(\d+\.)|(\.\d+)|(\d+)" Runat="server"></asp:RegularExpressionValidator>                        
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:HyperLink runat="server" ID="lblView" Target="_blank" Text="View Portfolio" NavigateUrl='<%# "~/ReportView.aspx?report=profile&id=" + Eval("Id").ToString() %>'></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                        <ItemTemplate>
                         <tr>
                            <td colspan="100%">
                            <div id="div<%# Eval("Id") %>" style="display:none;position:relative;left:10px;" >
                            <span><strong>Judges</strong></span>
			                 <asp:ListView ID="lvScores" runat="server" 
			                    OnItemDataBound="lvScores_ItemDataBound">
			                    <LayoutTemplate>
                                    <table border="0" cellpadding="1" class="grid">
                                        <tr>
                                            <th align="left">Name</th>
                                            <th align="left">Role</th>
                                            <th align="left">Status</th>
                                            <th align="left">Score</th>
                                            <th align="left">Submitted</th>
                                            <th></th>
                                            <th></th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server"></tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td><asp:Label runat="server" ID="lblName"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="lblRole"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labStatus"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labScore"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labSubmitted"></asp:Label></td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr style="background-color:#EFEFEF">
                                        <td><asp:Label runat="server" ID="lblName"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="lblRole"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labStatus"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labScore"></asp:Label></td>
                                        <td><asp:Label runat="server" ID="labSubmitted"></asp:Label></td>
                                    </tr>
                                </AlternatingItemTemplate>
                             </asp:ListView>
                        </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <AlternatingRowStyle CssClass="alt" />
        </asp:GridView>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

