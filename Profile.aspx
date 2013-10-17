<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="ProfileEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
    <asp:Panel ID="pnlMenu" runat="server" >
    <div id="menu">
    	<ul>
		    <li><asp:HyperLink ID="HyperLink1" runat="server" Text="Home" NavigateUrl="~/Default.aspx"></asp:HyperLink></li>
	    </ul>
      <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
 <script src="assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
  <script src="assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
  <script src="assets/javascript/jquery.maskedinput.js" type="text/javascript"></script>
  
  <script type="text/javascript" language="javascript">
       //<!--
      $(document).ready(function() {
      $("input[type=submit], button").button();
      $('#<%= txtPhone.ClientID %>').mask("(999) 999-9999");
    });    // Document Ready   
  //-->
  </script>
  
    <asp:LinkButton runat="server" ID="lnkPassword" Text="Change Password" OnClick="lnkPassword_Click"></asp:LinkButton>
    <asp:LinkButton runat="server" ID="lnkProfile" Text="Change Profile"  Visible="false" OnClick="lnkProfile_Click"></asp:LinkButton>
    <br />
    <asp:Panel runat="server" ID="pnlProfie" Visible="true">
    <table  cellpadding="10" cellspacing="10" style="border-collapse:collapse;text-align: center;margin-left: auto;margin-right: auto;text-align: left;" >
        <tr>
            <td align="center" colspan="2" style="background:#F2EBDE; font-weight:bold; ">Edit Your Profile</td>
        </tr>
        <tr>
            <td><asp:Label ID="labFullName" runat="server" AssociatedControlID="txtFullName">Name:</asp:Label></td>
            <td><asp:textbox id="txtFullName" Runat="server" Width="250px" MaxLength="50"></asp:textbox>
                <asp:RequiredFieldValidator ID="FullNameRequired" runat="server" 
                                        ControlToValidate="txtFullName" ErrorMessage="Name is required." 
                                        ToolTip="Name is required." ValidationGroup="ProfileEdit">*</asp:RequiredFieldValidator></td>
        </tr>
       <tr>
            <td><asp:Label ID="labEmail" runat="server" AssociatedControlID="txtEmail">Email Address:</asp:Label></td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server"  Width="250px" MaxLength="50"></asp:TextBox>
                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" 
                      ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                      ToolTip="Email is required." ValidationGroup="ProfileEdit">*</asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="EmailRegEx" runat="server" ControlToValidate="txtEmail" 
                                ErrorMessage="Incorrect email address entered" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ProfileEdit">*</asp:RegularExpressionValidator>
                <asp:CustomValidator ID="EmailValidator" runat="server" 
                        Text="*" Visible="true" Enabled="true" Display="Dynamic" 
                        ValidateEmptyText="true" ValidationGroup="ProfileEdit"
                        ControlToValidate="txtEmail" 
                        OnServerValidate="EmailValidator_ServerValidate" ></asp:CustomValidator>
                        </td>
        </tr>
        <tr>
            <td><asp:Label ID="labPhone" runat="server" AssociatedControlID="txtPhone">Phone Number:</asp:Label></td>
            <td><asp:TextBox ID="txtPhone" runat="server"  Width="250px" MaxLength="14"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" colspan="2">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="true" ValidationGroup="ProfileEdit" onclick="btnSubmit_Click"/>
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" onclick="btnCancel_Click"  />
            </td>
        </tr>
        <tr>
            <td colspan="2">
           <asp:ValidationSummary runat="server" ID="MyValidationSummary" ValidationGroup="ProfileEdit" DisplayMode="BulletList" CssClass="validationtext" ShowSummary="true"/></td>
        </tr>
    </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnlPassword" Visible="false">
     <table  cellpadding="10" cellspacing="10" style="border-collapse:collapse;text-align: center;margin-left: auto;margin-right: auto;text-align: left;" >
        <tr>
            <td align="center" colspan="2" style="background:#F2EBDE; font-weight:bold; ">Change Your Password</td>
        </tr>
     <tr>
            <td><asp:Label ID="labCurrentPassword" runat="server" AssociatedControlID="txtCurrentPassword">Current Password:</asp:Label></td>
            <td><asp:TextBox ID="txtCurrentPassword" runat="server"  Width="250px" MaxLength="50" TextMode="Password"></asp:TextBox>
             <asp:CustomValidator ID="CurrentPasswordValidator" runat="server" 
                        Text="*" EnableClientScript="false" Display="Dynamic" 
                        ValidateEmptyText="true" ValidationGroup="PasswordEdit"
                        ControlToValidate="txtCurrentPassword" 
                        OnServerValidate="CurrentPasswordValidator_ServerValidate" ></asp:CustomValidator></td>
        </tr>
        <tr>
            <td><asp:Label ID="labNewPassword" runat="server" AssociatedControlID="txtNewPassword">New Password:</asp:Label></td>
            <td><asp:TextBox ID="txtNewPassword" runat="server"  Width="250px" MaxLength="50" TextMode="Password"></asp:TextBox>
             <asp:CustomValidator ID="NewPasswordValidator" runat="server" 
                        Text="*" Visible="true" Enabled="true" Display="Dynamic" 
                        ValidateEmptyText="true" ValidationGroup="PasswordEdit"
                        ControlToValidate="txtNewPassword" 
                        OnServerValidate="NewPasswordValidator_ServerValidate" ></asp:CustomValidator></td>
        </tr>
        <tr>
            <td><asp:Label ID="labConfirm" runat="server" AssociatedControlID="txtConfirm" >Confirm Password:</asp:Label></td>
            <td><asp:TextBox ID="txtConfirm" runat="server"  Width="250px" MaxLength="50" TextMode="Password"></asp:TextBox>
             <asp:CustomValidator ID="ConfirmValidator" runat="server" 
                        Text="*" Visible="true" Enabled="true" Display="Dynamic" 
                        ValidateEmptyText="true" ValidationGroup="PasswordEdit"
                        ControlToValidate="txtConfirm" 
                        OnServerValidate="ConfirmValidator_ServerValidate" ></asp:CustomValidator></td>
        </tr>
        <tr><td colspan="2"><span>Passwords should be 8 characters in length and contain at least 1 number or special character</span></td></tr>
        <tr>
            <td colspan="2">
           <asp:ValidationSummary runat="server" ID="ValidationSummary1" ValidationGroup="PasswordEdit" DisplayMode="BulletList" CssClass="validationtext" ShowSummary="true" /></td>
        </tr>
        <tr><td></td></tr>
         <tr>
            <td align="right" colspan="2">
                <asp:Button ID="btnSavePassword" runat="server" Text="Save" CausesValidation="true" ValidationGroup="PasswordEdit" onclick="btnSavePassword_Click"/>
                <asp:Button ID="btnCancelPassword" runat="server" Text="Cancel" onclick="btnCancelPassword_Click"  />
            </td>
        </tr>
    </table>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

