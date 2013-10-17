<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 262px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
  <script src="assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
  <script src="assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    
  <script type="text/javascript" language="javascript">
       //<!--
      $(document).ready(function() {
            $("input[type=submit], button").button();
    });    // Document Ready   
  //-->
  </script>
  
            <table  cellpadding="10" cellspacing="10" style="border-style:solid; border-color:Black; border-width:1px;border-collapse:collapse;text-align: center;margin-left: auto;margin-right: auto;text-align: left;" >
                <tr>
                    <td>
                        <table border="0" cellpadding="0">
                            <tr>
                                <td align="center" colspan="2" style="background:#D2D0D0; font-weight:bold; ">Please Log In</td>
                            </tr>
                            <tr><td><br /></td></tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="txtUserName">User Name:</asp:Label>
                                </td>
                                <td class="style1">
                                    <asp:TextBox ID="txtUserName" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                        ControlToValidate="txtUserName" ErrorMessage="User Name is required." 
                                        ToolTip="User Name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="txtPassword">Password:</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                        ControlToValidate="txtPassword" ErrorMessage="Password is required." 
                                        ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" style="color:Red;">
                                    <asp:Literal ID="txtFailureText" runat="server" EnableViewState="False"></asp:Literal>
                                </td>
                            </tr>
                            <tr><td><br /></td></tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In" 
                                        ValidationGroup="Login1"  onclick="LoginButton_Click" />
                                </td>
                            </tr>
                            <tr><td>&nbsp;<asp:HyperLink runat="server" ID="lnkForgot" Text="Forgot your password?" NavigateUrl="RecoverAccount.aspx"></asp:HyperLink></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <center><strong>Note: Popup blockers must be disabled on this site in order for it to work properly.<br /> For instructions on how to do this, please click on this <a href="http://www.lbl.gov/ehs/training/webcourses/globalAssets/CourseRequirements/disablePopups/disablepopups.html" target="_blank">link</a></strong></center>
</asp:Content>

