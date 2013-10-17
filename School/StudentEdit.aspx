<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StudentEdit.aspx.cs" Inherits="School_StudentEdit" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    ul.a
    {
        font-family: "PT Serif", sans-serif;
        font-size: 11px;
        font-style: normal;
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
   <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript">
   </script><script src="../assets/javascript/jquery.maskedinput.js" type="text/javascript"></script>
    
  <script type="text/javascript" language="javascript">
      //<!--
      $('#<%= txtPhone.ClientID %>').mask("(999) 999-9999");
      $(document).ready(function() {
            $("input[type=submit], button").button();
      $(function() {
          $("#tabs").tabs();
      });
    });    // Document Ready   
  //-->
  </script>
  
<table width="40%">
    </table>
    <div style="text-align:left;padding:8px;">
        <asp:Button ID="Button1" runat="server" Text="Save" CausesValidation="true" ValidationGroup="UserEdit" onclick="btnSave_Click"/>
        <asp:Button ID="Button2" runat="server" Text="Cancel" CausesValidation="false" ValidationGroup="UserEdit" onclick="btnCancel_Click"/>
 </div> 
    <div id="tabs">
    <ul>
    <li><a href="#tabs-1">Student Information</a></li>
    <li><a href="#tabs-2">Transcripts & Scores</a></li>
    <li><a href="#tabs-3"><asp:Label runat="server" ID="lblStudentName" Text="Missing Items"></asp:Label></a></li>
  </ul>
   <div id="tabs-1">
     <asp:Label ID="Label1" runat="server" Visible="False"></asp:Label>
		        <table cellpadding="5" cellspacing="4px">
                    <tr>
                      <td>Full Name:</td>
                        <td><asp:textbox id="txtFullName" Runat="server" Width="250px" MaxLength="50"></asp:textbox>
                          <asp:RequiredFieldValidator ID="FullNameRequired" runat="server" 
                                        ControlToValidate="txtFullName" ErrorMessage="Name is required." 
                                        ToolTip="Name is required." ValidationGroup="UserEdit">*</asp:RequiredFieldValidator></td>
                      <td>Status:</td>                                        
                      <td><asp:DropDownList runat="server" ID="ddlStatus">
                            <asp:ListItem Value="Incomplete"></asp:ListItem>
                            <asp:ListItem Value="Submited"></asp:ListItem>
                            <asp:ListItem Value="Complete"></asp:ListItem>
                            <asp:ListItem Value="Canceled"></asp:ListItem>
                            <asp:ListItem Value="Certified"></asp:ListItem>
                           </asp:DropDownList></td>                                        
                    </tr>
                    <tr>
                        <td>Email:</td>
                        <td><asp:textbox id="txtEmail" Runat="server" Width="250px"></asp:textbox>
                          <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                                        ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                                        ToolTip="Email is required." ValidationGroup="UserEdit">*</asp:RequiredFieldValidator>
                          <asp:RegularExpressionValidator ID="EmailRegEx" runat="server" ControlToValidate="txtEmail" 
                                ErrorMessage="Incorrect email address entered" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="UserEdit">*</asp:RegularExpressionValidator>
                          <asp:CustomValidator ID="EmailValidator" runat="server" 
                                Text="*" Display="Dynamic" 
                                ValidateEmptyText="True" ValidationGroup="UserEdit"
                                ControlToValidate="txtEmail" 
                                OnServerValidate="EmailValidator_ServerValidate" ></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="labCategory" Text="Category" AssociatedControlID="ddlCategory"></asp:Label></td>                                   
                        <td><asp:dropdownlist id="ddlCategory" Runat="server"></asp:dropdownlist></td>
                        <td></td>
                        <td><asp:LinkButton runat="server" ID="lnkPicture" Text="Upload Picture" OnClick="lnkPicture_Click"></asp:LinkButton></td>
                        <td rowspan="6"><asp:Image runat="server" ID="picNominee"  AlternateText="Nominee" 
                                Visible="False" Width="200px" Height="250px"/></td>
                    </tr>
                     <tr>
                         <td>
                             Address:</td>
                         <td colspan="3">
                             <asp:TextBox ID="txtAddress" runat="server" Width="250px"></asp:TextBox>
                         </td>
               </tr>
               <tr>
                    <td>City:</td>
                    <td><asp:TextBox runat="server" ID="txtCity"></asp:TextBox></td>
                    <td>
                        &nbsp;State:</td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server">
                        </asp:DropDownList>
                    </td>
               </tr>
               <tr>
                    <td>Zip:</td>
                    <td><asp:TextBox runat="server" ID="txtZip"></asp:TextBox></td>
               </tr>
               <tr>
                    <td colspan="4">
                        <asp:Label ID="labCustomError" runat="server" CssClass="validationtext" 
                            Visible="False"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                            ValidationGroup="UserEdit" />
                    </td>
               </tr>
                <tr>
                    <td>Phone:</td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" Width="250px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Gender:</td>
                    <td><asp:DropDownList runat="server" ID="ddlGender">
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList></td>
               </tr>
                  <tr>
                    <td valign="top">Comments:</td>
                    <td colspan="4"><asp:textbox id="txtComment" Runat="server" Width="350px" 
                            Height="55px" TextMode="MultiLine"></asp:textbox></td>
                </tr>
                <tr>
                     <td>Parents:</td>
                     <td colspan="3"><asp:textbox id="txtParents" Runat="server" Width="250px"></asp:textbox></td>
                </tr>
            </table>
    </div>
    <div  id="tabs-2">
     <ul  class="a">
                <li>Proof of ACT scores is required if not on transcript. Students must select from one test the scores they want to use. ALL scores MUST come from the same test.</li>
                <li>Mixing and matching scores from different tests is NOT allowed.</li>
                <li>Please make sure the nominee's class ranking is on the transcript.</li>
            </ul> 
            <table width="50%">
                <tr><td>
                    <asp:CustomValidator runat="server" id="valTrans" OnServerValidate="valTrans_ServerValidate"  ValidationGroup="ScoreEdit" ErrorMessage="A certified list of high school grades must be included in your profile along with proof of ACT and SAT scores" >*</asp:CustomValidator></td>
                </tr>
            </table>
            <asp:Button runat="server" ID="lnkTransUpload" Text="Upload Transcript" OnClick="lnkTrans_Click"></asp:Button>
            <asp:ListView runat="server" ID="lvTranscript">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Uploaded Doc Name</th>
                        <th>Description</th>
                        <th>&nbsp;</th>
                        <th>&nbsp;</th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                  <tr>
                   <td><%#Eval("Name") %></td>
                   <td><%#Eval("Description") %></td>
                   <td><asp:HyperLink runat="server" ID="lblViewDoc" Target="_blank" Text="View" NavigateUrl='<%# "~/DocView.aspx?id=" + Eval("Id").ToString() %>'></asp:HyperLink></td>
                   <td><asp:LinkButton runat="server" ID="lblDeleteLetter" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
                <table>
                    <tr>
                        <td align="left" colspan="5"><strong>American College Testing (ACT)</strong> -- Please enter standard test scores. Test range is a possible low of 1 to a possible high of 36.</td>
                        
                    </tr>
                    <tr>
                        <td colspan="5"><asp:CustomValidator runat="server" id="valACTSAT" 
                                OnServerValidate="valACTSAT_ServerValidate"  
                                ValidationGroup="ScoreEdit" 
                                    ErrorMessage="You must include all ACT OR all SAT scores" ></asp:CustomValidator></td>
                    </tr>
                    <tr>
                        <td align="center"><asp:Label runat="server" ID="Label12" Text="English"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label18" Text="Mathematics"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label19" Text="Reading"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label20" Text="Sci./Reasoning"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label21" Text="Composite"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center"><asp:TextBox runat="server" id="txtACTEnglish" Width="30px" CausesValidation="true"></asp:TextBox>
                            <asp:RangeValidator ID="rngACTEnglish" runat="server" ControlToValidate="txtACTEnglish" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="vldACTEnglish" ControlToValidate="txtACTEnglish" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator></td>
                        
                        <td align="center"><asp:TextBox runat="server" id="txtACTMath" Width="30px" CausesValidation="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldACTMath" ControlToValidate="txtACTMath" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtACTMath" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                                                            
                        <td align="center"><asp:TextBox runat="server" id="txtACTReading" Width="30px" CausesValidation="true" ></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldACTReading" ControlToValidate="txtACTReading" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtACTReading" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>

                        <td align="center"><asp:TextBox runat="server" id="txtACTScience" Width="30px" CausesValidation="true"></asp:TextBox> 
                            <asp:RegularExpressionValidator ID="vldACTScience" ControlToValidate="txtACTScience" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtACTScience" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                                
                        <td align="center"><asp:TextBox runat="server" id="txtACTComposite" Width="30px" CausesValidation="true"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldACTComposite" ControlToValidate="txtACTComposite" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtACTComposite" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                    </tr>
                 </table>
                 <br />
                 <hr />
                 <table>
                     <tr>
                        <td align="left" colspan="3"><strong>Scholastic Assessment Test (SAT)</strong> -- Please list score. A possible high of 800 for each.</td>
                    </tr>
                    <tr>
                        <td align="center"><asp:Label runat="server" ID="Label13" Text="Reading"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label14" Text="Mathematics"></asp:Label></td>
                        <td align="center"><asp:Label runat="server" ID="Label15" Text="Writing"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="center"><asp:TextBox runat="server" id="txtSATReading" Width="30px" ></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldSATReading" ControlToValidate="txtSATReading" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSATReading" Type="Integer"
                                    MaximumValue="800" MinimumValue="1"
                                    ErrorMessage="SAT Scores cannot exceed 800." 
                                    ToolTip="SAT Scores cannot exceed 800." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                        
                        <td align="center"><asp:TextBox runat="server" id="txtSATMath" Width="30px" ></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldSATMath" ControlToValidate="txtSATMath" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtSATMath" Type="Integer"
                                    MaximumValue="800" MinimumValue="1"
                                    ErrorMessage="SAT Scores cannot exceed 800." 
                                    ToolTip="SAT Scores cannot exceed 800." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                        
                        <td align="center"><asp:TextBox runat="server" id="txtSATWriting" Width="30px" ></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldSATWriting" ControlToValidate="txtSATWriting" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="txtSATWriting" Type="Integer"
                                    MaximumValue="800" MinimumValue="1"
                                    ErrorMessage="SAT Scores cannot exceed 800." 
                                    ToolTip="SAT Scores cannot exceed 800." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>

                    </tr>
                   
                    <tr>
                        <td colspan="5"> <asp:ValidationSummary runat="server" ID="MyValidationSummary"  ValidationGroup="ScoreEdit" ShowSummary="true" /></td>
                    </tr>
                </table>
                <br />
                 <div><strong>Proof of ACT scores required if not on transcript. Students must select from one test the scores they want to use. ALL scores MUST come from the same test. Mixing and matching scores from different tests is NOT allowed.<br />
                 **Please make sure your class ranking is on your transcript.</strong>
                    </div>
                <br />
    </div>
    <div id="tabs-3">
     <asp:ListView runat="server" ID="lvItems">
            <LayoutTemplate>
             <table class="grid">
                <tr>
                    <th colspan="2">Missing Items (Items that need to be competed)</th>
                </tr>
                <tr id="itemPlaceholder" runat="server"></tr>
             </table>
            </LayoutTemplate>
            <ItemTemplate>
              <tr>
               <td><asp:Label runat="server" ID="lblName"><%#Eval("Name") %></asp:Label></td>
               <td><asp:Label runat="server" ID="lblDescription"><%#Eval("Description") %></asp:Label></td>
              </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
              <tr style="background-color:#EFEFEF">
                 <td><asp:Label runat="server" ID="lblName" ><%#Eval("Name") %></asp:Label></td>
               <td><asp:Label runat="server" ID="lblDescription"><%#Eval("Description") %></asp:Label></td>
              </tr>
            </AlternatingItemTemplate>
         </asp:ListView>
    </div>
 </div>
 
    
      <uc1:ModalDialog ID="modalDialog" runat="server"  Width="500" Height="350">  
         <Content>
            <h3 style="text-align: center; margin-bottom: 0px;"><asp:Label runat="server" ID="lblTitle"></asp:Label></h3><hr />
            <br />
            <asp:Label runat="server" ID="lblAttachmentId" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="lblAttachmentCategory" Visible="false"></asp:Label>
            <asp:Label runat="server" ID="lblAttachmentName" Visible="false"></asp:Label>
            <div>
	           <asp:FileUpload runat="server" ID="txtFileUpload" CssClass="fileupload"  />
	           <asp:CustomValidator runat="server" ID="validUpload" ControlToValidate="txtFileUpload" ValidationGroup="Upload" OnServerValidate="validUpload_ServerValidate" 
	                ErrorMessage="The file type selected is not a valid file format. Please only attempt to upload files in an approved format" >*</asp:CustomValidator>
            </div>
            <br />
            <div><asp:Label runat="server" ID="lblUploadInstructions" Visible="true"></asp:Label></div>
            <br />
            <div>Valid file formats are: &nbsp;<asp:Label runat="server" ID="lblFormats"></asp:Label></div>
            <div><asp:ValidationSummary runat="server" ID="vadSumm" ValidationGroup="Upload" ShowSummary="true" /></div>
            <br />
            <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnUpload" runat="server" Text="Upload" CausesValidation="true" onclick="btnUpload_Click"/>
                <asp:Button ID="btnClose" runat="server" Text="Cancel" CausesValidation="false" onclick="btnClose_Click" />
            </div>                    
        </Content>
    </uc1:ModalDialog>   
 </asp:Content>
 
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

