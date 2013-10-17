<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Scoring.aspx.cs" Inherits="Admin_Scoring" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

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
    <script type="text/javascript">
        //<!--
        $(function() {
            $("input[type=submit], button").button();
        });
        //-->
    </script>
    <table width="40%">
    <tr>
        <td><asp:HyperLink runat="server" ID="lnkImage" ImageUrl="~/assets/images/ico-back.png" Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink>
        <asp:HyperLink runat="server" ID="lnkBack"  Text="Goback" NavigateUrl="Default.aspx" ></asp:HyperLink></td>
        <td><asp:LinkButton runat="server" ID="lnkNew" Text="Add Score" OnClick="lnkNew_Click"></asp:LinkButton></td>
    </tr>
    <tr>
     <td>Select a Region: <asp:DropDownList runat="server" ID="ddlSearchRegion" onselectedindexchanged="ddlSearchRegion_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList></td>
    </tr>
    </table>
    <br />
  <asp:GridView ID="gvScore" runat="server" 
            AllowSorting="true"
            AutoGenerateColumns="False" 
            ShowHeader="true"
            ShowFooter="true"
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            onrowdatabound="gvScore_RowDataBound" 
            onrowcommand="gvScore_RowCommand" >
            <Columns>
                 <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnEdit" CommandName="EditScore" Text="Edit" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Id" DataField="Id" />
                <asp:BoundField HeaderText="Description" DataField="Name" />
                <asp:BoundField HeaderText="Weight" DataField="Weight"  />
                <asp:BoundField HeaderText="Minimum Range" DataField="MinRange" />
                <asp:BoundField HeaderText="Maxium Range" DataField="MaxRange"/>
                 <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="btnDelete" CommandName="DeleteScore" Text="Delete" CommandArgument='<%#Eval("Id") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
        <uc1:ModalDialog ID="modalDialog" runat="server"  Width="350" Height="250">  
         <Content>
            <asp:Label ID="lblId" runat="server" Visible="false"></asp:Label>
            <fieldset>
                <legend>Edit Score Category</legend>
                 <table>
                    <tr>
                        <td>Name</td>
                        <td><asp:textbox id="txtName" Runat="server" Width="150px" MaxLength="50"></asp:textbox>
                          <asp:RequiredFieldValidator ID="NameRequired" runat="server"   display="Dynamic" 
                                        ControlToValidate="txtName" ErrorMessage="Name is required." 
                                        ToolTip="Name is required." ValidationGroup="ScoreEdit">*</asp:RequiredFieldValidator></td>
                    </tr>
                    <tr>
                      <td>Weight</td>
                        <td><asp:textbox id="txtWeight" Runat="server" Width="50px" MaxLength="10"></asp:textbox>
                            <asp:RangeValidator ID="rngWeight" runat="server" ControlToValidate="txtWeight" Type="Integer"
                                    MaximumValue="100" MinimumValue="1"
                                    ErrorMessage="Weight cannot exceed 100." 
                                    ToolTip="Weight cannot exceed 100." 
                                    Display="Dynamic" 
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="vldWeight" ControlToValidate="txtWeight" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator></td>
                     <td>%</td>
                    </tr>
                    <tr>
                      <td>Min Range</td>
                        <td><asp:textbox id="txtMinRange" Runat="server" Width="50px" MaxLength="10"></asp:textbox>
                           <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtMinRange" Type="Integer"
                                    MaximumValue="100" MinimumValue="0"
                                    ErrorMessage="Range cannot exceed 100." 
                                    ToolTip="Range cannot be lower than 0 or greater than 100." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="txtMinRange" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                      <td>Max Range</td>
                        <td><asp:textbox id="txtMaxRange" Runat="server" Width="50px" MaxLength="10"></asp:textbox>
                          <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtMaxRange" Type="Integer"
                                    MaximumValue="100" MinimumValue="1"
                                    ErrorMessage="Range cannot exceed 100." 
                                    ToolTip="Range cannot exceed 100." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtMaxRange" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator></td>
                    </tr>
                    <tr>
                        <td><asp:validationsummary id="ErrorSummary" runat="server" ShowSummary="true" ValidationGroup="ScoreEdit"/></td>
                    </tr>
                 </table>
            </fieldset>
             <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true"  ValidationGroup="ScoreEdit" onclick="btnSave_Click"/>
                <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="false"  onclick="btnClose_Click" />
            </div>     
          </Content>
        </uc1:ModalDialog>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

