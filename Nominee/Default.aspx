<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="Nominee_Default" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/accordion.css" rel="stylesheet" type="text/css" />
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
      .counter {
	    font-family: Arial, Helvetica, sans-serif;
	    font-style: normal;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
<asp:Panel ID="pnlMenu" runat="server" >
    <div id="menu">
        <ul>
	    <li  class="first active"><asp:HyperLink runat="server" ID="lnkHome" Text="Home" NavigateUrl="#" ></asp:HyperLink></li>
	    <li><asp:HyperLink ID="HyperLink2" runat="server" Text="Account" NavigateUrl="~/Profile.aspx"></asp:HyperLink></li>
        </ul>
        <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <script src="../assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script src="../assets/javascript/jquery.maskedinput.js" type="text/javascript"></script>
    
<script type="text/javascript">


    $(function() {
        
        var limit = 500;    
        $("input[type=submit], button").button();
        $('#<%= txtPhone.ClientID %>').mask("(999) 999-9999");
        function count(id) {
            if (null != id) {
                var txtVal = $(id).val();
                var words = txtVal.trim().replace(/\s+/gi, ' ').split(' ').length;
                var chars = txtVal.length;
                if (chars === 0) { words = 0; }
                $('.counter').html('<br>' + words + ' words and ' + chars + ' characters');
                if (words >= limit) {
                    alert('You have exceeded the limit of 500 words');
                }
            }
        }
        //count(null);

        $('#<%= txtCategoryDescription.ClientID %>').on('keyup propertychange paste', function() {
            var input = '#' + this.id;
            count(input);
        });

        $('#<%= txtCitizenshipDescription.ClientID %>').on('keyup propertychange paste', function() {
            var input = '#' + this.id;
            count(input);
        });

        $('#<%= txtLeadershipDescription.ClientID %>').on('keyup propertychange paste', function() {
            var input = '#' + this.id;
            count(input);
        });

        $('#<%= txtUniqueQualities.ClientID %>').on('keyup propertychange paste', function() {
            var input = '#' + this.id;
            count(input);
        });

        $('#<%= txtLifeEnrichment.ClientID %>').on('keyup propertychange paste', function() {
            var input = '#' + this.id;
            count(input);
        });
    });
    
    function pageLoad() {
        var id = '<%= AccordianControl.ClientID %>';
        id += "_AccordionExtender";
        var accordion = $find(id);
        if (accordion) {
            accordion.add_selectedIndexChanged(onSelectedIndexChanged);
        }
    }

    function onSelectedIndexChanged(sender, eventArgs) {
        var id = '<%= AccordianControl.ClientID %>';
        id += "_AccordionExtender";
        var ctl = $find(id);
        var i = ctl.get_SelectedIndex();
        if (i > -1) {
            __doPostBack(id, i);
        }
    }
  
</script>

 <table width="80%">
            <tr>
                <td><asp:LinkButton runat="server" ID="lnkView" Text="View Portfolio" onclick="lnkView_Click" ></asp:LinkButton></td>
                <td><asp:LinkButton runat="server" ID="lnkSubmit" Text="Submit Portfolio" onclick="btnSubmit_Click" OnClientClick="return confirm('Are you sure you want to submit your profile?\nSubmitting your profile will mark it as completed and you will not be able to make changes to it.');" CausesValidation="true" ></asp:LinkButton></td>
                <td><asp:HyperLink runat="server" ID="lnkForm" Text="Parent/Student Declaration Form" NavigateUrl="../assets/forms/2013-Student-Parent-Declaration-Form.pdf" onclick="window.open (this.href, 'popupwindow',  'width=800,height=600,scrollbars,resizable'); return false;"></asp:HyperLink></td>
            </tr>
        </table>
    <hr />
    <table width="100%">
     <tr>
        <td><asp:Label runat="server" ID="lblName" CssClass="headerText"></asp:Label></td>
     </tr>
    </table>
    <hr />
  <ajaxToolkit:Accordion  
            runat="server" ID="AccordianControl"
            SelectedIndex="0"  
            RequireOpenedPane="true"
            SuppressHeaderPostbacks="false" 
            CssClass="accordion"  
            FadeTransitions="true"
            TransitionDuration="100"
            FramesPerSecond="20"
            HeaderCssClass="accordionHeader"  
            HeaderSelectedCssClass="accordionHeaderSelected"  
            ContentCssClass="accordionContent" >
    <Panes>
        <ajaxToolkit:AccordionPane runat="server" ID="AccordionPane1" >
        <Header><input type="image" src="<%= GetImage('0')%>" name="image1" style="vertical-align:middle" />Personal Info</Header>
        <Content>
        <table>
               <tr>
                    <td class="headerText">&nbsp;School:</td>
                    <td class="headerText"><asp:Label runat="server" ID="lblSchoolEdit"></asp:Label></td>
                    <td class="headerText"> &nbsp;Category:</td>
                    <td class="headerText"><asp:Label runat="server" ID="lblCategoryEdit" ></asp:Label></td>
               </tr>
               <tr>
                    <td>&nbsp;Name of Nominee:</td>
                    <td><asp:TextBox runat="server" ID="txtName"></asp:TextBox></td>
                    <td></td>
                    <td><asp:Button runat="server" ID="lnkPicture" Text="Upload Picture" OnClick="lnkPicture_Click" ></asp:Button></td>
                    <td rowspan="7"><asp:Image runat="server" ID="picNominee"  AlternateText="Nominee" Visible="false" ImageUrl="" Width="200px" Height="250px"/></td>
               </tr>
              
               <tr>
                    <td>&nbsp;Address:</td>
                    <td colspan="3"><asp:TextBox runat="server" ID="txtAddress"  Width="450px"></asp:TextBox></td>
               </tr>
               <tr>
                    <td>&nbsp;City:</td>
                    <td><asp:TextBox runat="server" ID="txtCity"></asp:TextBox></td>
                    <td>&nbsp;State:</td>
                    <td><asp:DropDownList runat="server" ID="ddlState">
                            </asp:DropDownList></td>
               </tr>
               <tr>
                    <td>&nbsp;Zip:</td>
                    <td><asp:TextBox runat="server" ID="txtZip"></asp:TextBox></td>
               </tr>
               <tr>
                    <td>&nbsp;Phone:</td>
                    <td><asp:TextBox runat="server" ID="txtPhone" MaxLength="14" ></asp:TextBox></td>
               </tr>
                <tr>
                    <td>&nbsp;Gender:</td>
                    <td><asp:DropDownList runat="server" ID="ddlGender">
                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                    </asp:DropDownList></td>
               </tr>
               <tr>
                    <td>&nbsp;Parents:</td>
                    <td colspan="3"><asp:TextBox runat="server" ID="txtParents" Width="450px"></asp:TextBox></td>
               </tr>
             </table>
             <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="btnSave" runat="server" Text="Save"  OnClick="btnSave_Click" CommandArgument="Personal"  />
                     </td>
                 </tr>
             </table>
        </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane runat="server" ID="AccordionPane2" >
        <Header><input type="image" src="<%= GetImage('1')%>" name="image2" style="vertical-align:middle" />Test Scores</Header>
        <Content>
            <ul>
                <li>School Coordinators are responsible for all test scores and transcripts. If you see no scores here, please see your school coordinator.</li>
                <li>Proof of ACT scores is required if not on transcript. Students must select from one test the scores they want to use. ALL scores MUST come from the same test.</li>
                <li>Mixing and matching scores from different tests is NOT allowed.</li>
                <li>Please make sure your class ranking is on your transcript.</li>
            </ul> 
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
                        <td align="center"><asp:TextBox runat="server" id="txtACTEnglish" Width="30px" CausesValidation="true" Enabled="false"></asp:TextBox>
                            <asp:RangeValidator ID="rngACTEnglish" runat="server" ControlToValidate="txtACTEnglish" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                            <asp:RegularExpressionValidator ID="vldACTEnglish" ControlToValidate="txtACTEnglish" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator></td>
                        
                        <td align="center"><asp:TextBox runat="server" id="txtACTMath" Width="30px" CausesValidation="true" Enabled="false"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldACTMath" ControlToValidate="txtACTMath" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtACTMath" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                                                            
                        <td align="center"><asp:TextBox runat="server" id="txtACTReading" Width="30px" CausesValidation="true" Enabled="false"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldACTReading" ControlToValidate="txtACTReading" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtACTReading" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>

                        <td align="center"><asp:TextBox runat="server" id="txtACTScience" Width="30px" CausesValidation="true" Enabled="false"></asp:TextBox> 
                            <asp:RegularExpressionValidator ID="vldACTScience" ControlToValidate="txtACTScience" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtACTScience" Type="Integer"
                                    MaximumValue="36" MinimumValue="1"
                                    ErrorMessage="ACT Scores cannot exceed 36." 
                                    ToolTip="ACT Scores cannot exceed 36." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                                
                        <td align="center"><asp:TextBox runat="server" id="txtACTComposite" Width="30px" CausesValidation="true" Enabled="false"></asp:TextBox>
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
                        <td align="center"><asp:TextBox runat="server" id="txtSATReading" Width="30px" Enabled="false"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldSATReading" ControlToValidate="txtSATReading" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="txtSATReading" Type="Integer"
                                    MaximumValue="800" MinimumValue="1"
                                    ErrorMessage="SAT Scores cannot exceed 800." 
                                    ToolTip="SAT Scores cannot exceed 800." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                        
                        <td align="center"><asp:TextBox runat="server" id="txtSATMath" Width="30px" Enabled="false"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="vldSATMath" ControlToValidate="txtSATMath" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(^([0-9]*|\d*\d{1}?\d*)$)" Runat="server"></asp:RegularExpressionValidator>
                            <asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="txtSATMath" Type="Integer"
                                    MaximumValue="800" MinimumValue="1"
                                    ErrorMessage="SAT Scores cannot exceed 800." 
                                    ToolTip="SAT Scores cannot exceed 800." 
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator></td>
                                        
                        <td align="center"><asp:TextBox runat="server" id="txtSATWriting" Width="30px" Enabled="false"></asp:TextBox>
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
                 <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button1" runat="server" Text="Save"  OnClick="btnSave_Click" CommandArgument="ScoreEdit" Enabled="false" />
                     </td>
                 </tr>
             </table>
        </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane3" >
         <Header><input type="image" src="<%= GetImage('2')%>" name="image3" style="vertical-align:middle" />Letter of Recommendation</Header>
         <Content>
                <strong>Include a Letter of Recommendation from any teacher or instructor in a supported format</strong><br />
                <asp:Button runat="server" ID="lnkLetter" OnClick="lnkLetter_Click" Text="Upload Letter" ></asp:Button>
                <asp:ListView runat="server" ID="lvLetters">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:HyperLink runat="server" ID="lblViewLetter" Target="_blank" Text="View" NavigateUrl='<%# "~/DocView.aspx?id=" + Eval("Id").ToString() %>'></asp:HyperLink></td>
                   <td><asp:LinkButton runat="server" ID="lblDeleteLetter" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
                </asp:ListView>
                <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button2" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Transcript"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane4" >
         <Header><input type="image" src="<%= GetImage('3')%>" name="image4" style="vertical-align:middle" />Category Scholarship Activities</Header>
         <Content>
            <table>
                    <tr>
                        <td align="left" colspan="2"><strong>In 20 words or less, list up to six activities, honors and awards relating to your category.</strong></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="lblActivity1" Text="1."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory0" Width="733px" MaxLength="3000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="Label3" Text="2."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory1" Width="733px" MaxLength="3000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="Label4" Text="3."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory2" Width="733px" MaxLength="3000" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="Label5" Text="4."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory3" Width="733px" MaxLength="3000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="Label6" Text="5."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory4" Width="733px" MaxLength="3000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="Label7" Text="6."></asp:Label></td>
                        <td><asp:TextBox runat="server" id="txtCategory5" Width="733px" MaxLength="3000" ></asp:TextBox></td>
                    </tr>
                 </table>
                 <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button3" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Category"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane >
        <ajaxToolkit:AccordionPane runat="server" ID="AccordionPane5"  >
         <Header><input type="image" src="<%= GetImage('4')%>" name="image5" style="vertical-align:middle" />Category Scholarship Activity Description</Header>
         <Content>
         <table>
                   <tr>
                      <td align="left" colspan="2" ><strong>Choose one of the activities, honors or awards, describe it briefly and explain why it was meaningful to you. What did you learn and what did you accomplish? <br />Please limit your response to 500 words or less.</strong></td>
                   </tr>
                   <tr>
                      <td><div  class="counter"></div></td>
                   </tr>
                   <tr>
                      <td> <asp:TextBox runat="server" ID="txtCategoryDescription" TextMode="MultiLine" Columns="50" Rows="30" Height="150px" Width="100%" MaxLength="3000" ></asp:TextBox></td>
                   </tr>
                </table>
                <br /><strong>Supporting Documents</strong><br />
                <ul><li>Upload files or documents that support the information you have entered, ie: certificates, newspaper stories, etc.</li></ul>
                <asp:Button runat="server" ID="lnkCatDocsUpload" OnCommand="cmdUpload_Command" CommandArgument="CategoryDescription" Text="Upload Files" ></asp:Button>
                <asp:ListView runat="server" ID="lvCatDocs">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
                <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button4" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="CatDescription"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane6" >
         <Header><input type="image" src="<%= GetImage('5')%>" name="image6" style="vertical-align:middle" />Leadership</Header>
         <Content>
             <table>
                <tr>
                    <td align="left" colspan="2" ><strong>List up to six activities, honors and awards that relate to the leadership qualifications.</strong></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label8" Text="1."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership0" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label9" Text="2."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership1" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label10" Text="3."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership2" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label11" Text="4."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership3" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label114" Text="5."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership4" Width="733px" MaxLength="3000" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label115" Text="6."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtLeadership5" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
            </table>
            <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button5" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Leadership"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
         <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane7" >
         <Header><input type="image" src="<%= GetImage('6')%>" name="image7" style="vertical-align:middle" />Leadership Description</Header>
         <Content>
                 <table>
                   <tr>
                      <td align="left" colspan="2" ><strong>Choose one of the activities, honors or awards that relate to the leadership qualification and describe it briefly. <br />Please limit your response to 500 words or less.</strong></td>
                   </tr>
                    <tr>
                      <td><div  class="counter"></div></td>
                   </tr>
                   <tr>
                      <td> <asp:TextBox runat="server" ID="txtLeadershipDescription" TextMode="MultiLine" Columns="50" Rows="30" Height="200px" Width="700px" MaxLength="3000"></asp:TextBox></td>
                   </tr>
                </table>
                <br /><strong>Supporting Documents</strong><br />
                <ul><li>Upload files or documents that support the information you have entered, ie: certificates, newspaper stories, etc.</li></ul>
                <asp:Button runat="server" ID="lnkLeadership" OnCommand="cmdUpload_Command" CommandArgument="LeadershipDescription" Text="Upload Files" ></asp:Button>
                <asp:ListView runat="server" ID="lvLeadership">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
                <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button6" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="LeadershipDesc"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
         <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane8" >
         <Header><input type="image" src="<%= GetImage('7')%>" name="image8" style="vertical-align:middle" />Community Service / Citizenship</Header>
         <Content>
         <table>
                <tr>
                    <td align="left" colspan="2" ><strong>List up to six activities, honors or awards that relate to the qualifications for Community Service and/or Citizenship.</strong></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label16" Text="1."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship0" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label17" Text="2."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship1" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label22" Text="3."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship2" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label23" Text="4."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship3" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label24" Text="5."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship4" Width="733px" MaxLength="3000" ></asp:TextBox></td>
                </tr>
                <tr>
                    <td><asp:Label runat="server" ID="Label25" Text="6."></asp:Label></td>
                    <td><asp:TextBox runat="server" id="txtCitizenship5" Width="733px" MaxLength="3000"></asp:TextBox></td>
                </tr>
            </table>
            <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button7" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Citizenshio"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
         <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane9" >
         <Header><input type="image" src="<%= GetImage('8')%>" name="image9" style="vertical-align:middle" />Community Service / Citizenship Description</Header>
         <Content>
          <table>
                   <tr>
                      <td align="left" colspan="2" ><strong>Choose one of the activities, honors or awards that relate to the Community Service/ Citizenship qualification and describe it briefly. <br />Please limit your response to 500 words or less.</strong></td>
                   </tr>
                    <tr>
                      <td><div  class="counter"></div></td>
                   </tr>
                   <tr>
                      <td> <asp:TextBox runat="server" ID="txtCitizenshipDescription" TextMode="MultiLine" Columns="50" Rows="30" Height="200px" Width="700px" MaxLength="3000"></asp:TextBox></td>
                   </tr>
                </table>
                <br /><strong>Supporting Documents</strong><br />
                <ul><li>Upload files or documents that support the information you have entered, ie: certificates, newspaper stories, etc.</li></ul>
                <asp:Button runat="server" ID="lnkCitizenship" OnCommand="cmdUpload_Command"   CommandArgument="CitizenshipDescription" Text="Upload Files" ></asp:Button>
                <asp:ListView runat="server" ID="lvCitizenship">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
                <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button8" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="CitizenDesc"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
         <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane10" >
         <Header><input type="image" src="<%= GetImage('9')%>" name="image10" style="vertical-align:middle" />Unique Qualities</Header>
         <Content>
              <table>
                        <tr>
                            <td align="left" colspan="2"><strong>Describe two or three unique things about yourself and relate them to your category. If possible, include challenges you have overcome and describe any distinctive experiences that have affected you.<br />Please limit your response to 500 words or less.</strong></td>
                        </tr>
                        <tr>
                            <td><div  class="counter"></div></td>
                        </tr>
                        <tr>
                            <td> <asp:TextBox runat="server" ID="txtUniqueQualities" TextMode="MultiLine" Columns="50" Rows="30" Height="200px" Width="700px" MaxLength="3000"></asp:TextBox></td>
                        </tr>
                    </table>
                    <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button9" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Unique"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane  runat="server" ID="AccordionPane11" >
         <Header><input type="image" src="<%= GetImage("10")%>" name="image11" style="vertical-align:middle" />Life Enrichment</Header>
         <Content>
          <table>
                    <tr>
                        <td align="left" colspan="2"><strong>How do you think your involvment in this category will enrich your life?<br />Please limit your response to 500 words or less.</strong></td>
                    </tr>
                     <tr>
                      <td><div  class="counter"></div></td>
                   </tr>
                    <tr>
                        <td> <asp:TextBox runat="server" ID="txtLifeEnrichment" TextMode="MultiLine" Columns="50" Rows="30" Height="200px" Width="700px" MaxLength="3000"></asp:TextBox></td>
                    </tr>
                </table>
                <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                          <asp:Button ID="Button10" runat="server" Text="Save" OnClick="btnSave_Click" CommandArgument="Enrichment"  />
                     </td>
                 </tr>
             </table>
         </Content>
        </ajaxToolkit:AccordionPane>
        <ajaxToolkit:AccordionPane runat="server" ID="AccordionPane12" >
         <Header><input type="image" src="<%= GetImage("11")%>" name="image12" style="vertical-align:middle" />Additional Attachments</Header>
         <Content>
                <span class="headerText">Upload/Link to Documents,Images, Media, Etc.</span>
                <hr />
                <ul>
                    <li class="headerText">Documents</li>
                    <li>You may upload 1 document in several file formats, including .PDF, .TXT, or .RTF.</li>
                    <li>You may also add a link to a document online. Make sure the link is public and you link directly to the document.</li>
                </ul>
                <asp:Button runat="server" ID="btnUploadDocs" OnCommand="cmdUpload_Command"   CommandArgument="Document" Text="Upload File" ></asp:Button>
                <asp:ListView runat="server" ID="lvAttachDocs">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
             <table cellpadding="3" cellspacing="3" width="100%">
                <tr>
                    <td>Add link</td></tr>
                 <tr>
                     <td>Description:</td>
                     <td><asp:TextBox runat="server" ID="txtDocDescription" Width="200px" MaxLength="100"></asp:TextBox></td>
                     <td>Url:</td>
                     <td><asp:TextBox runat="server" ID="txtDocLink" Width="500px"></asp:TextBox></td>
                 </tr>
             </table>
                <ul>
                    <li />
                    <li class="headerText">Images</li>
                    <li>You may upload 1 .JPG, .PNG, or .TIF file (Not to exceed 2 MB in size)</li>
                    <li>You may also add a link to an image online. Make sure the link is public so that judges can view it.</li>
                </ul>
                <asp:Button runat="server" ID="btnImage" OnCommand="cmdUpload_Command"  CommandArgument="Image" Text="Upload File" ></asp:Button>
                <asp:ListView runat="server" ID="lvImage">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
             <table cellpadding="3" cellspacing="3" width="100%">
                 <tr>
                    <td>Add link</td></tr>
                 <tr>
                     <td>Description:</td>
                     <td><asp:TextBox runat="server" ID="txtImageDescription" Width="200px" MaxLength="100"></asp:TextBox></td>
                     <td>Url:</td>
                     <td><asp:TextBox runat="server" ID="txtImageLink" Width="500px"></asp:TextBox></td>
                 </tr>
             </table>
              <ul>
                    <li />
                    <li class="headerText">Media</li>
                    <li>Media can be a maximum of 5 minutes in length</li>
                    <li>You may upload a media file (.WAV, .WMA, .MP4,.MPG,.ACC,.MP3). For video formats, please use a link to the vidoe on an external site.</li>
                    <li>You may share a link to anywhere your video is online. Make sure the link is public so that judges can view it.</li>
                </ul>
                <asp:Button runat="server" ID="btnMedia" OnCommand="cmdUpload_Command"  CommandArgument="Media" Text="Upload File" ></asp:Button>
                <asp:ListView runat="server" ID="lvMedia">
                <LayoutTemplate>
                    <table cellpadding="3" class="grid">
                        <tr>
                        <th>Name</th>
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
                   <td><asp:LinkButton runat="server" ID="lblDeleteDoc" Text="Delete" CommandArgument='<%# Eval("Id") %>' OnCommand="btnDelAttachment_Command" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"></asp:LinkButton></td>
                 </tr>
                </ItemTemplate>
                </asp:ListView>
             <table cellpadding="3" cellspacing="3" width="100%">
                 <tr>
                    <td>Add link</td></tr>
                 <tr>
                     <td>Description:</td>
                     <td><asp:TextBox runat="server" ID="txtMediaDescription" Width="200px" MaxLength="100"></asp:TextBox></td>
                     <td>Url:</td>
                     <td><asp:TextBox runat="server" ID="txtMediaLink" Width="500px"></asp:TextBox></td>
                 </tr>

             </table>
             <table cellpadding="3" cellspacing="3" width="100%">
                 <tr align="right">
                     <td align="right">
                         <asp:Button ID="Button11" runat="server" Text="Save" OnClick="btnSave_Click"
                             CommandArgument="Attachment" />
                     </td>
                 </tr>
             </table>
         </Content>
         </ajaxToolkit:AccordionPane>
    </Panes>
    </ajaxToolkit:Accordion>
    
    <uc1:ModalDialog ID="modalDialog" runat="server"  Width="500" Height="300">  
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

