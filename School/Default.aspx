<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

<asp:Content ID="contentHeader" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" media="screen" />
    <link href="../assets/css/themes/smoothness/jquery-ui-1.10.2.custom.min.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/tabs.css" rel="stylesheet" type="text/css" />
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
  <script type="text/javascript" language="javascript">
       //<!--
      $(document).ready(function() {
      $("input[type=submit], button").button();
      
        $(function() {
            $("#tabs").tabs();
        });
          function CheckBoxRequired_ClientValidate(sender, e) {
              e.IsValid = jQuery("input:checkbox").is(':checked');
          }
    });    // Document Ready   
  //-->
  </script>
  
 <div class="header"><strong><asp:Label runat="server" ID="lblSchool"></asp:Label></strong></div>
    <table width="80%">
        <tr>
            <td><asp:LinkButton runat="server" ID="lnkNew" Text="Add Nominee" OnClick="lnkNew_Click"></asp:LinkButton></td>
            <td><asp:LinkButton runat="server" ID="lnkSubmit" Text="Submit Nominees" OnClick="lnkSubmit_Click" OnClientClick="return confirm('Are you sure you want to submit all certified nominees? (You will not be able to make any changes to nominee portfolios after submitting all nominees)');"></asp:LinkButton></td>
            <td><asp:LinkButton runat="server" ID="btnNominees" Text="School Nominee Report" OnClick="btnNominees_Click"></asp:LinkButton></td>
            <td><asp:HyperLink runat="server" ID="lnkForm" Target="_blank" Text="Parent/Student Declaration Form" NavigateUrl="../assets/forms/2013-Student-Parent-Declaration-Form.pdf"></asp:HyperLink></td>
            <td><asp:LinkButton runat="server" ID="lnkImage" Text="Upload Group Photo" OnClick="btnImage_Click"></asp:LinkButton></td>
        </tr>
    </table>
    <br />
<div id="tabs">
    <ul>
    <li><a href="#tabs-1">Nominees</a></li>
    <li><a href="#tabs-2">Categories without Nominees</a></li>
    <li><a href="#tabs-3">Group Photo</a></li>
  </ul>
  <div id="tabs-1">
    <asp:GridView ID="gvNominees" runat="server" 
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            OnRowCommand="gvNominees_RowCommand"
            onrowdatabound="gvNominees_RowDataBound" >
            <Columns>
                <asp:TemplateField>
                  <ItemTemplate>
                     <asp:LinkButton runat="server" ID="btnEdit"  CommandName="Select" Text="Edit"></asp:LinkButton>
                     </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate><asp:Label runat="server" ID="labName">   </asp:Label></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Email">
                    <ItemTemplate><asp:Label runat="server" ID="labEmail">   </asp:Label></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Phone">
                    <ItemTemplate><asp:Label runat="server" ID="labPhone">   </asp:Label></ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Category">
                    <ItemTemplate><asp:Label runat="server" ID="labCategory">   </asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnStatus" CommandName="CheckStatus" Text="Status"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnDelete" CommandName="DeleteUser" Text="Delete"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnView" CommandName="ViewPortfolio" Text="View Portfolio"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate><asp:LinkButton runat="server" ID="btnCertify" CommandName="CertifyStudent" Text="Certify"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
               
              </Columns>
           </asp:GridView>
    </div>
    <div id="tabs-2">
            <asp:ListView runat="server" ID="lvMissingCats">
            <LayoutTemplate>
                <table border="0" cellpadding="1">
                    <tr id="itemPlaceholder" runat="server"></tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
              <tr>
               <td><asp:Label runat="server" ID="lblName"><%#Eval("Name") %></asp:Label></td>
              </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
              <tr style="background-color:#EFEFEF">
                 <td><asp:Label runat="server" ID="Label1"><%#Eval("Name") %></asp:Label></td>
              </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>No Missing Categories</EmptyDataTemplate>
         </asp:ListView>
    </div>
    <div id="tabs-3">
          <table>
                    <tr>
                        <td><asp:Button runat="server" ID="btnPhotoUpload" Text="Upload Photo"  OnClick="btnImage_Click" Enabled="true"></asp:Button>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Enabled="true"/></td>
                    </tr>
                    <tr>
                        <td><asp:Image runat="server" ID="imgPhoto"  AlternateText="Group Photo" Visible="false" ImageUrl="" Width="450px" Height="300px"/></td>
                    </tr>
                    <tr>
                        <td valign="top"><strong>Caption (Enter student names as they appear in the photo, this list will appear in all publications of this photo)</strong></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox runat="server" id="txtCaption" TextMode="MultiLine" Columns="50" Rows="10" Height="150px" Width="100%" MaxLength="999" ></asp:TextBox></td>
                    </tr>
                </table>
        </div>
  </div>
  
    <uc1:ModalDialog ID="mdlDlgPrincipal" runat="server"  Width="500" Height="350">  
         <Content>
            <h3 style="text-align: center; margin-bottom: 0px;">Principal's Report</h3><hr />
            <br />
            <asp:Label runat="server" ID="lblId" Visible="false"></asp:Label>
          <table>
             <tr>
                <td>Student Name:</td>
                <td><strong><asp:Label runat="server" ID="lblName"></asp:Label></strong></td>
           </tr>
           <tr>
                <td>School:</td>
                <td><asp:Label runat="server" ID="lblSchool"></asp:Label></td>
           </tr>
           <tr>
                <td>Category:</td>
                <td><asp:Label runat="server" ID="lblCategory" ></asp:Label></td>
            </tr>
          </table>
          <hr />
          <table>
            <tr>
                <td><asp:CheckBox runat="server" ID="chkTranscript" Text="I have included a school transcript for this Sterling Scholar Nominee for grades 9 through 12 and certify that this list is correct."/>
                    <asp:CustomValidator runat="server" ID="CheckBoxRequired" 
                            EnableClientScript="true" 
                            OnServerValidate="CheckBoxRequired_ServerValidate"
                            ClientValidationFunction="CheckBoxRequired_ClientValidate">You must select this box to certify this student.</asp:CustomValidator> 
                </td>
            </tr>
            <tr>
                <td><asp:CheckBox runat="server" ID="chkSchool" Text="I have checked the school-related activities, offices and awards submitted by the nominee and certify that they are correct."/>
                <asp:CustomValidator runat="server" ID="CustomValidator1" 
                            EnableClientScript="true" 
                            OnServerValidate="CheckBoxRequired_ServerValidate"
                            ClientValidationFunction="CheckBoxRequired_ClientValidate">You must select this box to certify this student.</asp:CustomValidator></td>
            </tr>
            <tr>
                <td><asp:CheckBox runat="server" ID="chkForms" Text="I have checked the Student Entry Forms of the nominee and certify that all information is correct to the best of my knowledge."/>
                <asp:CustomValidator runat="server" ID="CustomValidator2" 
                            EnableClientScript="true" 
                            OnServerValidate="CheckBoxRequired_ServerValidate"
                            ClientValidationFunction="CheckBoxRequired_ClientValidate">You must select this box to certify this student.</asp:CustomValidator></td>
            </tr>
            <tr>
                <td><asp:CheckBox runat="server" ID="chkTests" Text="I have checked the Standardized Test Data Sheet and certify that reported scores are correct."/>
                <asp:CustomValidator runat="server" ID="CustomValidator3" 
                            EnableClientScript="true" 
                            OnServerValidate="CheckBoxRequired_ServerValidate"
                            ClientValidationFunction="CheckBoxRequired_ClientValidate">You must select this box to certify this student.</asp:CustomValidator></td>
            </tr>
          </table>
            <br />
            <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnOk" runat="server" Text="Certify" CausesValidation="true" onclick="btnOk_Click"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" onclick="btnCancel_Click" />
                <asp:Button ID="btnReport" runat="server" Text="Print Report" CausesValidation="false" onclick="btnReport_Click" />
            </div>                    
        </Content>
    </uc1:ModalDialog>   
      
    <uc1:ModalDialog ID="modalImageDlg" runat="server"  Width="500" Height="300">  
         <Content>
            <h3 style="text-align: center; margin-bottom: 0px;">Upload Picture</h3><hr />
            <br />
            <div>
	           <asp:FileUpload runat="server" ID="txtFileUpload" CssClass="fileupload " />
            </div>
            <br />
            <div>Group photos must be in JPG or GIF format and should be no larger than 640 x 480 pixels. Images will be automatically resized to these dimensions but we recommend resizing your images before you upload them to avoid any potential problems.</div>
            <br />
            <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" onclick="btnUpload_Click"/>
                <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="false"  onclick="btnDlgClose_Click" />
            </div>                    
        </Content>
    </uc1:ModalDialog>   
    
    
          
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="BottomCenterPlaceHolder" runat="server">
</asp:Content>
